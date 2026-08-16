# Church Website

> Feature Branch: `feature/api-configuration`

## Overview

This milestone establishes the foundation of the ASP.NET Core Web API. The goal is to configure a production-ready API pipeline before implementing any business logic or authentication.

Rather than immediately creating controllers or authentication, this phase focuses on configuring the API infrastructure that every future feature will rely on.

---

# Objectives

- Configure API documentation
- Configure API middleware
- Configure CORS
- Configure request/response pipeline
- Configure exception handling
- Configure logging
- Prepare the API for authentication
- Prepare the API for database integration

---

# Technology Stack

## Backend

- ASP.NET Core Web API (.NET 10)

## Documentation

- OpenAPI
- Swagger UI
- Scalar API Reference

## Logging

- Serilog *(planned)*

---

# Project Structure

```
src
│
└── api
    │
    ├── Controllers
    ├── Extensions
    ├── Middlewares
    ├── Configurations
    ├── Properties
    ├── Program.cs
    └── appsettings.json
```

---

# Configuration Tasks

## API Documentation

Configure:

- OpenAPI
- Swagger UI
- Scalar

Purpose:

- Interactive API testing
- Endpoint documentation
- Request/Response inspection

---

## CORS

Configure Cross-Origin Resource Sharing (CORS) to allow communication between:

```
Blazor WebAssembly

↓

ASP.NET Core API
```

During development, the frontend and backend will run on different ports, making CORS configuration essential.

---

## Middleware

Configure middleware pipeline.

Current middleware:

- HTTPS Redirection
- Routing
- Exception Handling
- Swagger
- Scalar

Future middleware:

- Authentication
- Authorization
- Rate Limiting
- Request Logging

---

## Exception Handling

Create a centralized exception handling middleware.

Purpose:

- Prevent unhandled exceptions
- Return consistent error responses
- Improve debugging
- Improve client-side error handling

---

## Logging

Logging infrastructure will be introduced.

Planned features:

- Request logging
- Error logging
- Startup logging

Future implementation:

- Serilog
- File Logging
- Console Logging

---

## API Versioning *(Optional)*

Prepare the project to support API versioning.

Example:

```
/api/v1/members

/api/v2/members
```

Although versioning may not be immediately required, the API structure should allow it to be added without major refactoring.

---

# Current Progress

- [x] API project created
- [x] OpenAPI configured
- [ ] Swagger configured
- [ ] Scalar configured
- [ ] CORS configured
- [ ] Global exception middleware
- [ ] Logging configured
- [ ] API extensions created
- [ ] Health checks
- [ ] API versioning

---

# Deliverables

At the end of this milestone the API should provide:

- Interactive API documentation
- Clean middleware pipeline
- Global exception handling
- Configured CORS
- Ready for authentication
- Ready for Entity Framework Core

---

# Branch Strategy

Current branch:

```
feature/api-configuration
```

Merge flow:

```
feature/api-configuration

↓

develop

↓

main
```

---

# Next Milestone

```
feature/database-foundation
```

Upcoming tasks:

- PostgreSQL
- Entity Framework Core
- DbContext
- Base Entity
- Audit Fields
- Initial Migration
- Database Connection
- Seed Data

---

# Notes

No business entities or application features are implemented during this milestone.

The objective is to establish a stable and maintainable API foundation that subsequent milestones can build upon.

---

# Author

**Onwumelu Chijioke**

GitHub:
https://github.com/superkingsely

Portfolio:
https://chijioke-portfolio-web.vercel.app

LinkedIn:
https://www.linkedin.com/in/onwumelu-chijioke

---

# License

This project is being developed as a real-world Church Management System using ASP.NET Core, Blazor WebAssembly, MudBlazor, Entity Framework Core, and PostgreSQL.