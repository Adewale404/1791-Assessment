# 1791-Assessment
LoadExpressApi is a well-architected RESTful API developed using ASP.NET Core with a focus on clean, scalable, and maintainable code architecture. It follows the Clean Architecture principles by separating concerns across core, infrastructure, and presentation layers. 

# LoadExpressApi

A robust and well-structured RESTful API built using ASP.NET Core and Clean Architecture and Postgres DB as the Database.

## 🧱 Architecture Overview

This project follows the **Clean Architecture** approach:


---

## 🛠️ Technologies Used

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **Identity API with Roles**
- **Serilog for Logging**
- **Swagger / OpenAPI**
- **JWT Authentication**
- **Generic Repository Pattern**
- **Soft Delete & Audit Trails**
- **Clean Architecture Pattern**
- **LINQ, Async/Await, Dependency Injection**

---

## ✅ Key Features

- 🔹 Full CRUD for `User` entity using Identity
- 🔹 Descriptive and clean Swagger documentation
- 🔹 Centralized error handling middleware
- 🔹 Consistent response models with `Result<T>`
- 🔹 Role-based authorization (`Admin`, `Customer`, `Agent`)
- 🔹 Structured logs via Serilog (supports console, file, Elasticsearch)
- 🔹 Separation of concerns via Clean Architecture
- 🔹 Generic `IRepositoryAsync<T>` implementation

---

## 📂 Modules Breakdown

### 🔸 Domain Layer
- `User`, `Role`, `IAuditableEntity`, `ISoftDelete`
- Role constants

### 🔸 Application Layer
- `UserRequest`, `UserResponse`, `IUserService`
- `Result<T>`, `ErrorResult<T>`

### 🔸 Infrastructure Layer
- `RepositoryAsync<T>`
- `ApplicationDbContext`
- Logging via Serilog (with ElasticSearch/File options)

### 🔸 Presentation Layer
- `UserController` with routes like `/api/user/GetUsers`
- Swagger integration
- `Program.cs` and `ServiceCollection.cs` for bootstrapping

---

## 🔐 Authentication & Authorization

- JWT Bearer token authentication
- ASP.NET Core Identity with role management
- Endpoints are secured and return 401/403 as appropriate

---

## 📘 API Endpoints (Sample)

| Method | Route                          | Description             |
|--------|--------------------------------|-------------------------|
| GET    | `/api/user/GetUsers`           | Get all users           |
| GET    | `/api/user/GetUser/{email}`    | Get a user by email     |
| POST   | `/api/user/CreateUser`         | Create a new user       |
| PUT    | `/api/user/UpdateUser/{email}` | Update user details     |
| DELETE | `/api/user/DeleteUser/{email}` | Delete a user (soft)    |

