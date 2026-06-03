# AiImageTaskManager

AI 圖像生成任務管理平台與 API 自動化測試系統。
本專案使用 ASP.NET Core Web API 建立圖片生成任務管理流程，支援任務建立、狀態追蹤、背景處理、圖片結果儲存，以及 API 測試案例管理與執行紀錄查詢。

## 專案目標

本專案模擬一個 AI 圖像生成後端系統，使用者可以建立圖片生成任務，系統會透過 BackgroundService 非同步處理任務，並在任務完成後產生圖片結果紀錄。

此外，專案也加入 API 自動化測試模組，可建立測試案例、執行 HTTP 請求、驗證狀態碼，並保存測試執行紀錄。

## 技術棧

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* BackgroundService
* Swagger / OpenAPI
* xUnit
* WebApplicationFactory
* GitHub Actions CI
* Local File Storage

## 專案架構

```text
AiImageTaskManager
├── AiImageTaskManager.Api
│   ├── Controllers
│   ├── Program.cs
│   └── wwwroot/images/generated
│
├── AiImageTaskManager.Application
│   ├── DTOs
│   └── Interfaces
│
├── AiImageTaskManager.Domain
│   ├── Entities
│   └── Enums
│
├── AiImageTaskManager.Infrastructure
│   ├── BackgroundJobs
│   ├── Data
│   ├── FileStorage
│   └── Services
│
└── AiImageTaskManager.IntegrationTests
    ├── ApiTestCases
    ├── Factories
    └── ImageTasks
```

## 核心功能

### Image Task Management

* 建立圖片生成任務
* 查詢所有任務
* 查詢單一任務
* 取消任務
* 任務狀態追蹤

任務狀態包含：

```text
Pending
Running
Completed
Failed
Cancelled
```

### Background Processing

使用 ASP.NET Core BackgroundService 模擬圖片生成流程。

任務建立後，系統會自動將任務由：

```text
Pending → Running → Completed
```

並在完成後建立圖片結果紀錄。

### Generated Images

任務完成後會產生圖片檔案並儲存至：

```text
wwwroot/images/generated
```

同時將圖片路徑、尺寸、檔案大小與建立時間寫入資料庫。

圖片可透過瀏覽器直接存取，例如：

```text
/images/generated/task-1-20260603120000.png
```

### API Test Case Module

系統提供 API 自動化測試案例管理功能。

支援：

* 建立 API 測試案例
* 查詢測試案例
* 執行 HTTP request
* 驗證 HTTP status code
* 儲存測試執行紀錄
* 查詢測試歷史

## API Endpoints

### Image Tasks

```http
GET    /api/image-tasks
POST   /api/image-tasks
GET    /api/image-tasks/{id}
POST   /api/image-tasks/{id}/cancel
GET    /api/image-tasks/{id}/images
```

### API Test Cases

```http
GET    /api/test-cases
POST   /api/test-cases
GET    /api/test-cases/{id}
POST   /api/test-cases/{id}/run
GET    /api/test-cases/{id}/runs
```

## Example Request

### Create Image Task

```http
POST /api/image-tasks
```

```json
{
  "prompt": "a realistic train running on railway tracks",
  "negativePrompt": "blurry, low quality",
  "width": 512,
  "height": 512,
  "steps": 20,
  "cfgScale": 7,
  "seed": 12345
}
```

### Create API Test Case

```http
POST /api/test-cases
```

```json
{
  "name": "Get all image tasks",
  "method": "GET",
  "url": "https://localhost:7074/api/image-tasks",
  "headersJson": null,
  "bodyJson": null,
  "expectedStatusCode": 200
}
```

## Integration Tests

本專案使用 xUnit 與 WebApplicationFactory 建立 Integration Tests，測試環境使用 InMemoryDatabase，避免影響本機 SQLite 開發資料庫。

目前測試項目包含：

* 建立圖片任務
* 查詢圖片任務列表
* 查詢單一圖片任務
* 查詢不存在任務
* 建立 API 測試案例
* 查詢 API 測試案例列表
* 查詢單一 API 測試案例
* 查詢不存在測試案例

執行測試：

```bash
dotnet test
```

## CI/CD

本專案使用 GitHub Actions 建立 CI 流程。
每次 push 或 pull request 時，會自動執行：

```text
dotnet restore
dotnet build
dotnet test
```

## 如何執行專案

### 1. Clone 專案

```bash
git clone https://github.com/t0037798/AiImageTaskManager.git
cd AiImageTaskManager
```

### 2. 還原套件

```bash
dotnet restore
```

### 3. 建立資料庫

```bash
dotnet ef database update
```

### 4. 啟動 API

```bash
dotnet run --project AiImageTaskManager.Api
```

啟動後可開啟 Swagger：

```text
https://localhost:7074/swagger
```

## 專案亮點

* 採用分層架構設計，將 API、Application、Domain、Infrastructure 分離
* 使用 EF Core 管理資料存取與 migration
* 使用 BackgroundService 實作非同步任務處理流程
* 支援圖片結果儲存與靜態檔案存取
* 建立 API 測試案例管理與執行紀錄功能
* 使用 xUnit 建立 Integration Tests
* 使用 GitHub Actions 自動執行 build 與 test

## 未來規劃

* 串接 Stable Diffusion WebUI API 或 ComfyUI API
* 加入 JWT 使用者登入與權限管理
* 加入前端 Dashboard
* 支援更多 API response 驗證條件
* 支援多步驟 API 測試流程
* 將 SQLite 改為 PostgreSQL 或 SQL Server
* 加入 Docker Compose 部署環境
