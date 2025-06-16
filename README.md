# 🧠 Object Detection System - YOLOv8 + ASP.NET Core + FastAPI 

## 📌 Project Overview
This is a full-stack object detection system leveraging the power of YOLOv8, built with:

- Frontend to upload images and receive results.

- ASP.NET Core backend to manage user sessions, image metadata, Redis caching, and communication with a Python FastAPI service.

- FastAPI (Python) to process images and return YOLOv8 inference results.

- SQL Server for persistent data storage (User, MetadataImage, Request).

- Redis as a caching layer to avoid redundant processing of duplicate images.

## 🧱 System Architecture

```
User ──▶ ASP.NET Core API ──▶ Redis Cache
           │               └─ (If cache miss) ─▶ FastAPI (YOLOv8)
           ▼
       SQL Server
    (User, MetadataImage, Request)

---
```

## 🧩 Key Components
### ✅ ASP.NET Core (.NET 6)

- UserController: Generates new user sessions (user_id).

- ImageController: Handles image uploads, checks Redis cache, sends image to FastAPI, stores metadata, and logs the request.

- AppDbContext: Entity Framework context handling database access.

### ✅ FastAPI (Python)
- Receives base64-encoded or URL images.

- Runs YOLOv8 detection and returns the results in JSON.

### ✅ YOLOv8 (Ultralytics)
- Multiple YOLO models supported: nano, small, medium, large, and x.

- Processes and annotates detected objects.

### ✅ Redis
- Used to cache inference results keyed by image hash.

### ✅ SQL Server
- `User`: Tracks user sessions (`user_id`, `created_at`).

- `MetadataImage`: Stores image metadata and detection result.

- `Request`: Logs each detection request with reference to `User` and `MetadataImage`.


### 🗃️ Project Structure

```
.
├── backend/                      # FastAPI + YOLOv8 logic
│   ├── api/routes/               # FastAPI routes
│   ├── core/                     # Model loader logic
│   ├── detect_model/             # Inference pipeline
│   ├── schema/                   # Pydantic schemas
│   └── logs/                     # Logging utilities
│
├── WebApplication1/              # ASP.NET Core Web API
│   ├── Controllers/              # C# controllers
│   ├── Models/                   # EF models
│   ├── Data/                     # DbContext
│   ├── Extensions/               # Helpers (e.g., SaveMetadataAsync)
│   ├── Service/                  # Redis + FastAPI client
│   └── wwwroot/                  # Static file directory
│
├── appsettings.json              # ASP.NET Core configuration
├── Program.cs                    # Entry point
└── README.md                     # (This file)

---
```

## 🚀 How to Run
🏃 **Run ASP.NET Core backend**

```
#bash

cd WebApplication1
dotnet build
dotnet run
---
```
Available at: `http://localhost:5000`

🏃 **Run FastAPI backend**

```
#bash
cd backend
uvicorn main:app --host 0.0.0.0 --port 8000
---
```
Available at: `http://localhost:8000/api/detect`

## ⚠️ Notes

- Redis is used to avoid redundant YOLO processing on duplicate images.

- `allow_origins=["*"]` is configured for development; update it before deploying.

- Make sure `UploadSettings:UploadPath` is configured correctly in `appsettings.json`.