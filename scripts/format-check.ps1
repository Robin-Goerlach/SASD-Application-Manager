[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$solution = Join-Path $PSScriptRoot '..\SASD.Bewerbungsmanager.sln'

dotnet restore $solution
dotnet format $solution --verify-no-changes --no-restore
