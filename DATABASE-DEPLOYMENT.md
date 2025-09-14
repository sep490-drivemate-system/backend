# PostgreSQL Database Deployment Guide

## 🗄️ **Cách deploy database từ local lên Railway:**

### **Option 1: Sử dụng Entity Framework Migrations (Recommended)**

#### 1.1 Tạo Migration từ existing database
```bash
# Trong UserService project
cd src/UserService
dotnet ef migrations add InitialCreate --context ApplicationDbContext
dotnet ef database update
```

#### 1.2 Deploy với Migration tự động
Thêm vào `UserService/Program.cs`:
```csharp
// Auto-migrate database on startup (Production)
if (app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
}
```

### **Option 2: Export/Import SQL Schema**

#### 2.1 Export schema từ local database
```bash
# Chạy script export
.\database\export-schema.ps1
```

#### 2.2 Import vào Railway PostgreSQL
```bash
# Lấy connection string từ Railway
.\database\migrate-to-railway.ps1 -RailwayConnectionString "postgresql://user:pass@host:port/db"
```

### **Option 3: Sử dụng Railway PostgreSQL Template**

#### 3.1 Trong Railway Dashboard:
1. **New Service** → **Database** → **PostgreSQL**
2. Railway tự động tạo empty database
3. Lấy connection string từ **Variables** tab

#### 3.2 Cấu hình connection string:
```
DATABASE_URL=postgresql://postgres:password@host:port/railway
USERSERVICECONNECTION=${{Postgres.DATABASE_URL}}
```

## 🔧 **Setup cho UserService:**

### Cập nhật appsettings.Production.json:
```json
{
  "ConnectionStrings": {
    "USERSERVICECONNECTION": "${DATABASE_URL}"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### Environment Variables trên Railway:
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
ConnectionStrings__USERSERVICECONNECTION=${{Postgres.DATABASE_URL}}
```

## 📊 **Kiểm tra Database sau khi deploy:**

### Test connection:
```bash
# Connect to Railway PostgreSQL
psql $DATABASE_URL

# List tables
\dt

# Check specific table
SELECT * FROM "Users" LIMIT 5;
```

### Health check endpoint (thêm vào UserService):
```csharp
app.MapGet("/health/database", async (ApplicationDbContext context) =>
{
    try
    {
        await context.Database.CanConnectAsync();
        return Results.Ok(new { status = "healthy", database = "connected" });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database connection failed: {ex.Message}");
    }
});
```

## 🚀 **Deployment Flow:**

```
1. Railway PostgreSQL → Empty database
2. UserService deploy → Auto-migrate on startup
3. Tables created automatically
4. Ready to use!
```

## ⚠️ **Important Notes:**

- Railway PostgreSQL có **shared CPU/memory** ở free tier
- Database sẽ **sleep** sau 1h không sử dụng
- **Backup** data quan trọng thường xuyên
- Sử dụng **connection pooling** cho performance tốt hơn

## 🔍 **Troubleshooting:**

### Lỗi thường gặp:
1. **Connection timeout**: Kiểm tra connection string
2. **Migration failed**: Đảm bảo EF Core tools được cài
3. **Permission denied**: Kiểm tra database user permissions
4. **SSL required**: Thêm `sslmode=require` vào connection string
