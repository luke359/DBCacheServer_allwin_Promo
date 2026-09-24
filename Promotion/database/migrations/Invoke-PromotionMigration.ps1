param(
    [Parameter(Mandatory = $true)][string]$ConnectionFile,
    [Parameter(Mandatory = $true)][string]$DatabaseName,
    [string]$MysqlExe = 'C:\Program Files\MySQL\MySQL Server 9.7\bin\mysql.exe'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$migrationFile = Join-Path $PSScriptRoot 'V001__create_promotion_core_tables.sql'
$checksumFile = Join-Path $PSScriptRoot 'V001.sha256'
$verificationFile = Join-Path $root 'verification\V001__verify_promotion_core_schema.sql'
$fingerprintFile = Join-Path $root 'verification\V001__expected_fingerprints.tsv'

if ($DatabaseName -cnotmatch '^promotion_test_[A-Za-z0-9_]+$') {
    throw 'Only a dedicated promotion_test_ database is accepted by this test runner.'
}
if (-not (Test-Path -LiteralPath $ConnectionFile -PathType Leaf) -or
    -not (Test-Path -LiteralPath $MysqlExe -PathType Leaf)) {
    throw 'Connection file or mysql client was not found.'
}
$actualHash = (Get-FileHash -LiteralPath $migrationFile -Algorithm SHA256).Hash.ToLowerInvariant()
$expectedHash = (Get-Content -LiteralPath $checksumFile -Raw -Encoding UTF8).Trim().ToLowerInvariant()
if ($actualHash -cne $expectedHash) { throw 'V001 file hash differs from the pinned SHA-256.' }
if (-not (Test-Path -LiteralPath $fingerprintFile -PathType Leaf)) {
    throw 'Pinned V001 schema fingerprints are missing.'
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

$env:MYSQL_PWD = $passwordValue
try {
    $databaseExists = @(Invoke-MySql 'SELECT COUNT(*) FROM information_schema.SCHEMATA WHERE SCHEMA_NAME = DATABASE()')
    if ($databaseExists.Count -ne 1 -or $databaseExists[0] -ne '1') { throw 'Target database does not exist.' }
    $serverSettings = @(Invoke-MySql 'SELECT VERSION(), @@sql_mode, @@lower_case_table_names')
    $expectedMode = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION'
    if ($serverSettings.Count -ne 1) { throw 'Unable to read server settings.' }
    $serverFields = $serverSettings[0] -split "`t", 3
    if ($serverFields.Count -ne 3 -or $serverFields[0] -ne '9.7.0' -or
        $serverFields[1] -ne $expectedMode -or $serverFields[2] -ne '1') {
        throw 'Server version, sql_mode, or lower_case_table_names differs from the tested V001 baseline.'
    }

    $metadataTable = 'PromotionSchemaMigration'
    $metadataExists = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = '$metadataTable'")
    if ($metadataExists[0] -eq '0') {
        $tableCount = @(Invoke-MySql 'SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE()')
        if ($tableCount[0] -ne '0') { throw 'Target database is not empty; inspect before adoption.' }
        Invoke-MySql @'
CREATE TABLE `PromotionSchemaMigration` (
    `Version` VARCHAR(16) NOT NULL,
    `FileSha256` CHAR(64) NOT NULL,
    `StartedAt` DATETIME(6) NOT NULL,
    `CompletedAt` DATETIME(6) NULL,
    `Status` VARCHAR(24) NOT NULL,
    PRIMARY KEY (`Version`)
) ENGINE=InnoDB DEFAULT CHARACTER SET=utf8mb4 COLLATE=utf8mb4_bin
'@ | Out-Null
    }

    $record = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V001'")
    if ($record.Count -gt 0) {
        $fields = $record[0] -split "`t", 2
        if ($fields[0] -cne $actualHash) { throw 'V001 has the same version but a different file hash.' }
        if ($fields[1] -notin @('SUCCEEDED', 'PENDING_VERIFICATION')) {
            throw 'V001 did not finish previously; inspect partial DDL before any further action.'
        }
    } else {
        $coreCount = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME IN ('PromotionActivity','PromotionTriggerEvent','EligibilityEntry','BonusStatus','BonusHistory')")
        if ($coreCount[0] -ne '0') { throw 'Core tables already exist; refusing to apply V001.' }
        Invoke-MySql "INSERT INTO ``PromotionSchemaMigration`` (Version, FileSha256, StartedAt, Status) VALUES ('V001', '$actualHash', NOW(6), 'STARTED')" | Out-Null
        try {
            $migrationSql = Get-Content -LiteralPath $migrationFile -Raw -Encoding UTF8
            Invoke-MySql $migrationSql | Out-Null
            Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'PENDING_VERIFICATION' WHERE Version = 'V001'" | Out-Null
        } catch {
            try {
                Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'FAILED', CompletedAt = NOW(6) WHERE Version = 'V001'" | Out-Null
            } catch { }
            throw
        }
    }

    $actualFingerprints = @(Invoke-MySql (Get-Content -LiteralPath $verificationFile -Raw -Encoding UTF8))
    $tableCount = @(Invoke-MySql 'SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE()')
    if ($tableCount[0] -ne '6') { throw 'Unexpected table exists or expected table is missing.' }
    if ($actualFingerprints.Count -ne 4 -or
        $actualFingerprints[0] -notmatch '^tables\s+5\s+' -or
        $actualFingerprints[1] -notmatch '^columns\s+69\s+' -or
        $actualFingerprints[3] -notmatch '^foreign_keys\s+6\s+') {
        throw 'V001 metadata counts do not match PCS-02.'
    }
    $expectedFingerprints = @(Get-Content -LiteralPath $fingerprintFile -Encoding UTF8)
    if ($expectedFingerprints.Count -ne 4 -or
        (Compare-Object -ReferenceObject $expectedFingerprints -DifferenceObject $actualFingerprints -SyncWindow 0)) {
        throw 'Schema fingerprint differs from the pinned V001 manifest.'
    }
    Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'SUCCEEDED', CompletedAt = NOW(6) WHERE Version = 'V001' AND Status = 'PENDING_VERIFICATION'" | Out-Null
    Write-Output "V001 verified for $DatabaseName; SHA-256=$actualHash"
} finally {
    Remove-Item Env:MYSQL_PWD -ErrorAction SilentlyContinue
}
