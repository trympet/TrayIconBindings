 $version = "1.0.0"
 if ($(where.exe nbgv)) {
    Write-Host "building:"
    nbgv get-version
    nbgv cloud -p . -s GitHubActions
    $version = $(nbgv get-version -v NuGetPackageVersion)
}
else {
    Write-Host "nbgv not found. reverting to $version."
}
 
msbuild SimpleTrayIcon /p:Platform=x64 /p:Configuration=Release
msbuild SimpleTrayIcon /p:Platform=Win32 /p:Configuration=Release

dotnet build SimpleTrayIconProjections -c:Release /p:Platform=AnyCPU

nuget pack nuget\SimpleTrayIcon.nuspec -nopackageanalysis -basepath . -outputdirectory .\bin -version $version -properties  commit=$(git rev-parse HEAD)

msbuild SimpleTrayIcon /p:Platform=x64 /p:Configuration=Release /p:TrayIconOneCore=true
msbuild SimpleTrayIcon /p:Platform=Win32 /p:Configuration=Release /p:TrayIconOneCore=true

nuget pack nuget\SimpleTrayIcon.onecore.nuspec -nopackageanalysis -basepath . -outputdirectory .\bin -version $version -properties  commit=$(git rev-parse HEAD)