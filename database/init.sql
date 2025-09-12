-- DriveMate Database Schema
-- Generated from local PostgreSQL database

-- Create database (Railway will handle this)
-- CREATE DATABASE drivemate_db;

-- Users table
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Email" VARCHAR(255) UNIQUE NOT NULL,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "FirstName" VARCHAR(100),
    "LastName" VARCHAR(100),
    "PhoneNumber" VARCHAR(20),
    "Role" VARCHAR(50) DEFAULT 'User',
    "IsEmailVerified" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Instructors table
CREATE TABLE IF NOT EXISTS "Instructors" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UserId" UUID REFERENCES "Users"("Id") ON DELETE CASCADE,
    "LicenseNumber" VARCHAR(50) UNIQUE,
    "ExperienceYears" INTEGER,
    "Rating" DECIMAL(3,2) DEFAULT 0.00,
    "IsVerified" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Instructor Applications table
CREATE TABLE IF NOT EXISTS "InstructorApplications" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UserId" UUID REFERENCES "Users"("Id") ON DELETE CASCADE,
    "LicenseNumber" VARCHAR(50),
    "ExperienceYears" INTEGER,
    "ApplicationStatus" VARCHAR(50) DEFAULT 'Pending',
    "SubmittedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "ReviewedAt" TIMESTAMP NULL,
    "ReviewedBy" UUID REFERENCES "Users"("Id") NULL,
    "Notes" TEXT
);

-- Instructor Documents table
CREATE TABLE IF NOT EXISTS "InstructorDocuments" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "InstructorApplicationId" UUID REFERENCES "InstructorApplications"("Id") ON DELETE CASCADE,
    "DocumentType" VARCHAR(100),
    "DocumentUrl" VARCHAR(500),
    "UploadedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create indexes for better performance
CREATE INDEX IF NOT EXISTS "IX_Users_Email" ON "Users"("Email");
CREATE INDEX IF NOT EXISTS "IX_Instructors_UserId" ON "Instructors"("UserId");
CREATE INDEX IF NOT EXISTS "IX_InstructorApplications_UserId" ON "InstructorApplications"("UserId");
CREATE INDEX IF NOT EXISTS "IX_InstructorDocuments_ApplicationId" ON "InstructorDocuments"("InstructorApplicationId");
