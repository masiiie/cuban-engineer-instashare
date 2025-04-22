# 👩🏼‍💻📂 InstaShare 📂👩🏼‍💻

A scalable distributed file sharing system built with .NET and Angular.

## Technologies

- Frontend: Angular 18 (Latest LTS version)
- Backend: .NET
- Database: PostgreSQL
- File Storage: Azure Blob Storage (with local development alternative)

## Prerequisites

Before running the project, ensure you have the following installed:

- Node.js and npm
- .NET SDK
- Docker
- PostgreSQL (for local development)

## Getting Started

### Backend Setup

1. Navigate to the backend folder:
```bash
cd backend
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Configure the database:
   - The application uses PostgreSQL
   - Update the `postgresConnection` in appsettings.json to point to your database
   - For development, you can use pgAdmin locally
   - For production, it's recommended to use a cloud database

4. Configure Azure Blob Storage:
   - The application implements two storage services:
     - `BlobStorageService`: For actual Azure Blob Storage usage
     - `DevelopmentBlobAzureService`: A simulation for local development
   - Set `UseDevStorage` in appsettings.json:
     - `true`: Uses local development storage
     - `false`: Uses actual Azure Blob Storage (requires Azure credentials)

### Frontend Setup

1. Navigate to the frontend folder:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Configure backend URL:
   - Update the environment files with the correct backend URL

## Running the Application

### Backend
```bash
cd backend/InstaShare.WebApi
dotnet run
```

### Frontend
```bash
cd frontend
ng serve
```

Navigate to `http://localhost:4200/`

## Running Tests

### Backend Tests

1. Ensure Docker is running
2. Pull the required PostgreSQL image:
```bash
docker pull postgis/postgis:latest
```

3. Run the tests:
```bash
cd backend
dotnet test
```

## Architecture and Design Patterns

The backend implements several design patterns for maintainability and scalability:

- **Unit of Work Pattern**: Manages transactions and ensures data consistency
- **Repository Pattern**: Abstracts data access logic
- **Command Pattern**: Encapsulates business logic in discrete, reusable units