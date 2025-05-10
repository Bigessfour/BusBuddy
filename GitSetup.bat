@echo off
echo Setting up Git user identity and testing GitHub connectivity...

cd "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy"

echo Setting user name and email...
git config user.name "Steve McKitrick"
git config user.email "steve.mckitrick@wileyschool.org"

echo Checking Git status...
git status

echo Adding test file...
git add github-test.md

echo Current Git status:
git status

echo.
echo To complete the process, run:
echo git commit -m "Test GitHub connectivity and user identity"
echo git push

echo.
echo Setup completed!
pause
