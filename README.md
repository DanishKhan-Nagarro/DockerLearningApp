# Docker Learning Assignment

## Overview

This project demonstrates containerization of ASP.NET Core applications using Docker and Docker Compose.

The solution contains:
- ASP.NET Core MVC frontend application
- ASP.NET Core Web API backend application
- A simple Tasks page for basic create and read operations
- Dockerfiles for both applications
- Docker Compose configuration for multi-container setup
- Private Docker registry implementation
- SQLite database integration
- Multi-stage Docker builds
- Custom Docker network
- Named Docker volumes
- Bind mounts
- Docker security best practices
- Private Docker registry
- CI/CD pipeline using GitHub Actions

The frontend application displays weather data from the backend API and also provides a Tasks page for adding and viewing task items. Task data is persisted using SQLite.

---

## Technologies Used

- .NET 10
- ASP.NET Core MVC
- ASP.NET Core Web API
- SQLite
- Docker
- Docker Compose
- Docker Networks
- Docker Volumes
- Docker Security Best Practices
- Docker Bench Security

---

## Project Structure

```text
DockerLearningApp
│
├── .github
│   └── workflows
│       └── docker-build.yml
│
├── FrontendApp
│   ├── Controllers
│   ├── Models
│   ├── Services
│   ├── Views
│   ├── Dockerfile
│   └── FrontendApp.csproj
│
├── BackendApi
│   ├── Data
│   ├── Models
│   ├── Dockerfile
│   └── BackendApi.csproj
│
├── docker-compose.yml
├── .gitignore
└── README.md
```

## Applications

### FrontendApp

ASP.NET Core MVC application that displays weather forecast data received from the backend API and provides a Tasks page for basic task management.

### BackendApi

ASP.NET Core Web API application that provides weather forecast data and simple task API endpoints.

---

## Prerequisites

Before running the project, ensure the following are installed:

- Docker 20.10 or later
- Docker Compose
- .NET 10 SDK
- Visual Studio Code / Visual Studio

---

## Verify Docker Installation

```bash
docker --version
docker compose version
docker run hello-world
```

---

## Run the Application Locally

### Backend Application

```bash
cd BackendApi
dotnet run
```

### Frontend Application

Open a new terminal:

```bash
cd FrontendApp
dotnet run
```

### Local URLs

- Frontend Application: http://localhost:5117
- Backend API: http://localhost:5102/weatherforecast
- Tasks Page: http://localhost:5117/Tasks

---

## Build Docker Images

### Frontend Application

```bash
cd FrontendApp
docker build -t dockerlearning-frontend .
```

### Backend Application

```bash
cd BackendApi
docker build -t dockerlearning-backend .
```

---

## Run Containers Individually

### Run Frontend Container

```bash
docker run -d -p 8081:8080 --name frontend-container dockerlearning-frontend
```

### Run Backend Container

```bash
docker run -d -p 8082:8080 --name backend-container dockerlearning-backend
```

---

## Docker Commands Used

### List Containers

```bash
docker ps
docker ps -a
```

### Stop Containers

```bash
docker stop frontend-container
docker stop backend-container
```

### Start Containers

```bash
docker start frontend-container
docker start backend-container
```

### Remove Containers

```bash
docker rm frontend-container
docker rm backend-container
```

### View Logs

```bash
docker logs frontend-container
docker logs backend-container
```

### Inspect Containers

```bash
docker inspect frontend-container
docker inspect backend-container
```

---

## Docker Compose Configuration

The application uses Docker Compose to run the frontend and backend containers together.

### Run Multi-Container Application

```bash
docker compose up -d --build
```

### Stop Multi-Container Application

```bash
docker compose down
```

### Docker URLs

- Frontend Application: http://localhost:8081
- Backend API: http://localhost:8082/weatherforecast
- Tasks Page: http://localhost:8081/Tasks

---

## Private Docker Registry

A local private Docker registry was used for pushing and pulling Docker images.

### Run Local Registry

```bash
sudo docker run -d -p 5001:5000 --name local-registry registry:2
```

### Tag Images

```bash
sudo docker tag dockerlearning-frontend localhost:5001/dockerlearning-frontend:v1

sudo docker tag dockerlearning-backend localhost:5001/dockerlearning-backend:v1
```

### Push Images

```bash
sudo docker push localhost:5001/dockerlearning-frontend:v1

sudo docker push localhost:5001/dockerlearning-backend:v1
```

### Pull Images

```bash
sudo docker pull localhost:5001/dockerlearning-frontend:v1

sudo docker pull localhost:5001/dockerlearning-backend:v1
```

### Registry URL

- Private Docker Registry: http://localhost:5001/v2/_catalog

---

## CI/CD Configuration

This repository includes a GitHub Actions workflow located at `.github/workflows/docker-build.yml`. It is configured to trigger on pushes to the `main` branch and verify that both Docker images build successfully.

---

## Frontend Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["FrontendApp.csproj", "./"]

RUN dotnet restore "FrontendApp.csproj"

COPY . .

RUN dotnet publish "FrontendApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "FrontendApp.dll"]
```

---

## Backend Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["BackendApi.csproj", "./"]

RUN dotnet restore "BackendApi.csproj"

COPY . .

RUN dotnet publish "BackendApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "BackendApi.dll"]
```

---

## Docker Compose File

```yaml
services:
  frontendapp:
    build:
      context: ./FrontendApp
      dockerfile: Dockerfile
    image: dockerlearning-frontend
    container_name: frontend-container
    ports:
      - "8081:8080"
    depends_on:
      - backendapi

  backendapi:
    build:
      context: ./BackendApi
      dockerfile: Dockerfile
    image: dockerlearning-backend
    container_name: backend-container
    ports:
      - "8082:8080"
```

---

## Security Enhancements

The application implements several Docker security best practices:

- Containers run as a non-root user using USER app
- Read-only bind mounts used for secret files
- Least privilege container execution
- Multi-stage builds reduce attack surface
- Docker image inspection and validation performed
- Docker Bench Security audit executed

## Key Learnings

This project helped in understanding:

- Docker image creation
- Multi-stage Docker builds
- Container lifecycle management
- Docker networking
- Multi-container applications using Docker Compose
- Container communication using service names
- Docker image push and pull operations using a private registry
- Lightweight database-backed task management using SQLite

---

## Assignment Requirements Covered

| Requirement | Status |
|-------------|--------|
| Install Docker and Docker Compose | Completed |
| Verify Docker Installation | Completed |
| Create Web Application | Completed |
| Dockerfile Creation | Completed |
| Build Docker Images | Completed |
| Run Containers | Completed |
| Docker Commands | Completed |
| Inspect Containers and Logs | Completed |
| Docker Compose Setup | Completed |
| Multi-Container Application | Completed |
| Private Docker Registry | Completed |
| Push/Pull Docker Images | Completed |
| README Documentation | Completed |
| Database-backed enhancement | Completed |

# Author

**Danish Khan**