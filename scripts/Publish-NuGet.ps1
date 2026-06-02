param (
	[Parameter(Mandatory=$false)]
	[string]$target = "NuGet.org"
)

function ShowStep($text) {
	Write-Host -Object "-----$text-----" -ForegroundColor Magenta	
}

$ErrorActionPreference = "Stop"

$config = "Release"
$solutionFolder = Split-Path -Path $PSScriptRoot
$solution = Get-ChildItem -Path $solutionFolder -Filter *.slnx
$projectFolder = Join-Path -Path $solutionFolder -ChildPath "src"
$project = Get-ChildItem -Path $projectFolder -Filter *.csproj
$outputFolder = Join-Path -Path $PSScriptRoot -ChildPath "output"

if (-not $solution) {
	Write-Error -Message "Solution file (*.slnx) not found in: $solutionFolder"
}
if (-not $project) {
	Write-Error -Message "Project file (*.csproj) not found in: $projectFolder"
}

if (Test-Path -Path $outputFolder) {
	Remove-Item -Path $outputFolder -Recurse -Force
}

New-Item -ItemType Directory -Path $outputFolder -Force | Out-Null

ShowStep "Restoring"
& dotnet restore $solution
ShowStep "Building"
& dotnet build $solution --configuration $config --no-restore
ShowStep "Testing"
& dotnet test $solution --configuration $config --no-restore --no-build --verbosity normal
ShowStep "Packing"
& dotnet pack $project --configuration $config --no-restore --no-build --output $outputFolder

$nupkg = Get-ChildItem -Path $outputFolder -Filter *.nupkg
# TODO: Add git packages source
$nugetSource = "https://api.nuget.org/v3/index.json"
$apiKey = [System.Environment]::GetEnvironmentVariable("NUGET_ORG_API_KEY")

ShowStep "Publishing"
& dotnet nuget push $nupkg.FullName --source $nugetSource --api-key $apiKey --skip-duplicate

Remove-Item -Path $outputFolder -Recurse -Force