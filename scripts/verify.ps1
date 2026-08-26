[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$solution = Join-Path $PSScriptRoot '..\SASD.Bewerbungsmanager.sln'

Write-Host '==> Restore'
dotnet restore $solution

Write-Host '==> Release build'
dotnet build $solution -c Release --no-restore

Write-Host '==> Tests'
dotnet test $solution -c Release --no-build

Write-Host 'Verification completed successfully.'
