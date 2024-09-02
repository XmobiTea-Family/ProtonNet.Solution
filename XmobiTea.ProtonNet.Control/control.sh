#!/bin/bash

# Get the first command-line argument
INPUT_COMMAND="$1"

# Check if the command-line argument is provided
if [ -z "$INPUT_COMMAND" ]; then
  echo "Error: No command provided. Please specify a command, e.g., start, stop, install, log, uninstall, debug, help, version."
  exit 1
fi

# Define the configuration file path
SERVER_SETTINGS_FILE_PATH="ProtonServerSettings.json"

# Check if the configuration file exists
if [ ! -e "$SERVER_SETTINGS_FILE_PATH" ]; then
    echo "Error: Configuration file '$SERVER_SETTINGS_FILE_PATH' not found. Please verify the file's existence."
    exit 1
fi

# Extract values from the configuration file
TARGET_FRAMEWORK=$(awk -F'"' '/"TargetFramework":/ {print $4}' "$SERVER_SETTINGS_FILE_PATH")
TARGET_RUNTIME=$(awk -F'"' '/"TargetRuntime":/ {print $4}' "$SERVER_SETTINGS_FILE_PATH")

# Check if the extracted values are present
if [ -z "$TARGET_FRAMEWORK" ] || [ -z "$TARGET_RUNTIME" ]; then
  echo "Error: Missing 'TargetFramework' or 'TargetRuntime' in the configuration file '$SERVER_SETTINGS_FILE_PATH'."
  exit 1
fi

# Define the path to the control file
CONTROL_SUPPORT_FILE_PATH="./control/${TARGET_FRAMEWORK}/${TARGET_RUNTIME}/XmobiTea.ProtonNet.Control"

# Add the .exe extension if needed
if [[ "$TARGET_RUNTIME" == win* ]]; then
  CONTROL_SUPPORT_FILE_PATH="$CONTROL_SUPPORT_FILE_PATH.exe"
fi

# Check if the control file exists
if [ ! -e "$CONTROL_SUPPORT_FILE_PATH" ]; then
    echo "Error: Control file '$CONTROL_SUPPORT_FILE_PATH' not found. Please check the path and try again."
    exit 1
fi

# Get the project name from the second argument
INPUT_PROJECT_NAME="$2"

# Prepare the parameters for the command
FINAL_INPUT_PARAMETERS="$INPUT_COMMAND"

# Add the project name to the parameters if provided
if [ -n "$INPUT_PROJECT_NAME" ]; then
  FINAL_INPUT_PARAMETERS="$FINAL_INPUT_PARAMETERS $INPUT_PROJECT_NAME"
fi

# Execute the command
"$CONTROL_SUPPORT_FILE_PATH" $FINAL_INPUT_PARAMETERS
