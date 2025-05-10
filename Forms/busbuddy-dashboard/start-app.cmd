@echo off
echo Setting up environment...
set PATH=%PATH%;C:\Program Files\nodejs
cd /d "%~dp0"
echo Installing dependencies...
call npm install react-scripts --save
call npm install
echo Starting React app...
call npm start
pause
