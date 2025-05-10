@echo off
echo Setting up environment for BusBuddy Dashboard...
set PATH=%PATH%;C:\Program Files\nodejs
cd /d "%~dp0"

echo Installing backend dependencies...
if not exist "backend\api\node_modules" (
  cd backend\api
  call npm init -y
  call npm install express mssql cors dotenv
  cd ..\..
)

echo Installing frontend dependencies...
call npm install react-scripts --save
call npm install

echo Starting API server...
start cmd /k "cd backend\api && node index.js"

echo Starting React app...
call npm start
