[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$solution = Join-Path $PSScriptRoot '..\SASD.Bewerbungsmanager.sln'

Write-Host '==> Restore'
dotnet restore $solution

Write-Host '==> Release build'
dotnet build $solution -c Release --no-restore

Write-Host '==> Tests'
& (Join-Path $PSScriptRoot 'Verify-Tests.ps1') -Configuration Release

Write-Host 'Verification completed successfully.'
