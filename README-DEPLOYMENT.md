# Railway Deployment Guide for DriveMate System

This guide will help you deploy your API Gateway and UserService to Railway with CI/CD automation.

## Prerequisites

1. **Railway Account**: Sign up at [railway.app](https://railway.app)
2. **GitHub Repository**: Push your code to GitHub
3. **Railway CLI** (optional): Install for local testing

## Step 1: Database Setup on Railway

1. **Create PostgreSQL Database**:
   - Go to Railway dashboard
   - Click "New Project" → "Provision PostgreSQL"
   - Note the database connection details

2. **Set Environment Variables**:
   ```
   POSTGRES_PASSWORD=your_secure_password
   USERSERVICECONNECTION=postgresql://username:password@host:port/database
   ```

## Step 2: Deploy UserService

1. **Create New Service**:
   - In Railway dashboard, click "New Service"
   - Connect your GitHub repository
   - Select "UserService" as service name

2. **Configure Build Settings**:
   - Set build command: Use Dockerfile at `src/UserService/Dockerfile`
   - Set root directory: `/` (repository root)

3. **Set Environment Variables**:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:80
   ConnectionStrings__USERSERVICECONNECTION=${{Postgres.DATABASE_URL}}
   JWT_KEY=your_jwt_secret_key_here
   JWT_ISSUER=https://your-userservice.railway.app
   JWT_AUDIENCE=https://your-apigateway.railway.app
   ```

## Step 3: Deploy API Gateway

1. **Create New Service**:
   - Create another service for "ApiGateway"
   - Connect same GitHub repository

2. **Configure Build Settings**:
   - Set build command: Use Dockerfile at `src/ApiGetwate/Dockerfile`
   - Set root directory: `/` (repository root)

3. **Set Environment Variables**:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:80
   USERSERVICE_HOST=your-userservice.railway.app
   PRODUCTSERVICE_HOST=your-productservice.railway.app
   APIGATEWAY_HOST=your-apigateway.railway.app
   JWT_KEY=your_jwt_secret_key_here
   JWT_ISSUER=https://your-userservice.railway.app
   JWT_AUDIENCE=https://your-apigateway.railway.app
   ```

## Step 4: GitHub Actions CI/CD Setup

1. **Add Railway Token to GitHub Secrets**:
   - Go to your GitHub repository
   - Settings → Secrets and variables → Actions
   - Add new secret: `RAILWAY_TOKEN`
   - Get token from Railway dashboard → Account Settings → Tokens

2. **GitHub Actions Workflow**:
   - The workflow file is already created at `.github/workflows/deploy.yml`
   - It will automatically deploy on push to main/master branch

## Step 5: Environment Variables Reference

### Required Environment Variables for UserService:
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
ConnectionStrings__USERSERVICECONNECTION=postgresql://...
JWT_KEY=your_jwt_secret_key
JWT_ISSUER=https://your-userservice.railway.app
JWT_AUDIENCE=https://your-apigateway.railway.app
```

### Required Environment Variables for API Gateway:
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
USERSERVICE_HOST=your-userservice.railway.app
PRODUCTSERVICE_HOST=your-productservice.railway.app (for future)
APIGATEWAY_HOST=your-apigateway.railway.app
JWT_KEY=your_jwt_secret_key
JWT_ISSUER=https://your-userservice.railway.app
JWT_AUDIENCE=https://your-apigateway.railway.app
```

## Step 6: Testing Deployment

1. **Check Service Health**:
   - UserService: `https://your-userservice.railway.app/swagger`
   - API Gateway: `https://your-apigateway.railway.app/swagger`

2. **Test API Endpoints**:
   - Authentication: `POST https://your-apigateway.railway.app/auth/login`
   - User registration: `POST https://your-apigateway.railway.app/auth/register`

## Troubleshooting

### Common Issues:

1. **Database Connection Failed**:
   - Verify `USERSERVICECONNECTION` environment variable
   - Check PostgreSQL service is running
   - Ensure database exists

2. **JWT Token Issues**:
   - Verify `JWT_KEY` is the same across all services
   - Check `JWT_ISSUER` and `JWT_AUDIENCE` URLs

3. **Service Communication**:
   - Verify `USERSERVICE_HOST` in API Gateway
   - Check Railway service URLs are correct

4. **Build Failures**:
   - Check Dockerfile paths are correct
   - Verify all dependencies are restored
   - Check build logs in Railway dashboard

## Next Steps

1. **Add SSL/TLS**: Railway provides HTTPS by default
2. **Custom Domain**: Configure custom domain in Railway settings
3. **Monitoring**: Set up logging and monitoring
4. **Scaling**: Configure auto-scaling based on traffic

## Support

- Railway Documentation: [docs.railway.app](https://docs.railway.app)
- GitHub Actions: [docs.github.com/actions](https://docs.github.com/en/actions)
