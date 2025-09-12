# Test Docker build locally
Write-Host "Testing Docker builds for DriveMate services..." -ForegroundColor Green

# Check if Docker is running
try {
    docker --version
    Write-Host "✓ Docker is available" -ForegroundColor Green
} catch {
    Write-Host "✗ Docker is not available. Please install Docker Desktop." -ForegroundColor Red
    exit 1
}

# Test UserService Docker build
Write-Host "`nBuilding UserService Docker image..." -ForegroundColor Yellow
try {
    docker build -f src/UserService/Dockerfile -t drivemate-userservice:test .
    Write-Host "✓ UserService Docker build successful" -ForegroundColor Green
} catch {
    Write-Host "✗ UserService Docker build failed" -ForegroundColor Red
    Write-Host "Check the Dockerfile and project structure" -ForegroundColor Yellow
}

# Test API Gateway Docker build
Write-Host "`nBuilding API Gateway Docker image..." -ForegroundColor Yellow
try {
    docker build -f src/ApiGetwate/Dockerfile -t drivemate-apigateway:test .
    Write-Host "✓ API Gateway Docker build successful" -ForegroundColor Green
} catch {
    Write-Host "✗ API Gateway Docker build failed" -ForegroundColor Red
    Write-Host "Check the Dockerfile and project structure" -ForegroundColor Yellow
}

# Show built images
Write-Host "`nDocker images built:" -ForegroundColor Cyan
docker images | Select-String "drivemate"

Write-Host "`nNext steps:" -ForegroundColor Cyan
Write-Host "1. Create Docker Hub account and repositories" -ForegroundColor White
Write-Host "2. Add DOCKER_USERNAME and DOCKER_PASSWORD to GitHub Secrets" -ForegroundColor White
Write-Host "3. Push code to trigger GitHub Actions build" -ForegroundColor White
Write-Host "4. Deploy from Docker Hub images on Railway" -ForegroundColor White
