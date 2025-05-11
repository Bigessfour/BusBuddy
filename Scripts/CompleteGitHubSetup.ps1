# Complete GitHub setup and test
# This script will set up Git user identity and test connectivity

# Set Git user identity
Write-Host "Setting Git user identity..." -ForegroundColor Cyan
git config --global user.name "Steve McKitrick"
git config --global user.email "steve.mckitrick@wileyschool.org"

Write-Host "Verifying Git user identity settings..." -ForegroundColor Cyan
$userName = git config user.name
$userEmail = git config user.email
Write-Host "User name: $userName" -ForegroundColor Green
Write-Host "User email: $userEmail" -ForegroundColor Green

# Check if we're in a Git repository
if (-not (Test-Path ".git")) {
    Write-Host "This is not a Git repository. Initializing..." -ForegroundColor Yellow
    git init
    Write-Host "Git repository initialized." -ForegroundColor Green
} else {
    Write-Host "Git repository already exists." -ForegroundColor Green
}

# Check remote status
$remotes = git remote -v
if (-not $remotes) {
    Write-Host "No remote repository configured." -ForegroundColor Yellow
    Write-Host "To add a remote, use: git remote add origin <your-github-repo-url>" -ForegroundColor Yellow
} else {
    Write-Host "Remote repositories configured:" -ForegroundColor Green
    Write-Host $remotes
}

# Check for modified github-test.md file and stage it
$status = git status -s
if ($status -like "*github-test.md*") {
    Write-Host "Changes detected in github-test.md. Staging file..." -ForegroundColor Yellow
    git add github-test.md
    Write-Host "File staged." -ForegroundColor Green
}

Write-Host "`nTo commit the changes, use:" -ForegroundColor Cyan
Write-Host "git commit -m 'Test GitHub connectivity and user identity'" -ForegroundColor Yellow

Write-Host "`nTo push to GitHub (if remote is configured), use:" -ForegroundColor Cyan
Write-Host "git push -u origin main" -ForegroundColor Yellow

Write-Host "`nGitHub setup completed!" -ForegroundColor Green
