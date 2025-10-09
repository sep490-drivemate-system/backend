# Setup Docker Hub Repositories for DriveMate Microservices
param(
    [Parameter(Mandatory=$true)]
    [string]$DockerHubUsername,
    
    [Parameter(Mandatory=$false)]
    [string]$DockerHubToken
)

Write-Host "=== DriveMate Docker Hub Setup ===" -ForegroundColor Green

# Repository names
$repositories = @(
    "drivemate-userservice",
    "drivemate-bookingservice", 
    "drivemate-resourceservice",
    "drivemate-apigateway"
)

# Function to create Docker Hub repository
function Create-DockerHubRepo {
    param(
        [string]$RepoName,
        [string]$Username,
        [string]$Token
    )
    
    Write-Host "Creating repository: $Username/$RepoName" -ForegroundColor Yellow
    
    $body = @{
        name = $RepoName
        description = "DriveMate $RepoName microservice"
        is_private = $false
        namespace = $Username
    } | ConvertTo-Json
    
    try {
        if ($Token) {
            $headers = @{
                "Authorization" = "Bearer $Token"
                "Content-Type" = "application/json"
            }
            
            $response = Invoke-RestMethod -Uri "https://hub.docker.com/v2/repositories/" -Method POST -Body $body -Headers $headers
            Write-Host "✅ Repository $RepoName created successfully!" -ForegroundColor Green
        } else {
            Write-Host "⚠️  No token provided. Please create repository manually: $Username/$RepoName" -ForegroundColor Yellow
        }
    }
    catch {
        if ($_.Exception.Response.StatusCode -eq 409) {
            Write-Host "ℹ️  Repository $RepoName already exists" -ForegroundColor Cyan
        } else {
            Write-Host "❌ Failed to create repository $RepoName`: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Login to Docker Hub
if ($DockerHubToken) {
    Write-Host "Logging in to Docker Hub..." -ForegroundColor Cyan
    echo $DockerHubToken | docker login --username $DockerHubUsername --password-stdin
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Successfully logged in to Docker Hub" -ForegroundColor Green
    } else {
        Write-Host "❌ Failed to login to Docker Hub" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "⚠️  No Docker Hub token provided. Manual repository creation required." -ForegroundColor Yellow
}

# Create repositories
Write-Host "`nCreating Docker Hub repositories..." -ForegroundColor Cyan
foreach ($repo in $repositories) {
    Create-DockerHubRepo -RepoName $repo -Username $DockerHubUsername -Token $DockerHubToken
    Start-Sleep -Seconds 1
}

# Build and push initial images (optional)
$buildImages = Read-Host "`nDo you want to build and push initial images? (y/N)"
if ($buildImages -eq 'y' -or $buildImages -eq 'Y') {
    Write-Host "`nBuilding and pushing Docker images..." -ForegroundColor Cyan
    
    # Build UserService
    Write-Host "Building UserService..." -ForegroundColor Yellow
    docker build -t "$DockerHubUsername/drivemate-userservice:latest" -f src/UserService/Dockerfile .
    docker push "$DockerHubUsername/drivemate-userservice:latest"
    
    # Build BookingService
    Write-Host "Building BookingService..." -ForegroundColor Yellow
    docker build -t "$DockerHubUsername/drivemate-bookingservice:latest" -f src/BookingService/Dockerfile .
    docker push "$DockerHubUsername/drivemate-bookingservice:latest"
    
    # Build ResourceService
    Write-Host "Building ResourceService..." -ForegroundColor Yellow
    docker build -t "$DockerHubUsername/drivemate-resourceservice:latest" -f src/ResourceService/Dockerfile .
    docker push "$DockerHubUsername/drivemate-resourceservice:latest"
    
    # Build API Gateway
    Write-Host "Building API Gateway..." -ForegroundColor Yellow
    docker build -t "$DockerHubUsername/drivemate-apigateway:latest" -f src/ApiGetwate/Dockerfile .
    docker push "$DockerHubUsername/drivemate-apigateway:latest"
    
    Write-Host "✅ All images built and pushed successfully!" -ForegroundColor Green
}

# Summary
Write-Host "`n=== Setup Summary ===" -ForegroundColor Green
Write-Host "Docker Hub Username: $DockerHubUsername" -ForegroundColor White
Write-Host "Repositories created:" -ForegroundColor White
foreach ($repo in $repositories) {
    Write-Host "  - $DockerHubUsername/$repo" -ForegroundColor Cyan
}

Write-Host "`n📋 Next Steps:" -ForegroundColor Yellow
Write-Host "1. Add GitHub Secrets:" -ForegroundColor White
Write-Host "   - DOCKERHUB_USERNAME: $DockerHubUsername" -ForegroundColor Gray
Write-Host "   - DOCKERHUB_TOKEN: [your-access-token]" -ForegroundColor Gray
Write-Host "2. Setup Railway projects for each service" -ForegroundColor White
Write-Host "3. Configure Railway webhook URLs in GitHub Secrets" -ForegroundColor White
Write-Host "4. Push code to trigger CI/CD pipeline" -ForegroundColor White

Write-Host "`n🔗 Useful Links:" -ForegroundColor Yellow
Write-Host "- Docker Hub: https://hub.docker.com/u/$DockerHubUsername" -ForegroundColor Cyan
Write-Host "- Railway: https://railway.app/dashboard" -ForegroundColor Cyan
Write-Host "- GitHub Actions: https://github.com/your-repo/actions" -ForegroundColor Cyan
