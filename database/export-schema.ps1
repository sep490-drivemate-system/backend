# Export database schema from local PostgreSQL
param(
    [string]$DatabaseName = "drivemate_db",
    [string]$Username = "postgres",
    [string]$ServerHost = "localhost",
    [string]$Port = "5432",
    [SecureString]$Password = $null
)

Write-Host "Exporting database schema..." -ForegroundColor Green

# Find PostgreSQL installation
$PgDumpPaths = @(
    "C:\Program Files\PostgreSQL\16\bin\pg_dump.exe",
    "C:\Program Files\PostgreSQL\15\bin\pg_dump.exe",
    "C:\Program Files\PostgreSQL\14\bin\pg_dump.exe",
    "C:\Program Files\PostgreSQL\13\bin\pg_dump.exe",
    "pg_dump"  # If in PATH
)

$PgDumpPath = $null
foreach ($path in $PgDumpPaths) {
    if (Test-Path $path -ErrorAction SilentlyContinue) {
        $PgDumpPath = $path
        break
    }
    if ($path -eq "pg_dump") {
        try {
            & pg_dump --version | Out-Null
            $PgDumpPath = "pg_dump"
            break
        } catch {
            continue
        }
    }
}

if (-not $PgDumpPath) {
    Write-Host "✗ pg_dump not found. Please install PostgreSQL or add it to PATH." -ForegroundColor Red
    Write-Host "Download from: https://www.postgresql.org/download/" -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ Found pg_dump at: $PgDumpPath" -ForegroundColor Green

# Get password if not provided
if (-not $Password) {
    $Password = Read-Host "Enter PostgreSQL password" -AsSecureString
}

# Convert SecureString to plain text for pg_dump
$PlainPassword = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($Password))

# Set environment variable for password
$env:PGPASSWORD = $PlainPassword

try {
    # Create database directory if not exists
    if (-not (Test-Path "database")) {
        New-Item -ItemType Directory -Path "database" -Force | Out-Null
    }
    
    # Export schema only (no data)
    Write-Host "Exporting schema to database/init.sql..." -ForegroundColor Yellow
    & $PgDumpPath -h $ServerHost -p $Port -U $Username -d $DatabaseName --schema-only --no-owner --no-privileges -f "database/init.sql"
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Schema exported successfully to database/init.sql" -ForegroundColor Green
        
        # Show file size
        $FileSize = (Get-Item "database/init.sql").Length
        Write-Host "File size: $([math]::Round($FileSize/1KB, 2)) KB" -ForegroundColor Gray
    } else {
        Write-Host "✗ Schema export failed with exit code: $LASTEXITCODE" -ForegroundColor Red
        exit 1
    }
    
    # Export data (optional)
    $ExportData = Read-Host "Do you want to export data too? (y/N)"
    if ($ExportData -eq "y" -or $ExportData -eq "Y") {
        Write-Host "Exporting data to database/data.sql..." -ForegroundColor Yellow
        & $PgDumpPath -h $ServerHost -p $Port -U $Username -d $DatabaseName --data-only --no-owner --no-privileges -f "database/data.sql"
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ Data exported successfully to database/data.sql" -ForegroundColor Green
            $DataFileSize = (Get-Item "database/data.sql").Length
            Write-Host "Data file size: $([math]::Round($DataFileSize/1KB, 2)) KB" -ForegroundColor Gray
        } else {
            Write-Host "⚠ Data export failed, but schema export was successful" -ForegroundColor Yellow
        }
    }
    
} catch {
    Write-Host "✗ Export failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} finally {
    # Clear password from environment
    Remove-Item Env:PGPASSWORD -ErrorAction SilentlyContinue
}

Write-Host "`nNext steps:" -ForegroundColor Cyan
Write-Host "1. Review database/init.sql file" -ForegroundColor White
Write-Host "2. Run migrate-to-railway.ps1 with Railway connection string" -ForegroundColor White
Write-Host "3. Or use auto-migration in UserService (recommended)" -ForegroundColor White
