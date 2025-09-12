# Script to migrate database schema to Railway PostgreSQL
param(
    [string]$RailwayConnectionString = "",
    [string]$LocalConnectionString = "Host=localhost;Database=drivemate_db;Username=postgres;Password=your_password"
)

Write-Host "Database Migration to Railway" -ForegroundColor Green

if (-not $RailwayConnectionString) {
    Write-Host "Usage: .\migrate-to-railway.ps1 -RailwayConnectionString 'postgresql://user:pass@host:port/db'" -ForegroundColor Yellow
    Write-Host "Get connection string from Railway PostgreSQL service" -ForegroundColor Yellow
    exit 1
}

# Check if psql is available
try {
    psql --version | Out-Null
    Write-Host "✓ PostgreSQL client available" -ForegroundColor Green
} catch {
    Write-Host "✗ PostgreSQL client not found. Please install PostgreSQL tools." -ForegroundColor Red
    Write-Host "Download from: https://www.postgresql.org/download/" -ForegroundColor Yellow
    exit 1
}

# Run migration script
Write-Host "Running database migration..." -ForegroundColor Yellow
try {
    psql $RailwayConnectionString -f "database/init.sql"
    Write-Host "✓ Database migration completed successfully" -ForegroundColor Green
} catch {
    Write-Host "✗ Migration failed. Check connection string and permissions." -ForegroundColor Red
    exit 1
}

Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. Verify tables created in Railway PostgreSQL" -ForegroundColor White
Write-Host "2. Update UserService connection string" -ForegroundColor White
Write-Host "3. Test UserService deployment" -ForegroundColor White
