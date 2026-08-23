main
│
└── develop
      │
      ├── feature/project-setup
      ├── feature/api-configuration
      ├── feature/database-foundation
      ├── feature/authentication
      ├── feature/members
      ├── feature/departments
      ├── feature/workers
      ├── feature/events
      ├── feature/attendance
      ├── feature/giving
      ├── feature/sermons
      ├── feature/prayer-requests
      └── feature/announcements


      feature/project-setup
        │
        ▼
feature/api-configuration
        │
        ├── OpenAPI
        ├── Swagger
        ├── Scalar
        ├── CORS
        └── Global Exception
        │
        ▼
feature/authentication
        │
        ├── Identity
        ├── JWT
        ├── Roles
        ├── Authorization
        └── Current User
        │
        ▼
feature/database-foundation
        │
        ├── EF Core
        ├── SQLite
        ├── AppDbContext
        ├── Auditing
        └── DB configuration
        │
        ▼
feature/church-domain
        │
        ├── Member
        ├── Family
        ├── Department
        ├── Worker
        ├── Event
        └── etc.
        │
        ▼
feature/database-migrations
        │
        ├── EF configurations
        ├── Migration
        └── SQLite DB
        │
        ▼
feature/members
        │
        ├── Create
        ├── Read
        ├── Update
        └── Delete