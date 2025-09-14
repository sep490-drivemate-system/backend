# Auto-Deploy Setup: GitHub → Docker Hub → Railway

## Quy trình tự động hoá hoàn chỉnh:

```
GitHub Push → GitHub Actions → Docker Hub → Railway Redeploy
```

## Cách hoạt động:

### 1. **GitHub Actions** (Đã setup)
- Khi push code lên GitHub
- Tự động build Docker images
- Push lên Docker Hub với tag `latest`
- Trigger Railway redeploy qua webhook

### 2. **Railway Auto-Redeploy Options:**

#### **Option A: Railway Webhooks (Recommended)**
```bash
# Flow: GitHub Actions → Railway Webhook → Auto Redeploy
Push code → Build Docker → Trigger Railway → Pull new image
```

#### **Option B: Manual Redeploy**
```bash
# Cần click manual trong Railway dashboard
Push code → Build Docker → Manual click "Redeploy"
```

## Setup Railway Webhooks:

### Bước 1: Lấy Railway Webhook URLs
1. Railway Dashboard → Service → Settings → Webhooks
2. Copy webhook URL cho mỗi service
3. Add vào GitHub Secrets:
   ```
   RAILWAY_USERSERVICE_WEBHOOK_URL=https://railway.app/api/v2/webhooks/...
   RAILWAY_APIGATEWAY_WEBHOOK_URL=https://railway.app/api/v2/webhooks/...
   ```

### Bước 2: GitHub Secrets cần thiết
```
DOCKER_USERNAME=your_dockerhub_username
DOCKER_PASSWORD=your_dockerhub_password
RAILWAY_USERSERVICE_WEBHOOK_URL=webhook_url_for_userservice
RAILWAY_APIGATEWAY_WEBHOOK_URL=webhook_url_for_apigateway
```

## Quy trình deploy hoàn chỉnh:

### 1. **Lần đầu setup:**
```bash
# 1. Tạo Docker Hub repositories
# 2. Setup GitHub Secrets
# 3. Deploy services trên Railway từ Docker Hub images
# 4. Cấu hình Railway webhooks
```

### 2. **Mỗi lần update code:**
```bash
git add .
git commit -m "Update features"
git push origin main

# Tự động diễn ra:
# → GitHub Actions build Docker images
# → Push lên Docker Hub
# → Trigger Railway webhooks
# → Railway pull images mới và redeploy
```

## Alternative: Watchtower (Advanced)

Nếu muốn Railway tự động check Docker Hub:

```yaml
# Thêm vào docker-compose hoặc Railway config
watchtower:
  image: containrrr/watchtower
  environment:
    - WATCHTOWER_POLL_INTERVAL=300  # Check every 5 minutes
    - WATCHTOWER_CLEANUP=true
```

## Kết quả:

✅ **Push code** → ✅ **Auto build** → ✅ **Auto deploy** → ✅ **Live update**

**Hoàn toàn tự động, không cần thao tác manual!**
