#Set-ExecutionPolicy Unrestricted
$ErrorActionPreference = "Stop"

# Set current directory to script directory
Set-Location -Path $PSScriptRoot

# Output path where the build and all generated files is stored
$BuildPath = "$PSScriptRoot\Build"

# Target framework and runtime identifier
# win10-x64 is required - don't use win-x64 because of: https://github.com/PowerShell/PowerShell/issues/7909
$TargetFramework = "net8.0-windows10.0.19041.0"
#$TargetFramework = "net7.0-windows10.0.19041.0"
#$TargetFramework = "net6.0-windows10.0.17763.0"
$RuntimeIdentifier = "win10-x64"
$CertThumb = "874C34D76D0750519E23E065A42F0EE1268C31D6"
#$PublishProfile = ".\src\ActiveDirectorySearch\Properties\PublishProfiles\EXE-win10-x64.pubxml"

cls
# publish as exe files (https://learn.microsoft.com/en-us/dotnet/maui/windows/deployment/publish-unpackaged-cli?view=net-maui-8.0)
dotnet publish ".\src\ActiveDirectorySearch\ActiveDirectorySearch.csproj" -f $TargetFramework -c Release -p:WindowsAppSDKSelfContained=true -p:RuntimeIdentifierOverride=$RuntimeIdentifier -p:PackageCertificateThumbprint=$CertThumb 