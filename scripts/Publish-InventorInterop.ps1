param (
	[Parameter(Mandatory=$false)]
	[string]$githubUsername = [System.Environment]::GetEnvironmentVariable("GH_USERNAME"),

	[Parameter(Mandatory=$false)]
	[string]$githubToken = [System.Environment]::GetEnvironmentVariable("GH_NUGET_AUTH_TOKEN")
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrEmpty($githubUsername)) {
	Write-Host -Object "GitHub account name not found in evnvironment varible!" -ForegroundColor Yellow
	$githubUsername = Read-Host -Prompt "Enter your GitHub account name"

	if ([string]::IsNullOrEmpty($githubUsername)) {
		Write-Error -Message "Script canceled: GitHub account name missing!"
	}
}

if ([string]::IsNullOrEmpty($githubToken)) {
	Write-Host -Object "GitHub token not found in evnvironment varible!" -ForegroundColor Yellow
	$githubToken = Read-Host -Prompt "Enter your GitHub Classic Token (ghp_...)"

	if ([string]::IsNullOrEmpty($githubToken)) {
		Write-Error -Message "Script canceled: Authorization token missing!"
	}
}

$inventorVersion = 2026
$tlbimpExe = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\tlbimp.exe"
$inventorDll = "C:\Program Files\Autodesk\Inventor $inventorVersion\Bin\Public Assemblies\Autodesk.Inventor.Interop.dll"
$inventorTlb = "C:\Program Files\Autodesk\Inventor $inventorVersion\Bin\RxInventor.tlb"

if (-not (Test-Path -Path $tlbimpExe)) { 
	Write-Error -Message "Tlbimp.exe not found: $tlbimpExe"
}
if (-not (Test-Path -Path $inventorDll)) { 
	Write-Error -Message "Inventor DLL not found: $inventorDll"
}
if (-not (Test-Path -Path $inventorTlb)) { 
	Write-Error -Message "Inventor TLB not found: $inventorTlb"
}

$versionInfo = (Get-Item -Path $inventorDll).VersionInfo
$assemblyVersion = $versionInfo.FileVersion
Write-Host -Object "Detected Inventor FileVersion: $assemblyVersion" -ForegroundColor Cyan

$packageId = "FlederM4us.Inventor.Interop"
$outputDllName = "$packageId.dll"
$nugetOutputFolder = Join-Path -Path $PSScriptRoot -ChildPath "output"
$stagingDir = Join-Path -Path $nugetOutputFolder -ChildPath "$packageId.Staging"
$libDir = Join-Path -Path $stagingDir -ChildPath "lib\net8.0"
$outputDllPath = Join-Path -Path $libDir -ChildPath $outputDllName

if (Test-Path -Path $nugetOutputFolder) {
	Remove-Item -Path $nugetOutputFolder -Recurse -Force
}
New-Item -ItemType Directory -Path $nugetOutputFolder -Force | Out-Null
New-Item -ItemType Directory -Path $libDir -Force | Out-Null

Write-Host -Object "Generating interop assembly..." -ForegroundColor Green
# & $tlbimpExe $inventorTlb `
#   /out:$outputDllPath `
#   /namespace:Inventor `
#   /asmversion:$assemblyVersion `

# if (-not (Test-Path -Path $outputDllPath)) { 
# 	Write-Error -Message "Failed to generate interop assembly."
# }

Copy-Item -Path $inventorDll -Destination $outputDllPath

Write-Host -Object "Generating .nuspec file..." -ForegroundColor Green
$nuspecName = "$packageId.nuspec"
$nuspecPath = Join-Path -Path $stagingDir -ChildPath "$nuspecName"
$nuspecContent = @"
<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
	<metadata>
		<id>$packageId</id>
		<version>$assemblyVersion</version>
		<authors>FlederM4us</authors>
		<description>Private Interop assembly generated via TlbImp for background CI/CD builds.</description>
		<dependencies>
			<group targetFramework="net8.0" />
		</dependencies>
	</metadata>
	<files>
		<file src="lib\**" target="lib" />
	</files>
</package>
"@
Set-Content -Path $nuspecPath -Value $nuspecContent -Encoding utf8

Write-Host -Object "Packing NuGet package..." -ForegroundColor Green
& dotnet pack "$stagingDir/$nuspecName" /p:NuspecFile="$nuspecName" /p:NuspecBasePath=$stagingDir --output $nugetOutputFolder

Write-Host -Object "Pushing package to GitHub Packages..." -ForegroundColor Green
$githubSource = "https://nuget.pkg.github.com/$githubUsername/index.json"
$nupkgFile = Get-ChildItem -Path $nugetOutputFolder -Filter "*.nupkg" | Select-Object -First 1
& dotnet nuget push $nupkgFile.FullName --source $githubSource --api-key $githubToken --skip-duplicate

Remove-Item -Path $nugetOutputFolder -Recurse -Force