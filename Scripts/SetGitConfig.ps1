# SetGitConfig.ps1
# Script to set Git user identity and test commit functionality

# Set both local and global Git config to ensure it works
Write-Host "Setting Git user identity..." -ForegroundColor Cyan

# Set local repository configuration
Write-Host "Setting local repository configuration..." -ForegroundColor Yellow
git config --local user.name "Steve McKitrick"
git config --local user.email "steve.mckitrick@wileyschool.org"

# Set global configuration as fallback
Write-Host "Setting global configuration as fallback..." -ForegroundColor Yellow
git config --global user.name "Steve McKitrick"
git config --global user.email "steve.mckitrick@wileyschool.org"

# Verify configuration
Write-Host "`nVerifying configuration..." -ForegroundColor Cyan
Write-Host "Local repository configuration:" -ForegroundColor Yellow
Write-Host "User name: $(git config --local user.name)"
Write-Host "User email: $(git config --local user.email)"

Write-Host "`nGlobal configuration:" -ForegroundColor Yellow
Write-Host "User name: $(git config --global user.name)"
Write-Host "User email: $(git config --global user.email)"

# Create test file for commit
$testFilePath = "github-test.md"
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
$content = @"
# GitHub Connectivity Test

This file was created to test GitHub connectivity and Git user identity configuration.

Created by: $(git config user.name)
Email: $(git config user.email)
Timestamp: $timestamp
"@

Write-Host "`nCreating test file: $testFilePath" -ForegroundColor Yellow
$content | Out-File -FilePath $testFilePath -Encoding utf8

Write-Host "`nTest file created. You can now try to commit and push this file with:" -ForegroundColor Green
Write-Host "git add $testFilePath" -ForegroundColor Cyan
Write-Host "git commit -m 'Test GitHub connectivity and user identity'" -ForegroundColor Cyan
Write-Host "git push" -ForegroundColor Cyan

Write-Host "`nGit configuration complete!" -ForegroundColor Green
