 msbuild TrayIcon /p:Platform=x64 /p:Configuration=Release
 msbuild TrayIcon /p:Platform=win32 /p:Configuration=Release
 dotnet build TrayIconProjections -c:Release
 
 nuget pack nuget\TrayIcon.nuspec -nopackageanalysis -basepath . -outputdirectory .\bin -version 1.0.0