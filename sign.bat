@echo off
setlocal

:: smctl healthcheck
echo [INFO] Synchronizing Certs
smctl windows certsync --keypair-alias=%SM_KEYPAIR_ALIAS%

:: Input check
if "%~1"=="" (
    echo [ERROR] No input file provided.
    exit /b 1
)

:: Show full input
echo [INFO] Original input: "%~1"

set "orig=%~1"
set "dir=%~dp1"
set "name=%~n1"
set "ext=%~x1"

echo [INFO] Directory: "%dir%"
echo [INFO] Base name: "%name%"
echo [INFO] Extension: "%ext%"

:: Decide rename
if /I "%ext%"==".exe" (
    echo [INFO] File already has .exe extension.
    set "temp=%orig%"
) else (
    echo [INFO] Renaming "%orig%" to "%name%.exe"
    ren "%orig%" "%name%.exe"
    set "temp=%dir%%name%.exe"
)

:: Confirm file exists before signing
if not exist "%temp%" (
    echo [ERROR] File to sign not found: "%temp%"
    exit /b 1
)

:: Run signing tool
echo [INFO] Signing: "%temp%"
smctl sign --fingerprint %SM_CODE_SIGNING_CERT_SHA1_HASH% --input "%temp%" 
echo [INFO] smctl returned: %ERRORLEVEL%

if errorlevel 1 (
    echo [ERROR] smctl sign failed!
    if not "%ext%"==".exe" ren "%temp%" "%name%%ext%"
    exit /b 1
)

:: Verify signature
echo [INFO] Verifying signature...
signtool verify /pa "%temp%"
echo [INFO] signtool verify returned: %ERRORLEVEL%

if errorlevel 1 (
    echo [ERROR] Signature verification FAILED
    if not "%ext%"==".exe" ren "%temp%" "%name%%ext%"
    exit /b 1
)

:: Rename back if needed
if /I not "%ext%"==".exe" (
    echo [INFO] Renaming "%temp%" back to "%name%%ext%"
    ren "%temp%" "%name%%ext%"
)

echo [SUCCESS] Signing complete for: "%orig%"
exit /b 0
