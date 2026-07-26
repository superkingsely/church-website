# Church Website

> A modern Church Management System (CMS) built with ASP.NET Core Web API, Blazor WebAssembly, MudBlazor, Entity Framework Core, and PostgreSQL.

## Feature Branch

```
feature/project-setup
```

## Overview

This branch establishes the initial project structure and development environment for the Church Website. The focus of this milestone is to create a clean, scalable architecture that will support future features such as authentication, membership management, events, sermons, departments, attendance, giving, and administration.

---

# Technology Stack

## Frontend

- Blazor WebAssembly (.NET 10)
- MudBlazor

## Backend

- ASP.NET Core Web API (.NET 10)

## Database

- PostgreSQL (to be configured)

## ORM

- Entity Framework Core (to be configured)

## Version Control

- Git
- GitHub

---

# Solution Structure

```
church-website
│
├── src
│   ├── api
│   ├── application
│   ├── client
│   ├── domain
│   ├── infrastructure
│   └── shared
│
├── test
│
├── .gitignore
│
├── README.md
│
└── church-website.slnx
```

---

# Project Description

## api

ASP.NET Core Web API.

Responsibilities:

- REST API
- Authentication
- Authorization
- Controllers
- Middleware
- Dependency Injection

---

## client

Blazor WebAssembly application using MudBlazor.

Responsibilities:

- User Interface
- Routing
- Components
- Pages
- HTTP communication with the API

---

## domain

Contains the core business model.

Responsibilities:

- Entities
- Enums
- Interfaces
- Value Objects
- Domain Exceptions

No external dependencies should exist in this project.

---

## application

Contains the application's business logic.

Responsibilities:

- DTOs
- Commands
- Queries
- Services
- Validators
- Interfaces

---

## infrastructure

Contains implementation details.

Responsibilities:

- Entity Framework Core
- Database Context
- Repositories
- Identity
- External Services

---

## shared

Contains objects shared between the Client and API.

Examples:

- DTOs
- Enums
- Constants
- Shared Models

---

## test

Contains automated tests.

Future additions:

- Unit Tests
- Integration Tests

---

# MudBlazor Configuration

The frontend uses MudBlazor for UI components.

Bootstrap was intentionally removed in favor of MudBlazor to avoid duplicate UI frameworks and maintain a consistent design system.

MudBlazor services were registered in `Program.cs`.

MudBlazor CSS and JavaScript assets were added to the client application.

---

# Git Branch Strategy

Permanent branches:

```
main
develop
```

Feature branches:

```
feature/project-setup
feature/authentication
feature/public-website
feature/member-management
feature/departments
feature/workers
feature/events
feature/sermons
feature/gallery
feature/prayer-requests
feature/giving
feature/attendance
feature/reports
feature/deployment
```

---

# Current Progress

- [x] Repository initialized
- [x] Git configured
- [x] Solution (.slnx) created
- [x] API project created
- [x] Client project created
- [x] Domain project created
- [x] Application project created
- [x] Infrastructure project created
- [x] Shared project created
- [x] Test project created
- [x] MudBlazor installed
- [x] Bootstrap removed
- [x] Git ignore configured

---

# Upcoming Work

The next milestone will implement authentication.

Planned features include:

- ASP.NET Core Identity
- JWT Authentication
- Refresh Tokens
- Role Management
- User Registration
- User Login
- Authorization Policies

---

# Development Notes

- Architecture follows Clean Architecture principles.
- Frontend and backend are developed as separate applications.
- Communication between the client and API will occur over HTTP.
- Entity Framework Core and PostgreSQL will be configured in the next milestone.
- Cloud deployment will be performed incrementally as features are completed.

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

This project is being developed as a real-world Church Management System and portfolio project.