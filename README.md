# 🔐 LearnToKeycloak - .NET 10 API with Keycloak Authentication

A modern ASP.NET Core 10.0 Web API demonstrating **Keycloak** integration for authentication and authorization with **JWT Bearer tokens**.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Keycloak](https://img.shields.io/badge/Keycloak-Authentication-blue?style=flat)](https://www.keycloak.org/)
[![OpenAPI](https://img.shields.io/badge/OpenAPI-3.0-green?style=flat)](https://www.openapis.org/)
[![Scalar](https://img.shields.io/badge/Scalar-API%20Documentation-orange?style=flat)](https://scalar.com/)
---

## 📋 Table of Contents

- [Features](#-features)
- [Technologies](#-technologies)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Keycloak Setup](#-keycloak-setup)
- [API Endpoints](#-api-endpoints)
- [Testing with Scalar UI](#-testing-with-scalar-ui)
- [Project Structure](#-project-structure)
- [License](#-license)

---

## ✨ Features

- ✅ **JWT Bearer Authentication** with Keycloak
- ✅ **OpenAPI/Swagger Documentation** with Scalar UI
- ✅ **Role-Based Access Control (RBAC)**
- ✅ **RESTful API Design**
- ✅ **Keycloak User Management**
- ✅ **Token-based Authorization**
- ✅ **CORS Support**
- ✅ **.NET 10.0** with minimal APIs

---

## 🛠️ Technologies

| Technology | Purpose |
|------------|---------|
| **.NET 10.0** | Web API Framework |
| **Keycloak** | Identity and Access Management |
| **Keycloak.AuthServices** | Keycloak integration for .NET |
| **Scalar.AspNetCore** | Modern OpenAPI documentation UI |
| **Microsoft.AspNetCore.OpenApi** | OpenAPI specification generation |

---

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- [Docker](https://www.docker.com/) (for running Keycloak)
- [Git](https://git-scm.com/)
- A REST client (Postman, Insomnia, or use Scalar UI)

---

## 🚀 Installation

### 1. Clone the repository

```bash
git clone https://github.com/yourusername/LearnToKeycloak.git
cd LearnToKeycloak/LearnToKeycloak
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Build the project

```bash
dotnet build
```

### 4. Run the application

```bash
dotnet run
```

The API will be available at: `https://localhost:7000` (or `http://localhost:5000`)

---

## ⚙️ Configuration

### appsettings.json

Update the `appsettings.json` file with your Keycloak configuration:

```json
{
  "KeycloakConfiguration": {
    "Hostname": "http://localhost:8080",
    "ClientId": "myclient",
    "Realm": "myrealm",
    "ClientSecret": "YOUR_CLIENT_SECRET",
    "ClientUUID": "YOUR_CLIENT_UUID"
  },
  "Keycloak": {
    "realm": "myrealm",
    "auth-server-url": "http://localhost:8080/",
    "ssl-required": "none",
    "resource": "myclient",
    "verify-token-audience": false,
    "credentials": {
      "secret": "YOUR_CLIENT_SECRET"
    }
  }
}
```

> ⚠️ **Security Note**: Never commit sensitive credentials to version control. Use **User Secrets** or **Environment Variables** for production.

---

## 🔑 Keycloak Setup

### Running Keycloak with Docker

```bash
docker run -p 8080:8080 \
  -e KEYCLOAK_ADMIN=admin \
  -e KEYCLOAK_ADMIN_PASSWORD=admin \
  quay.io/keycloak/keycloak:latest \
  start-dev
```

### Configure Keycloak

1. **Access Keycloak Admin Console**
   - URL: `http://localhost:8080`
   - Username: `admin`
   - Password: `admin`

2. **Create a Realm**
   - Navigate to: **Administration Console** → **Create Realm**
   - Realm name: `myrealm`

3. **Create a Client**
   - Navigate to: **Clients** → **Create Client**
   - Client ID: `myclient`
   - Client Protocol: `openid-connect`
   - Access Type: `confidential`
   - Valid Redirect URIs: `*`
   - Web Origins: `*`

4. **Get Client Secret**
   - Navigate to: **Clients** → `myclient` → **Credentials**
   - Copy the **Secret** and update your `appsettings.json`

5. **Create Users and Roles** (Optional)
   - Create users in: **Users** → **Add User**
   - Create roles in: **Realm Roles** → **Add Role**
   - Assign roles to users

---

## 📡 API Endpoints

### 🔓 Authentication

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `POST` | `/api/GetAccesToken/GetToken` | Get JWT access token | ❌ No |

### 👥 User Management

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `GET` | `/api/User/GetAllUsers` | Get all Keycloak users | ✅ Yes |

### 🎭 Role Management

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `GET` | `/api/Role/GetAllRoles` | Get all realm roles | ✅ Yes |

### 🔐 User Role Management

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `GET` | `/api/UserRole/GetUserRoles/{userId}` | Get roles for a specific user | ✅ Yes |
| `POST` | `/api/UserRole/AssignRole` | Assign role to user | ✅ Yes |

### 🔑 Auth Controller

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `GET` | `/api/Auth/GetUserInfo` | Get current user information | ✅ Yes |

---

## 🧪 Testing with Scalar UI

### Accessing the API Documentation

1. Start the application in **Development** mode
2. Navigate to: `https://localhost:7000/scalar/v1`

### Authenticating Requests

1. **Get Access Token**
   - Use the `/api/GetAccesToken/GetToken` endpoint
   - Provide Keycloak credentials
   - Copy the returned JWT token

2. **Use the Token**
   - Click the **🔒 Lock Icon** or **"Authenticate"** button in Scalar UI
   - Select **Bearer** authentication
   - Paste your JWT token
   - Click **"Authenticate"**

3. **Make Authenticated Requests**
   - Now you can test protected endpoints
   - The Bearer token will be automatically included in requests

---

## 📁 Project Structure

```
LearnToKeycloak/
├── Controllers/
│   ├── AuthController.cs          # User authentication info
│   ├── GetAccesTokenController.cs # Token generation
│   ├── RoleController.cs          # Role management
│   ├── UserController.cs          # User operations
│   └── UserRoleController.cs      # User-role assignments
├── Dtos/
│   └── [Data Transfer Objects]    # API models
├── Options/
│   └── KeycloakConfiguration.cs   # Keycloak config model
├── Services/
│   └── KeycloakService.cs         # Keycloak API client
├── Program.cs                     # Application entry point
├── appsettings.json              # Configuration
└── README.md                     # This file
```

---

## 🔒 Security Best Practices

- ✅ Always use **HTTPS** in production
- ✅ Store secrets in **Azure Key Vault** or **User Secrets**
- ✅ Implement **role-based authorization** on endpoints
- ✅ Validate JWT tokens on every request
- ✅ Use **strong client secrets** in Keycloak
- ✅ Enable **token expiration** and **refresh tokens**
- ✅ Implement **rate limiting** to prevent abuse


---

## 🙏 Acknowledgments

- [Keycloak](https://www.keycloak.org/) - Open Source Identity and Access Management
- [Scalar](https://scalar.com/) - Beautiful API documentation
- [Keycloak.AuthServices](https://github.com/NikiforovAll/keycloak-authorization-services-dotnet) - .NET integration library

---

**Made with ❤️ using .NET 10 and Keycloak**
