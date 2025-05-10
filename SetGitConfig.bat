@echo off
echo Setting Git user identity...

echo Setting local repository configuration...
git config --local user.name "Steve McKitrick"
git config --local user.email "steve.mckitrick@wileyschool.org"

echo Setting global configuration as fallback...
git config --global user.name "Steve McKitrick"
git config --global user.email "steve.mckitrick@wileyschool.org"

echo.
echo Verifying configuration...
echo Local repository configuration:
echo User name: 
git config --local user.name
echo User email: 
git config --local user.email

echo.
echo Global configuration:
echo User name: 
git config --global user.name
echo User email: 
git config --global user.email

echo.
echo Creating test file...
echo # GitHub Connectivity Test > github-test.md
echo. >> github-test.md
echo This file was created to test GitHub connectivity and Git user identity configuration. >> github-test.md
echo. >> github-test.md
echo Created by: Steve McKitrick >> github-test.md
echo Email: steve.mckitrick@wileyschool.org >> github-test.md
echo Timestamp: %DATE% %TIME% >> github-test.md

echo.
echo Test file created. You can now try to commit and push this file with:
echo git add github-test.md
echo git commit -m "Test GitHub connectivity and user identity"
echo git push

echo.
echo Git configuration complete!
pause
