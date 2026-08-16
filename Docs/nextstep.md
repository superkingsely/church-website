These are exactly the kinds of questions we should be asking now. A little planning here will save us from reorganizing the project later.

## My recommendation is **No**—authentication should **not** be our immediate next milestone.

There are a few pieces of foundation work we should complete first.

---

# Roadmap

## ✅ Milestone 1 (Completed)

**Branch**

```text
feature/project-setup
```

Completed:

* Solution structure
* Clean Architecture projects
* MudBlazor client
* Web API
* Git
* README
* `.gitignore`

Merge into `develop` after reviewing.

---

# Milestone 2

## API Configuration

**Branch**

```text
feature/api-configuration
```

I recommend we do this next.

### What belongs here?

* Configure Swagger/OpenAPI
* Configure Scalar
* Configure CORS
* Configure API Versioning (optional but recommended)
* Global Exception Middleware
* Serilog logging
* Health Checks
* API Extensions folder
* ProblemDetails responses
* Base API configuration

### Why before Authentication?

Because authentication is part of the API.

I'd rather have the API fully configured before adding Identity and JWT.

---

# Swagger or Scalar?

My recommendation is:

**Use both.**

### Swagger

Use Swagger because almost every .NET developer knows it.

```
/swagger
```

---

### Scalar

Also install Scalar.

```
/scalar
```

Scalar has a much cleaner UI and is becoming increasingly popular.

During development we'll use both. If you later decide to expose API documentation publicly, you can keep Scalar and disable Swagger if you want.

---

# Milestone 3

## Database Foundation

**Branch**

```text
feature/database-foundation
```

This milestone will include:

* PostgreSQL
* EF Core
* DbContext
* Initial Migration
* BaseEntity
* AuditableEntity
* Seed data
* Configurations
* Connection strings

Notice...

No business models yet.

Just the database infrastructure.

---

# Milestone 4

## Domain Models

**Branch**

```text
feature/domain-models
```

Now we create things like:

```
ApplicationUser

Member

Department

WorkerDepartment

Branch

Role

Permission

Announcement

Event

Sermon
```

Why separate it?

Because models evolve.

Keeping them in their own milestone gives us a clean Git history.

---

# Milestone 5

## Authentication

**Branch**

```text
feature/authentication
```

This is where authentication really belongs.

We'll implement:

* ASP.NET Core Identity
* JWT
* Refresh Tokens
* Login
* Register
* Forgot Password
* Email Confirmation
* Roles
* Permissions
* Authorization Policies

Authentication will depend on the models from Milestone 4, so it's cleaner to do it afterward.

---

# Updated Roadmap

```text
feature/project-setup
        │
        ▼
feature/api-configuration
        │
        ▼
feature/database-foundation
        │
        ▼
feature/identity-foundation
        │
        ▼
feature/domain-models
        │
        ▼
feature/authentication
        │
        ▼
feature/member-management
        │
        ▼
feature/visitor-management
        │
        ▼
feature/departments
        │
        ▼
feature/workers
        │
        ▼
feature/events
        │
        ▼
feature/sermons
        │
        ▼
feature/gallery
        │
        ▼
feature/prayer-requests
        │
        ▼
feature/giving
        │
        ▼
feature/attendance
        │
        ▼
feature/reports
        │
        ▼
feature/deployment
```

I like this order because each milestone builds on the previous one.

---

# README for Authentication

Don't create it yet.

Create it **when we start**:

```
feature/authentication
```

It will document:

* ASP.NET Identity
* JWT
* Refresh Tokens
* Authorization
* Roles
* Claims
* Login Flow
* Registration Flow
* Password Reset
* Email Verification
* Authentication Middleware

That README will be much more meaningful once the work actually exists.

---

# One thing I also want to add

Since this is a **real church website**, I'd like us to create a `docs` folder at the root from the beginning.

```
church-website
│
├── docs
│   ├── architecture.md
│   ├── roadmap.md
│   ├── erd.md
│   ├── api-endpoints.md
│   ├── deployment.md
│   ├── branching-strategy.md
│   └── coding-standards.md
│
├── src
├── test
└── README.md
```

This keeps long-form documentation separate from the main `README.md`. The root README stays concise, while the `docs` folder becomes the project's technical documentation. As the project grows, having architecture, ERDs, deployment notes, and coding standards in one place will make it much easier to maintain and onboard anyone else who contributes.
