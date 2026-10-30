@echo off
setlocal
dotnet publish "%~dp0MainScreenCurtain.csproj" -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o "%~dp0publish"
if errorlevel 1 exit /b 1
echo.
echo Published: %~dp0publish\MainScreenCurtain.exe
endlocal
