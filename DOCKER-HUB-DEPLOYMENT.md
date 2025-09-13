# Docker Hub + Railway Deployment Guide

## Bước 1: Cấu hình Docker Hub

### 1.1 Tạo Docker Hub account
- Truy cập [hub.docker.com](https://hub.docker.com) và đăng ký
- Tạo repository: `drivemate-userservice` và `drivemate-apigateway`

### 1.2 Cấu hình GitHub Secrets
Trong GitHub repository → Settings → Secrets and variables → Actions:

```
DOCKER_USERNAME=your_dockerhub_username
DOCKER_PASSWORD=your_dockerhub_password_or_token
```

## Bước 2: Push code và build Docker images

```bash
git add .
git commit -m "Add Docker Hub workflow"
git push origin main
```

GitHub Actions sẽ tự động:
- Build Docker images cho UserService và API Gateway
- Push lên Docker Hub với tags: `latest`, `main-<commit-sha>`

## Bước 3: Deploy trên Railway từ Docker Hub

### 3.1 Tạo UserService
1. Railway Dashboard → **New Service** → **Docker Image**
2. **Image URL**: `your_dockerhub_username/drivemate-userservice:latest`
3. **Service name**: `userservice`

### 3.2 Environment Variables cho UserService
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
ConnectionStrings__USERSERVICECONNECTION=${{Postgres.DATABASE_URL}}
JWT_KEY=your_super_secret_jwt_key_at_least_32_characters_long
JWT_ISSUER=https://userservice-production-xxxx.up.railway.app
JWT_AUDIENCE=https://apigateway-production-xxxx.up.railway.app
```

### 3.3 Tạo API Gateway
1. Railway Dashboard → **New Service** → **Docker Image**
2. **Image URL**: `your_dockerhub_username/drivemate-apigateway:latest`
3. **Service name**: `apigateway`

### 3.4 Environment Variables cho API Gateway
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
USERSERVICE_HOST=userservice-production-xxxx.up.railway.app
APIGATEWAY_HOST=apigateway-production-xxxx.up.railway.app
JWT_KEY=your_super_secret_jwt_key_at_least_32_characters_long
JWT_ISSUER=https://userservice-production-xxxx.up.railway.app
JWT_AUDIENCE=https://apigateway-production-xxxx.up.railway.app
```

## Bước 4: Cấu hình Auto-Deploy

### 4.1 Railway Webhooks (Tùy chọn)
Để tự động redeploy khi có Docker image mới:
1. Railway service → Settings → Webhooks
2. Trigger redeploy khi Docker Hub có image mới

### 4.2 Manual Redeploy
Mỗi khi push code mới:
1. GitHub Actions build image mới
2. Vào Railway Dashboard → Service → **Redeploy**
3. Railway sẽ pull image mới từ Docker Hub

## Bước 5: Kiểm tra Deployment

### 5.1 Health Check URLs
- UserService: `https://userservice-production-xxxx.up.railway.app/swagger`
- API Gateway: `https://apigateway-production-xxxx.up.railway.app/swagger`

### 5.2 Test API Endpoints
```bash
# Test qua API Gateway
curl https://apigateway-production-xxxx.up.railway.app/auth/health

# Test trực tiếp UserService
curl https://userservice-production-xxxx.up.railway.app/api/auth/health
```

## Lợi ích của phương pháp này:

✅ **Tách biệt build và deploy**: Build trên GitHub, deploy trên Railway
✅ **Caching tốt hơn**: Docker layers được cache trên Docker Hub
✅ **Rollback dễ dàng**: Có thể deploy lại image cũ bất kỳ lúc nào
✅ **Multi-platform**: Image có thể deploy ở nhiều nơi khác nhau
✅ **Version control**: Mỗi commit có image riêng với tag

## Troubleshooting

### Lỗi thường gặp:
1. **Docker build failed**: Kiểm tra Dockerfile syntax
2. **Push to Docker Hub failed**: Kiểm tra DOCKER_USERNAME và DOCKER_PASSWORD
3. **Railway pull failed**: Đảm bảo image là public hoặc cấu hình credentials
4. **Service không start**: Kiểm tra environment variables và logs
