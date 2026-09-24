param(
    [Parameter(Mandatory = $true)][string]$ConnectionFile,
    [Parameter(Mandatory = $true)][string]$DatabaseName,
    [string]$MysqlExe = 'C:\Program Files\MySQL\MySQL Server 9.7\bin\mysql.exe'
)

$ErrorActionPreference = 'Stop'
$migrationFile = Join-Path $PSScriptRoot 'V002__promotion_activity_new_fields.sql'
$checksumFile = Join-Path $PSScriptRoot 'V002.sha256'
$v001ChecksumFile = Join-Path $PSScriptRoot 'V001.sha256'
if ($DatabaseName -cnotmatch '^promotion_test_[A-Za-z0-9_]+$') {
    throw 'Only a dedicated promotion_test_ database is accepted by this test runner.'
}
if (-not (Test-Path -LiteralPath $ConnectionFile -PathType Leaf) -or
    -not (Test-Path -LiteralPath $MysqlExe -PathType Leaf)) {
    throw 'Connection file or mysql client was not found.'
}
$actualHash = (Get-FileHash -LiteralPath $migrationFile -Algorithm SHA256).Hash.ToLowerInvariant()
$expectedHash = (Get-Content -LiteralPath $checksumFile -Raw -Encoding UTF8).Trim().ToLowerInvariant()
if ($actualHash -cne $expectedHash) { throw 'V002 file hash differs from the pinned SHA-256.' }
$v001Hash = (Get-Content -LiteralPath $v001ChecksumFile -Raw -Encoding UTF8).Trim().ToLowerInvariant()

$settings = @{}
foreach ($line in Get-Content -LiteralPath $ConnectionFile -Encoding UTF8) {
    $pair = $line -split ':', 2
    if ($pair.Count -eq 2) { $settings[$pair[0].Trim()] = $pair[1].Trim() }
}
if (-not $settings.ContainsKey('mysqlConnection')) { throw 'mysqlConnection is missing.' }
$builder = [System.Data.Common.DbConnectionStringBuilder]::new()
[System.Data.Common.DbConnectionStringBuilder].GetProperty('ConnectionString').SetValue($builder, $settings['mysqlConnection'])
function Get-ConnectionValue([string]$key) {
    $item = $null
    if ($builder.TryGetValue($key, [ref]$item)) { return [string]$item }
    throw "Missing connection key: $key"
}
$hostName = Get-ConnectionValue 'Server'
$portValue = Get-ConnectionValue 'Port'
$userValue = Get-ConnectionValue 'UserID'
$passwordValue = Get-ConnectionValue 'Pwd'
if ($hostName -notmatch '^(localhost|127\.0\.0\.1|::1)$' -or $portValue -notmatch '^\d{1,5}$') {
    throw 'This test runner only accepts a loopback MySQL server.'
}

function Invoke-MySql([string]$sql) {
    $mysqlArgs = @('--protocol=tcp', "--host=$hostName", "--port=$portValue",
        "--user=$userValue", "--database=$DatabaseName", '--connect-timeout=5',
        '--batch', '--skip-column-names', "--execute=$sql")
    $output = @(& $MysqlExe @mysqlArgs 2>&1)
    if ($LASTEXITCODE -ne 0) { throw "MySQL operation failed: $($output -join ' ')" }
    return $output
}

function Assert-V002Schema {
    $columns = @(Invoke-MySql @'
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT, CHARACTER_MAXIMUM_LENGTH
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='PromotionActivity'
  AND COLUMN_NAME IN ('WagerCalculationType','MaxBetAmount','GameServerList','MaxBalanceConvertedAmount')
ORDER BY COLUMN_NAME
'@)
    if ($columns.Count -ne 4) { throw 'V002 column count is incorrect.' }
    for ($i = 0; $i -lt $columns.Count; $i++) {
        $actual = ($columns[$i] -split "`t")
        if ($actual.Count -ne 5) { throw 'V002 column metadata shape is incorrect.' }
        if ($actual[0] -cne @('GameServerList','MaxBalanceConvertedAmount','MaxBetAmount','WagerCalculationType')[$i] -or
            $actual[1] -cne @('varchar','int','int','tinyint')[$i] -or
            $actual[2] -cne @('YES','YES','YES','NO')[$i] -or
            $actual[3] -cne @('NULL','150','5','1')[$i] -or
            $actual[4] -cne @('500','NULL','NULL','NULL')[$i]) {
            throw 'V002 column metadata differs from PCS-02.'
        }
    }
}

$env:MYSQL_PWD = $passwordValue
try {
    $databaseExists = @(Invoke-MySql 'SELECT COUNT(*) FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = DATABASE()')
    if ($databaseExists.Count -ne 1 -or $databaseExists[0] -ne '1') { throw 'Target database does not exist.' }
    $v001 = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V001'")
    if ($v001.Count -ne 1) { throw 'V001 audit record is missing.' }
    $v001Fields = $v001[0] -split "`t", 2
    if ($v001Fields[0] -cne $v001Hash -or $v001Fields[1] -cne 'SUCCEEDED') {
        throw 'V001 audit record is not a verified success.'
    }
    $v002 = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V002'")
    if ($v002.Count -gt 0) {
        $fields = $v002[0] -split "`t", 2
        if ($fields[0] -cne $actualHash -or $fields[1] -cne 'SUCCEEDED') {
            throw 'V002 is incomplete or its hash differs; inspect partial DDL before proceeding.'
        }
        Assert-V002Schema
        Write-Output "V002 already verified for $DatabaseName; SHA-256=$actualHash"
        return
    }

    $before = @(Invoke-MySql @'
SELECT COLUMN_NAME, IS_NULLABLE FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='PromotionActivity'
  AND COLUMN_NAME IN ('WagerCalculationType','MaxBetAmount','GameServerList','MaxBalanceConvertedAmount')
ORDER BY COLUMN_NAME
'@)
    if ($before.Count -ne 2 -or $before[0] -cne "MaxBalanceConvertedAmount`tNO" -or
        $before[1] -cne "MaxBetAmount`tNO") {
        throw 'PromotionActivity is not the expected V001 schema; refusing V002.'
    }

    Invoke-MySql "INSERT INTO ``PromotionSchemaMigration`` (Version, FileSha256, StartedAt, Status) VALUES ('V002', '$actualHash', NOW(6), 'STARTED')" | Out-Null
    try {
        Invoke-MySql (Get-Content -LiteralPath $migrationFile -Raw -Encoding UTF8) | Out-Null
        Assert-V002Schema
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'SUCCEEDED', CompletedAt = NOW(6) WHERE Version = 'V002' AND Status = 'STARTED'" | Out-Null
    } catch {
        try {
            Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'FAILED', CompletedAt = NOW(6) WHERE Version = 'V002'" | Out-Null
        } catch { }
        throw
    }
    Write-Output "V002 verified for $DatabaseName; SHA-256=$actualHash"
} finally {
    Remove-Item Env:MYSQL_PWD -ErrorAction SilentlyContinue
}
