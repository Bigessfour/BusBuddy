@echo off
echo Starting BusBuddy file organization process...
cd c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy

echo Creating directories if they don't exist...
if not exist "Scripts" mkdir Scripts
if not exist "Docs" mkdir Docs

echo Moving PowerShell scripts to Scripts directory...
for /r %%F in (*.ps1) do (
    if not "%%~dpF"=="c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\Scripts\" (
        echo Moving %%F to Scripts directory
        move "%%F" "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\Scripts\"
    )
)

echo Moving Markdown files to Docs directory...
for /r %%F in (*.md) do (
    if not "%%~dpF"=="c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\Docs\" (
        echo Moving %%F to Docs directory
        move "%%F" "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\Docs\"
    )
)

echo Organization complete!
echo.
echo Files have been organized according to the file organization strategy.
pause
