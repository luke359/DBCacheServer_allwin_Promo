param(
    [Parameter(Mandatory = $true)][string]$ConnectionFile,
    [Parameter(Mandatory = $true)][string]$DatabaseName,
    [string]$MysqlExe = 'C:\Program Files\MySQL\MySQL Server 9.7\bin\mysql.exe'
)

$ErrorActionPreference = 'Stop'
$migrationFile = Join-Path $PSScriptRoot 'V004__bonus_status_daily_trigger_days.sql'
$checksumFile = Join-Path $PSScriptRoot 'V004.sha256'

if ($DatabaseName -cnotmatch '^promotion_test_[A-Za-z0-9_]+$') {
    throw 'Only a dedicated promotion_test_ database is accepted by this test runner.'
}
if (-not (Test-Path -LiteralPath $ConnectionFile -PathType Leaf) -or
    -not (Test-Path -LiteralPath $MysqlExe -PathType Leaf)) {
    throw 'Connection file or mysql client was not found.'
}

$actualHash = (Get-FileHash -LiteralPath $migrationFile -Algorithm SHA256).Hash.ToLowerInvariant()
$expectedHash = (Get-Content -LiteralPath $checksumFile -Raw -Encoding UTF8).Trim().ToLowerInvariant()
if ($actualHash -cne $expectedHash) { throw 'V004 file hash differs from the pinned SHA-256.' }

$priorHashes = @{}
foreach ($version in @('V001', 'V002', 'V003')) {
    $priorHashes[$version] = (Get-Content -LiteralPath (Join-Path $PSScriptRoot "$version.sha256") -Raw -Encoding UTF8).Trim().ToLowerInvariant()
}

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

function Assert-V004Schema {
    $columns = @(Invoke-MySql @'
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='PromotionBonusStatus'
  AND COLUMN_NAME IN ('LastFirstLoginBusinessDay','LastFirstDepositBusinessDay')
ORDER BY COLUMN_NAME
'@)
    if ($columns.Count -ne 2) { throw 'V004 column count is incorrect.' }
    foreach ($column in $columns) {
        $fields = $column -split "`t"
        if ($fields.Count -ne 4 -or $fields[1] -cne 'date' -or
            $fields[2] -cne 'YES' -or $fields[3] -cne 'NULL') {
            throw 'V004 column metadata differs from PCS-02.'
        }
    }
}

$env:MYSQL_PWD = $passwordValue
try {
    foreach ($version in @('V001', 'V002', 'V003')) {
        $record = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = '$version'")
        if ($record.Count -ne 1) { throw "$version audit record is missing." }
        $fields = $record[0] -split "`t", 2
        if ($fields[0] -cne $priorHashes[$version] -or $fields[1] -cne 'SUCCEEDED') {
            throw "$version audit record is not a verified success."
        }
    }

    $v004 = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V004'")
    if ($v004.Count -gt 0) {
        $fields = $v004[0] -split "`t", 2
        if ($fields[0] -cne $actualHash -or $fields[1] -cne 'SUCCEEDED') {
            throw 'V004 is incomplete or its hash differs; inspect partial DDL before proceeding.'
        }
        Assert-V004Schema
        Write-Output "V004 already verified for $DatabaseName; SHA-256=$actualHash"
        return
    }

    $before = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='PromotionBonusStatus' AND COLUMN_NAME IN ('LastFirstLoginBusinessDay','LastFirstDepositBusinessDay')")
    if ($before.Count -ne 1 -or $before[0] -ne '0') {
        throw 'PromotionBonusStatus is not the expected V003 schema; refusing V004.'
    }

    Invoke-MySql "INSERT INTO ``PromotionSchemaMigration`` (Version, FileSha256, StartedAt, Status) VALUES ('V004', '$actualHash', NOW(6), 'STARTED')" | Out-Null
    try {
        Invoke-MySql (Get-Content -LiteralPath $migrationFile -Raw -Encoding UTF8) | Out-Null
        Assert-V004Schema
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'SUCCEEDED', CompletedAt = NOW(6) WHERE Version = 'V004' AND Status = 'STARTED'" | Out-Null
    } catch {
        try {
            Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'FAILED', CompletedAt = NOW(6) WHERE Version = 'V004'" | Out-Null
        } catch { }
        throw
    }
    Write-Output "V004 verified for $DatabaseName; SHA-256=$actualHash"
} finally {
    Remove-Item Env:MYSQL_PWD -ErrorAction SilentlyContinue
}
