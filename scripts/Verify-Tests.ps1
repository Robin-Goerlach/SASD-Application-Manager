[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$resultsDirectory = Join-Path $repositoryRoot 'TestResults\discovery-gate'

# Keep the list explicit. A newly added test project must be deliberately included in this gate;
# otherwise a solution-level green exit code could hide a project for which no tests were run.
$testProjects = @(
    'tests\SASD.Bewerbungsmanager.Domain.Tests\SASD.Bewerbungsmanager.Domain.Tests.csproj',
    'tests\SASD.Bewerbungsmanager.Application.Tests\SASD.Bewerbungsmanager.Application.Tests.csproj',
    'tests\SASD.Bewerbungsmanager.Infrastructure.Tests\SASD.Bewerbungsmanager.Infrastructure.Tests.csproj',
    'tests\SASD.Bewerbungsmanager.Presentation.Tests\SASD.Bewerbungsmanager.Presentation.Tests.csproj',
    'tests\SASD.Bewerbungsmanager.SystemTests\SASD.Bewerbungsmanager.SystemTests.csproj'
)

if (Test-Path -LiteralPath $resultsDirectory) {
    # This path is fixed below the repository's ignored TestResults directory. Removing only this
    # gate-owned directory prevents stale TRX files from being mistaken for the current run.
    Remove-Item -LiteralPath $resultsDirectory -Recurse -Force
}

New-Item -ItemType Directory -Path $resultsDirectory -Force | Out-Null
$summary = @()

foreach ($relativeProject in $testProjects) {
    $projectPath = Join-Path $repositoryRoot $relativeProject
    if (-not (Test-Path -LiteralPath $projectPath -PathType Leaf)) {
        throw "Required test project was not found: $relativeProject"
    }

    $projectName = [System.IO.Path]::GetFileNameWithoutExtension($projectPath)
    $resultFileName = "$projectName.trx"

    Write-Host "==> Testing $projectName"
    & dotnet test $projectPath `
        -c $Configuration `
        --no-build `
        --no-restore `
        --logger "trx;LogFileName=$resultFileName" `
        --results-directory $resultsDirectory

    if ($LASTEXITCODE -ne 0) {
        throw "Test execution failed for $projectName with exit code $LASTEXITCODE."
    }

    $resultPath = Join-Path $resultsDirectory $resultFileName
    if (-not (Test-Path -LiteralPath $resultPath -PathType Leaf)) {
        throw "Test execution for $projectName produced no TRX result file."
    }

    [xml]$testRun = Get-Content -LiteralPath $resultPath -Raw
    $counters = $testRun.SelectSingleNode("//*[local-name()='Counters']")
    if ($null -eq $counters) {
        throw "The TRX result for $projectName contains no test counters."
    }

    $total = [int]$counters.GetAttribute('total')
    $executed = [int]$counters.GetAttribute('executed')
    $passed = [int]$counters.GetAttribute('passed')
    $failed = [int]$counters.GetAttribute('failed')

    if ($total -lt 1 -or $executed -lt 1) {
        throw "No tests were discovered and executed for $projectName."
    }

    if ($failed -gt 0) {
        throw "$failed tests failed in $projectName."
    }

    $summary += [PSCustomObject]@{
        Project = $projectName
        Total = $total
        Executed = $executed
        Passed = $passed
    }
}

Write-Host ''
Write-Host 'Verified test discovery and execution:'
$summary | Format-Table -AutoSize
Write-Host ("Total executed tests: {0}" -f (($summary | Measure-Object -Property Executed -Sum).Sum))
