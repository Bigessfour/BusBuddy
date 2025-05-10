# TestGitHubConnectivity.ps1
# Script to test GitHub push/pull functionality

Write-Host "Testing GitHub connectivity and push/pull functionality..." -ForegroundColor Cyan

# 1. Check if we're in a git repository
Write-Host "Checking git repository status..." -ForegroundColor Yellow
if (-not (Test-Path ".git")) {
    Write-Host "ERROR: Not in a Git repository root folder!" -ForegroundColor Red
    exit 1
}

# 2. Verify remote configuration
Write-Host "Verifying remote configuration..." -ForegroundColor Yellow
$remotes = git remote -v
Write-Host $remotes

# 3. Test connection to GitHub
Write-Host "Testing connection to GitHub.com..." -ForegroundColor Yellow
$pingResult = ping github.com -n 1
Write-Host $pingResult

# 4. Check authentication status (without actually pushing anything)
Write-Host "Testing authentication with GitHub..." -ForegroundColor Yellow
$authTest = git ls-remote --heads origin 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "SUCCESS: Authentication with GitHub is working properly!" -ForegroundColor Green
} else {
    Write-Host "ERROR: Authentication with GitHub failed. Details:" -ForegroundColor Red
    Write-Host $authTest
}

# 5. Check branch tracking
Write-Host "Checking branch tracking configuration..." -ForegroundColor Yellow
$branch = git branch --show-current
$tracking = git config --get branch.$branch.remote
if ($tracking) {
    Write-Host "SUCCESS: Current branch '$branch' is tracking '$tracking'" -ForegroundColor Green
} else {
    Write-Host "WARNING: Current branch '$branch' is not tracking a remote branch" -ForegroundColor Yellow
}

# 6. Check for pending changes
Write-Host "Checking for pending changes..." -ForegroundColor Yellow
$status = git status --porcelain
if ($status) {
    Write-Host "You have uncommitted changes in your working directory:" -ForegroundColor Yellow
    Write-Host $status
} else {
    Write-Host "Working directory is clean." -ForegroundColor Green
}

# 7. Check user identity configuration
Write-Host "Checking Git user identity..." -ForegroundColor Yellow
$userName = git config user.name
$userEmail = git config user.email
Write-Host "Current user name: $userName" -ForegroundColor Cyan
Write-Host "Current user email: $userEmail" -ForegroundColor Cyan

if (-not $userName -or -not $userEmail) {
    Write-Host "WARNING: Git user identity is not fully configured!" -ForegroundColor Red
    Write-Host "Run these commands to set your identity:" -ForegroundColor Yellow
    Write-Host "git config --global user.name 'Steve McKitrick'" -ForegroundColor Yellow
    Write-Host "git config --global user.email 'steve.mckitrick@wileyschool.org'" -ForegroundColor Yellow
} else {
    Write-Host "Git user identity is properly configured." -ForegroundColor Green
}

Write-Host "`nGitHub connectivity check complete!" -ForegroundColor Cyan
