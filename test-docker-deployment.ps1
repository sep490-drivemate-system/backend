# Test Docker Deployment Script for DriveMate Microservices
Write-Host "=== DriveMate Docker Deployment Test ===" -ForegroundColor Green

# Function to test service health
function Test-ServiceHealth {
    param(
        [string]$ServiceName,
        [string]$Url,
        [int]$MaxRetries = 10
    )
    
    Write-Host "Testing $ServiceName at $Url..." -ForegroundColor Yellow
    
    for ($i = 1; $i -le $MaxRetries; $i++) {
        try {
            $response = Invoke-WebRequest -Uri $Url -Method GET -TimeoutSec 10
            if ($response.StatusCode -eq 200) {
                Write-Host "✅ $ServiceName is healthy!" -ForegroundColor Green
                return $true
            }
        }
        catch {
            Write-Host "⏳ Attempt $i/$MaxRetries failed. Waiting 10 seconds..." -ForegroundColor Yellow
            Start-Sleep -Seconds 10
        }
    }
    
    Write-Host "❌ $ServiceName failed health check after $MaxRetries attempts" -ForegroundColor Red
    return $false
}

# Build and start services
Write-Host "Building and starting all services..." -ForegroundColor Cyan
docker-compose up -d --build

# Wait for services to start
Write-Host "Waiting 30 seconds for services to initialize..." -ForegroundColor Cyan
Start-Sleep -Seconds 30

# Test all services
$services = @(
    @{ Name = "API Gateway"; Url = "http://localhost:8080/health" },
    @{ Name = "User Service"; Url = "http://localhost:8081/health" },
    @{ Name = "Booking Service"; Url = "http://localhost:8082/health" },
    @{ Name = "Resource Service"; Url = "http://localhost:8083/health" },
    @{ Name = "PostgreSQL Database"; Url = "http://localhost:8081/health/database" }
)

$allHealthy = $true
foreach ($service in $services) {
    $isHealthy = Test-ServiceHealth -ServiceName $service.Name -Url $service.Url
    if (-not $isHealthy) {
        $allHealthy = $false
    }
}

# Show container status
Write-Host "`n=== Container Status ===" -ForegroundColor Cyan
docker-compose ps

# Show logs if any service failed
if (-not $allHealthy) {
    Write-Host "`n=== Service Logs (Last 50 lines) ===" -ForegroundColor Yellow
    docker-compose logs --tail=50
}

# Summary
if ($allHealthy) {
    Write-Host "`n🎉 All services are running successfully!" -ForegroundColor Green
    Write-Host "Access points:" -ForegroundColor Cyan
    Write-Host "- API Gateway: http://localhost:8080" -ForegroundColor White
    Write-Host "- User Service: http://localhost:8081" -ForegroundColor White
    Write-Host "- Booking Service: http://localhost:8082" -ForegroundColor White
    Write-Host "- Resource Service: http://localhost:8083" -ForegroundColor White
    Write-Host "- Swagger UI: http://localhost:8080/swagger" -ForegroundColor White
} else {
    Write-Host "`n❌ Some services failed to start properly. Check the logs above." -ForegroundColor Red
    Write-Host "To stop services: docker-compose down" -ForegroundColor Yellow
}

Write-Host "`nTo stop all services: docker-compose down" -ForegroundColor Cyan
Write-Host "To view logs: docker-compose logs -f [service-name]" -ForegroundColor Cyan
