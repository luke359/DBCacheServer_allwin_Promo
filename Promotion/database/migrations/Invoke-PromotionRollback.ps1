[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = 'High')]
param(
    [Parameter(Mandatory = $true)][string]$ConnectionFile,
    [Parameter(Mandatory = $true)][string]$DatabaseName,
    [Parameter(Mandatory = $true)][switch]$AllowEmptySchemaRollback,
    [string]$MysqlExe = 'C:\Program Files\MySQL\MySQL Server 9.7\bin\mysql.exe'
)

$ErrorActionPreference = 'Stop'
if (-not $AllowEmptySchemaRollback) { throw 'Explicit -AllowEmptySchemaRollback is required.' }
if ($DatabaseName -cnotmatch '^promotion_test_[A-Za-z0-9_]+$') {
    throw 'Rollback is restricted to a dedicated promotion_test_ database.'
}
if (-not (Test-Path -LiteralPath $ConnectionFile -PathType Leaf) -or
    -not (Test-Path -LiteralPath $MysqlExe -PathType Leaf)) {
    throw 'Connection file or mysql client was not found.'
}

$settings = @{}
foreach ($line in Get-Content -LiteralPath $ConnectionFile -Encoding UTF8) {
    $pair = $line -split ':', 2
    if ($pair.Count -eq 2) { $settings[$pair[0].Trim()] = $pair[1].Trim() }
}
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
    throw 'Rollback is restricted to a loopback MySQL server.'
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
    $expectedHash = (Get-Content -LiteralPath (Join-Path $PSScriptRoot 'V001.sha256') -Raw -Encoding UTF8).Trim().ToLowerInvariant()
    $expectedV002Hash = (Get-Content -LiteralPath (Join-Path $PSScriptRoot 'V002.sha256') -Raw -Encoding UTF8).Trim().ToLowerInvariant()
    $expectedV003Hash = (Get-Content -LiteralPath (Join-Path $PSScriptRoot 'V003.sha256') -Raw -Encoding UTF8).Trim().ToLowerInvariant()
    $expectedV004Hash = (Get-Content -LiteralPath (Join-Path $PSScriptRoot 'V004.sha256') -Raw -Encoding UTF8).Trim().ToLowerInvariant()
    $expectedV005Hash = (Get-Content -LiteralPath (Join-Path $PSScriptRoot 'V005.sha256') -Raw -Encoding UTF8).Trim().ToLowerInvariant()
    $record = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V001'")
    if ($record.Count -ne 1) { throw 'V001 migration record is missing.' }
    $fields = $record[0] -split "`t", 2
    if ($fields.Count -ne 2 -or $fields[0] -cne $expectedHash -or $fields[1] -ne 'SUCCEEDED') {
        throw 'V001 migration record is not the exact successful version.'
    }
    $v002Record = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V002'")
    if ($v002Record.Count -ne 1) { throw 'V002 migration record is missing.' }
    $v002Fields = $v002Record[0] -split "`t", 2
    if ($v002Fields.Count -ne 2 -or $v002Fields[0] -cne $expectedV002Hash -or $v002Fields[1] -ne 'SUCCEEDED') {
        throw 'V002 migration record is not the exact successful version.'
    }
    $v003Record = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V003'")
    if ($v003Record.Count -ne 1) { throw 'V003 migration record is missing.' }
    $v003Fields = $v003Record[0] -split "`t", 2
    if ($v003Fields.Count -ne 2 -or $v003Fields[0] -cne $expectedV003Hash -or $v003Fields[1] -ne 'SUCCEEDED') {
        throw 'V003 migration record is not the exact successful version.'
    }
    $v004Record = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V004'")
    if ($v004Record.Count -ne 1) { throw 'V004 migration record is missing.' }
    $v004Fields = $v004Record[0] -split "`t", 2
    if ($v004Fields.Count -ne 2 -or $v004Fields[0] -cne $expectedV004Hash -or $v004Fields[1] -ne 'SUCCEEDED') {
        throw 'V004 migration record is not the exact successful version.'
    }
    $v005Record = @(Invoke-MySql "SELECT FileSha256, Status FROM ``PromotionSchemaMigration`` WHERE Version = 'V005'")
    if ($v005Record.Count -ne 1) { throw 'V005 migration record is missing.' }
    $v005Fields = $v005Record[0] -split "`t", 2
    if ($v005Fields.Count -ne 2 -or $v005Fields[0] -cne $expectedV005Hash -or $v005Fields[1] -ne 'SUCCEEDED') {
        throw 'V005 migration record is not the exact successful version.'
    }
    $schemaCount = @(Invoke-MySql 'SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE()')
    if ($schemaCount[0] -ne '6') { throw 'Unexpected tables exist; rollback refused.' }
    $rowCount = @(Invoke-MySql @'
SELECT (SELECT COUNT(*) FROM `PromotionActivity`)
     + (SELECT COUNT(*) FROM `PromotionTriggerEvent`)
     + (SELECT COUNT(*) FROM `EligibilityEntry`)
     + (SELECT COUNT(*) FROM `PromotionBonusStatus`)
     + (SELECT COUNT(*) FROM `PromotionBonusHistory`)
'@)
    if ($rowCount[0] -ne '0') { throw 'At least one core table has data; rollback refused.' }
    $renamedTableCount = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('PromotionBonusStatus','PromotionBonusHistory')")
    $legacyTableCount = @(Invoke-MySql "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('BonusStatus','BonusHistory')")
    if ($renamedTableCount[0] -ne '2' -or $legacyTableCount[0] -ne '0') {
        throw 'V003 table names differ from the expected schema; rollback refused.'
    }

    if ($PSCmdlet.ShouldProcess($DatabaseName, 'Drop five empty V001 core tables in reverse FK order')) {
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLBACK_STARTED' WHERE Version = 'V001' AND Status = 'SUCCEEDED'" | Out-Null
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLBACK_STARTED' WHERE Version = 'V002' AND Status = 'SUCCEEDED'" | Out-Null
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLBACK_STARTED' WHERE Version = 'V003' AND Status = 'SUCCEEDED'" | Out-Null
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLBACK_STARTED' WHERE Version = 'V004' AND Status = 'SUCCEEDED'" | Out-Null
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLBACK_STARTED' WHERE Version = 'V005' AND Status = 'SUCCEEDED'" | Out-Null
        foreach ($table in @('PromotionBonusHistory', 'PromotionBonusStatus', 'EligibilityEntry', 'PromotionTriggerEvent', 'PromotionActivity')) {
            Invoke-MySql "DROP TABLE ``$table``" | Out-Null
        }
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLED_BACK', CompletedAt = NOW(6) WHERE Version = 'V001'" | Out-Null
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLED_BACK', CompletedAt = NOW(6) WHERE Version = 'V002'" | Out-Null
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLED_BACK', CompletedAt = NOW(6) WHERE Version = 'V003'" | Out-Null
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLED_BACK', CompletedAt = NOW(6) WHERE Version = 'V004'" | Out-Null
        Invoke-MySql "UPDATE ``PromotionSchemaMigration`` SET Status = 'ROLLED_BACK', CompletedAt = NOW(6) WHERE Version = 'V005'" | Out-Null
        Write-Output "V001-V005 empty schema rolled back in $DatabaseName; migration audit retained."
    }
} finally {
    [Environment]::SetEnvironmentVariable('MYSQL_PWD', $null, 'Process')
}
