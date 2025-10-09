# Test CI/CD Pipeline for DriveMate Microservices
param(
    [Parameter(Mandatory=$false)]
    [string]$GitHubRepo = "",
    
    [Parameter(Mandatory=$false)]
    [string]$DockerHubUsername = ""
)

Write-Host "=== DriveMate CI/CD Pipeline Test ===" -ForegroundColor Green

# Function to check GitHub Actions status
function Check-GitHubActions {
    param([string]$RepoUrl)
    
    if ($RepoUrl) {
        Write-Host "🔍 Checking GitHub Actions..." -ForegroundColor Yellow
        Write-Host "Repository: $RepoUrl" -ForegroundColor Cyan
        Write-Host "Go to: $RepoUrl/actions to view workflow status" -ForegroundColor White
    } else {
        Write-Host "⚠️  GitHub repository URL not provided" -ForegroundColor Yellow
    }
}

# Function to check Docker Hub images
function Check-DockerHubImages {
    param([string]$Username)
    
    if ($Username) {
        Write-Host "`n🐳 Checking Docker Hub images..." -ForegroundColor Yellow
        
        $repositories = @(
            "drivemate-userservice",
            "drivemate-bookingservice",
            "drivemate-resourceservice", 
            "drivemate-apigateway"
        )
        
        foreach ($repo in $repositories) {
            $imageUrl = "https://hub.docker.com/r/$Username/$repo"
            Write-Host "  - $Username/$repo" -ForegroundColor Cyan
            
            try {
                $response = Invoke-WebRequest -Uri $imageUrl -Method HEAD -TimeoutSec 10
                if ($response.StatusCode -eq 200) {
                    Write-Host "    ✅ Repository exists" -ForegroundColor Green
                } else {
                    Write-Host "    ❌ Repository not found" -ForegroundColor Red
                }
            }
            catch {
                Write-Host "    ❌ Repository not accessible" -ForegroundColor Red
            }
        }
    } else {
        Write-Host "⚠️  Docker Hub username not provided" -ForegroundColor Yellow
    }
}

# Function to test Railway deployments
function Test-RailwayDeployments {
    Write-Host "`n🚂 Testing Railway deployments..." -ForegroundColor Yellow
    
    $services = @(
        @{ Name = "UserService"; Pattern = "*userservice*" },
        @{ Name = "BookingService"; Pattern = "*bookingservice*" },
        @{ Name = "ResourceService"; Pattern = "*resourceservice*" },
        @{ Name = "API Gateway"; Pattern = "*apigateway*" }
    )
    
    Write-Host "Please provide Railway URLs for testing:" -ForegroundColor Cyan
    
    foreach ($service in $services) {
        $url = Read-Host "Enter $($service.Name) Railway URL (or press Enter to skip)"
        if ($url) {
            try {
                Write-Host "Testing $($service.Name) at $url..." -ForegroundColor Yellow
                $healthUrl = "$url/health"
                $response = Invoke-WebRequest -Uri $healthUrl -Method GET -TimeoutSec 15
                
                if ($response.StatusCode -eq 200) {
                    Write-Host "  ✅ $($service.Name) is healthy!" -ForegroundColor Green
                } else {
                    Write-Host "  ⚠️  $($service.Name) responded with status: $($response.StatusCode)" -ForegroundColor Yellow
                }
            }
            catch {
                Write-Host "  ❌ $($service.Name) health check failed: $($_.Exception.Message)" -ForegroundColor Red
            }
        }
    }
}

# Function to check GitHub Secrets
function Check-GitHubSecrets {
    Write-Host "`n🔐 GitHub Secrets Checklist:" -ForegroundColor Yellow
    
    $secrets = @(
        "DOCKERHUB_USERNAME",
        "DOCKERHUB_TOKEN", 
        "RAILWAY_USERSERVICE_WEBHOOK",
        "RAILWAY_BOOKINGSERVICE_WEBHOOK",
        "RAILWAY_RESOURCESERVICE_WEBHOOK",
        "RAILWAY_APIGATEWAY_WEBHOOK"
    )
    
    Write-Host "Required GitHub Secrets:" -ForegroundColor Cyan
    foreach ($secret in $secrets) {
        Write-Host "  ☐ $secret" -ForegroundColor White
    }
    
    Write-Host "`nTo add secrets:" -ForegroundColor Gray
    Write-Host "1. Go to GitHub repository → Settings → Secrets and variables → Actions" -ForegroundColor Gray
    Write-Host "2. Click 'New repository secret'" -ForegroundColor Gray
    Write-Host "3. Add each secret with its corresponding value" -ForegroundColor Gray
}

# Function to simulate CI/CD trigger
function Test-CICDTrigger {
    Write-Host "`n🚀 Testing CI/CD Trigger..." -ForegroundColor Yellow
    
    # Check if we're in a git repository
    try {
        $gitStatus = git status 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Git repository detected" -ForegroundColor Green
            
            $branch = git branch --show-current
            Write-Host "Current branch: $branch" -ForegroundColor Cyan
            
            $triggerTest = Read-Host "Do you want to create a test commit to trigger CI/CD? (y/N)"
            if ($triggerTest -eq 'y' -or $triggerTest -eq 'Y') {
                # Create a test file
                $testFile = "cicd-test-$(Get-Date -Format 'yyyyMMdd-HHmmss').txt"
                "CI/CD Pipeline Test - $(Get-Date)" | Out-File -FilePath $testFile
                
                git add $testFile
                git commit -m "test: trigger CI/CD pipeline - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
                
                $pushConfirm = Read-Host "Push to trigger GitHub Actions? (y/N)"
                if ($pushConfirm -eq 'y' -or $pushConfirm -eq 'Y') {
                    git push origin $branch
                    Write-Host "✅ Test commit pushed! Check GitHub Actions for workflow status." -ForegroundColor Green
                    
                    if ($GitHubRepo) {
                        Write-Host "GitHub Actions: $GitHubRepo/actions" -ForegroundColor Cyan
                    }
                } else {
                    Write-Host "ℹ️  Commit created but not pushed. Run 'git push' manually to trigger CI/CD." -ForegroundColor Cyan
                }
            }
        } else {
            Write-Host "❌ Not in a Git repository" -ForegroundColor Red
        }
    }
    catch {
        Write-Host "❌ Git not available or not in a repository" -ForegroundColor Red
    }
}

# Main execution
Write-Host "Starting CI/CD pipeline test..." -ForegroundColor Cyan

# Check GitHub Actions
Check-GitHubActions -RepoUrl $GitHubRepo

# Check Docker Hub
Check-DockerHubImages -Username $DockerHubUsername

# Check GitHub Secrets
Check-GitHubSecrets

# Test Railway deployments
Test-RailwayDeployments

# Test CI/CD trigger
Test-CICDTrigger

# Summary
Write-Host "`n=== Test Summary ===" -ForegroundColor Green
Write-Host "✅ CI/CD pipeline test completed" -ForegroundColor Green
Write-Host "`n📋 Next Steps:" -ForegroundColor Yellow
Write-Host "1. Verify all GitHub Secrets are configured" -ForegroundColor White
Write-Host "2. Check GitHub Actions workflow runs" -ForegroundColor White
Write-Host "3. Monitor Docker Hub for new image pushes" -ForegroundColor White
Write-Host "4. Verify Railway deployments are successful" -ForegroundColor White
Write-Host "5. Test all service endpoints" -ForegroundColor White

Write-Host "`n🔗 Monitoring Links:" -ForegroundColor Yellow
if ($GitHubRepo) {
    Write-Host "- GitHub Actions: $GitHubRepo/actions" -ForegroundColor Cyan
}
if ($DockerHubUsername) {
    Write-Host "- Docker Hub: https://hub.docker.com/u/$DockerHubUsername" -ForegroundColor Cyan
}
Write-Host "- Railway Dashboard: https://railway.app/dashboard" -ForegroundColor Cyan
