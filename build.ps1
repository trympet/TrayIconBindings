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
 
 msbuild TrayIcon /p:Platform=x64 /p:Configuration=Release
 msbuild TrayIcon /p:Platform=win32 /p:Configuration=Release
 dotnet build TrayIconProjections -c:Release
 
 nuget pack nuget\TrayIcon.nuspec -nopackageanalysis -basepath . -outputdirectory .\bin -version $version