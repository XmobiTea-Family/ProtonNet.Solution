@echo off
setlocal

REM Get the first command-line argument
set INPUT_COMMAND=%1

REM Check if the command-line argument is provided
if "%INPUT_COMMAND%"=="" (
    echo Error: No command provided. Please specify a command, such as start, stop, install, log, uninstall, debug, help, or version.
    exit /b 1
)

REM Define the configuration file path
set SERVER_SETTINGS_FILE_PATH=ProtonServerSettings.json

REM Check if the configuration file exists
if not exist "%SERVER_SETTINGS_FILE_PATH%" (
    echo Error: Configuration file "%SERVER_SETTINGS_FILE_PATH%" not found. Please verify the file's presence.
    exit /b 1
)

REM Extract values from the configuration file
for /f "tokens=2 delims=:" %%A in ('findstr /C:"\"TargetFramework\":" "%SERVER_SETTINGS_FILE_PATH%"') do set TARGET_FRAMEWORK=%%A
for /f "tokens=2 delims=:" %%A in ('findstr /C:"\"TargetRuntime\":" "%SERVER_SETTINGS_FILE_PATH%"') do set TARGET_RUNTIME=%%A

REM Remove quotation marks, commas, and unnecessary whitespace from values
set TARGET_FRAMEWORK=%TARGET_FRAMEWORK:"=%
set TARGET_FRAMEWORK=%TARGET_FRAMEWORK:,=%
set TARGET_RUNTIME=%TARGET_RUNTIME:"=%
set TARGET_RUNTIME=%TARGET_RUNTIME:,=%

REM Trim leading and trailing whitespace (if any)
for /f "tokens=* delims= " %%A in ("%TARGET_FRAMEWORK%") do set TARGET_FRAMEWORK=%%A
for /f "tokens=* delims= " %%A in ("%TARGET_RUNTIME%") do set TARGET_RUNTIME=%%A

REM Check if the extracted values are present
if "%TARGET_FRAMEWORK%"=="" (
    echo Error: 'TargetFramework' is missing in the configuration file "%SERVER_SETTINGS_FILE_PATH%".
    exit /b 1
)

if "%TARGET_RUNTIME%"=="" (
    echo Error: 'TargetRuntime' is missing in the configuration file "%SERVER_SETTINGS_FILE_PATH%".
    exit /b 1
)

REM Determine the path to the control file
set CONTROL_SUPPORT_FILE_PATH=./control/%TARGET_FRAMEWORK%/%TARGET_RUNTIME%/XmobiTea.ProtonService.Control

REM Add the .exe extension if needed
if "%TARGET_RUNTIME:~0,3%"=="win" (
    set CONTROL_SUPPORT_FILE_PATH=%CONTROL_SUPPORT_FILE_PATH%.exe
)

REM Check if the control file exists
if not exist "%CONTROL_SUPPORT_FILE_PATH%" (
    echo Error: Control file "%CONTROL_SUPPORT_FILE_PATH%" not found. Please check the path and try again.
    exit /b 1
)

REM Get the project name from the second argument
set INPUT_PROJECT_NAME=%2

REM Prepare the parameters for the command
set "FINAL_INPUT_PARAMETERS=%INPUT_COMMAND%"

REM Add the project name to the parameters if provided
if not "%INPUT_PROJECT_NAME%"=="" (
    set "FINAL_INPUT_PARAMETERS=%INPUT_COMMAND% %INPUT_PROJECT_NAME%"
)

REM Execute the command
PowerShell -Command "Start-Process -FilePath '%CONTROL_SUPPORT_FILE_PATH%' -ArgumentList '%FINAL_INPUT_PARAMETERS%' -Wait -Verb RunAs"

REM Show last 10 log
PowerShell -Command "Get-Content -Path './logs/proton-control.log' -Tail 10"

endlocal
