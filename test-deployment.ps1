# Test script for local Docker deployment
Write-Host "Testing DriveMate System Deployment..." -ForegroundColor Green

# Check if Docker is running
try {
    docker --version
    Write-Host "✓ Docker is available" -ForegroundColor Green
} catch {
    Write-Host "✗ Docker is not available. Please install Docker Desktop." -ForegroundColor Red
    exit 1
}

# Check if .env file exists (for local testing)
if (-not (Test-Path ".env")) {
    Write-Host "⚠ .env file not found. Creating sample .env file..." -ForegroundColor Yellow
    
    $envContent = @"
# Database Configuration
POSTGRES_PASSWORD=your_secure_password_here
USERSERVICECONNECTION=Host=localhost;Database=drivemate_db;Username=postgres;Password=your_secure_password_here

# JWT Configuration
JWT_KEY=your_super_secret_jwt_key_that_should_be_at_least_32_characters_long
JWT_ISSUER=https://localhost:5000
JWT_AUDIENCE=https://localhost:5000

# API Hosts (for production)
USERSERVICE_HOST=localhost:5100
PRODUCTSERVICE_HOST=localhost:5200
APIGATEWAY_HOST=localhost:5000
"@
    
    Set-Content -Path ".env" -Value $envContent
    Write-Host "✓ Sample .env file created. Please update with your actual values." -ForegroundColor Green
}

# Test Docker build for UserService
Write-Host "Testing UserService Docker build..." -ForegroundColor Yellow
try {
    docker build -f src/UserService/Dockerfile -t drivemate-userservice .
    Write-Host "✓ UserService Docker build successful" -ForegroundColor Green
} catch {
    Write-Host "✗ UserService Docker build failed" -ForegroundColor Red
}

# Test Docker build for API Gateway
Write-Host "Testing API Gateway Docker build..." -ForegroundColor Yellow
try {
    docker build -f src/ApiGetwate/Dockerfile -t drivemate-apigateway .
    Write-Host "✓ API Gateway Docker build successful" -ForegroundColor Green
} catch {
    Write-Host "✗ API Gateway Docker build failed" -ForegroundColor Red
}

Write-Host "Deployment test completed!" -ForegroundColor Green
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. Update .env file with your actual values" -ForegroundColor White
Write-Host "2. Push code to GitHub repository" -ForegroundColor White
Write-Host "3. Set up Railway project and services" -ForegroundColor White
Write-Host "4. Configure environment variables in Railway" -ForegroundColor White
Write-Host "5. Deploy using GitHub Actions or Railway CLI" -ForegroundColor White
