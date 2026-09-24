param(
    [Parameter(Mandatory = $true)][string]$ConnectionFile,
    [Parameter(Mandatory = $true)][string]$DatabaseName,
    [string]$MysqlExe = 'C:\Program Files\MySQL\MySQL Server 9.7\bin\mysql.exe'
)

$ErrorActionPreference = 'Stop'
$migrationFile = Join-Path $PSScriptRoot 'V003__rename_promotion_bonus_tables.sql'
$checksumFile = Join-Path $PSScriptRoot 'V003.sha256'
$v001ChecksumFile = Join-Path $PSScriptRoot 'V001.sha256'
$v002ChecksumFile = Join-Path $PSScriptRoot 'V002.sha256'

if ($DatabaseName -cnotmatch '^promotion_test_[A-Za-z0-9_]+$') {
    throw 'Only a dedicated promotion_test_ database is accepted by this test runner.'
}
if (-not (Test-Path -LiteralPath $ConnectionFile -PathType Leaf) -or
    -not (Test-Path -LiteralPath $MysqlExe -PathType Leaf)) {
    throw 'Connection file or mysql client was not found.'
}

$actualHash = (Get-FileHash -LiteralPath $migrationFile -Algorithm SHA256).Hash.ToLowerInvariant()
$expectedHash = (Get-Content -LiteralPath $checksumFile -Raw -Encoding UTF8).Trim().ToLowerInvariant()
if ($actualHash -cne $expectedHash) { throw 'V003 file hash differs from the pinned SHA-256.' }
$v001Hash = (Get-Content -LiteralPath $v001ChecksumFile -Raw -Encoding UTF8).Trim().ToLowerInvariant()
$v002Hash = (Get-Content -LiteralPath $v002ChecksumFile -Raw -Encoding UTF8).Trim().ToLowerInvariant()

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

function Assert-V003Schema {
    $newTables = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('PromotionBonusStatus','PromotionBonusHistory')")
    $oldTables = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('BonusStatus','BonusHistory')")
    if ($newTables[0] -ne '2' -or $oldTables[0] -ne '0') {
        throw 'V003 table names are incomplete.'
    }
    $newSchemaObjects = @(Invoke-MySql @'
SELECT COUNT(*)
FROM (
    SELECT DISTINCT INDEX_NAME AS ObjectName
    FROM information_schema.STATISTICS
    WHERE TABLE_SCHEMA=DATABASE()
      AND TABLE_NAME IN ('PromotionBonusStatus','PromotionBonusHistory')
      AND INDEX_NAME LIKE '%PromotionBonus%'
    UNION ALL
    SELECT CONSTRAINT_NAME
    FROM information_schema.REFERENTIAL_CONSTRAINTS
    WHERE CONSTRAINT_SCHEMA=DATABASE()
      AND TABLE_NAME IN ('PromotionBonusStatus','PromotionBonusHistory')
      AND CONSTRAINT_NAME LIKE 'FK_PromotionBonus%'
) AS SchemaObjects
'@)
    if ($newSchemaObjects[0] -ne '13') { throw 'V003 index or foreign-key names are incomplete.' }
}

$env:MYSQL_PWD = $passwordValue
try {
    $v001 = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V001'")
    $v002 = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V002'")
    if ($v001.Count -ne 1 -or $v002.Count -ne 1) { throw 'V001 or V002 audit record is missing.' }
    $v001Fields = $v001[0] -split "`t", 2
    $v002Fields = $v002[0] -split "`t", 2
    if ($v001Fields[0] -cne $v001Hash -or $v001Fields[1] -cne 'SUCCEEDED' -or
        $v002Fields[0] -cne $v002Hash -or $v002Fields[1] -cne 'SUCCEEDED') {
        throw 'V001 or V002 audit record is not a verified success.'
    }

    $v003 = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V003'")
    if ($v003.Count -gt 0) {
        $fields = $v003[0] -split "`t", 2
        if ($fields[0] -cne $actualHash -or $fields[1] -cne 'SUCCEEDED') {
            throw 'V003 is incomplete or its hash differs; inspect partial DDL before proceeding.'
        }
        Assert-V003Schema
        Write-Output "V003 already verified for $DatabaseName; SHA-256=$actualHash"
        return
    }

    $oldTables = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('BonusStatus','BonusHistory')")
    $newTables = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('PromotionBonusStatus','PromotionBonusHistory')")
    if (($oldTables[0] -ne '2' -or $newTables[0] -ne '0') -and
        ($oldTables[0] -ne '0' -or $newTables[0] -ne '2')) {
        throw 'Bonus table names are in a mixed or unexpected state; refusing V003.'
    }

    Invoke-MySql "INSERT INTO ``PromotionSchemaMigration`` (Version, FileSha256, StartedAt, Status) VALUES ('V003', '$actualHash', NOW(6), 'STARTED')" | Out-Null
    try {
        if ($oldTables[0] -eq '2') {
            Invoke-MySql (Get-Content -LiteralPath $migrationFile -Raw -Encoding UTF8) | Out-Null
        }
        Assert-V003Schema
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'SUCCEEDED', CompletedAt = NOW(6) WHERE Version = 'V003' AND Status = 'STARTED'" | Out-Null
    } catch {
        try {
            Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'FAILED', CompletedAt = NOW(6) WHERE Version = 'V003'" | Out-Null
        } catch { }
        throw
    }
    Write-Output "V003 verified for $DatabaseName; SHA-256=$actualHash"
} finally {
    Remove-Item Env:MYSQL_PWD -ErrorAction SilentlyContinue
}
