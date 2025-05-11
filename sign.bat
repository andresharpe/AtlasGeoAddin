@echo off
setlocal

:: Check if input was provided
if "%~1"=="" (
    echo Usage: sign.bat filename
    exit /b 1
)

:: Get the full path and filename without extension
set "orig=%~1"
set "dir=%~dp1"
set "name=%~n1"
set "ext=%~x1"

:: Handle case where file already has .exe extension
if /I "%ext%"==".exe" (
    echo Input file already has .exe extension.
    set "temp=%orig%"
) else (
    ren "%orig%" "%name%.exe"
    set "temp=%dir%%name%.exe"
)

:: Sign the file
smctl sign --fingerprint ffc2d19f32085e28dc4c7a218a865cedf4fa346b --input "%temp%"
if errorlevel 1 (
    echo Signing failed!
    if not "%ext%"==".exe" ren "%temp%" "%name%%ext%"
    exit /b 1
)

:: Rename back if original wasn't .exe
if /I not "%ext%"==".exe" (
    ren "%temp%" "%name%%ext%"
)

echo Done.
endlocal
