

Yes — **Stage C is the Swagger/Scalar JWT UI security stage**. And because you're on **.NET 10 + Swashbuckle 10.2.3**, the old code you commented out is exactly where the red squiggles came from: Swashbuckle v10 changed the OpenAPI types and `AddSecurityRequirement` API. ([GitHub][1])

The good news: we can now configure it correctly for your versions.

## Stage C — Step 1: Configure the JWT security scheme

Keep your existing `ApiUiExtensions.cs`, but replace the commented `ConfigureJwt` with this:

```csharp id="c7m2n4"
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class ApiUiExtensions
{
    public static WebApplicationBuilder UiServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MFM Ijaiye web-api",
                Version = "v1",
                Description =
                    "REST API for the Church Website Management System.",
                Contact = new OpenApiContact
                {
                    Name = "Onwumelu Chijioke \n 0802-9696-733",
                }
            });

            ConfigureJwt(opt);
        });

        return builder;
    }

    private static void ConfigureJwt(
        SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description =
                    "Enter your JWT token. Example: Bearer {token}",

                In = ParameterLocation.Header,

                Type = SecuritySchemeType.Http,

                Scheme = "bearer",

                BearerFormat = "JWT"
            });

        options.AddSecurityRequirement(
            document => new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecuritySchemeReference(
                        "Bearer",
                        document)
                ] = []
            });
    }

    public static WebApplication UiPipline(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "Church Website API v1");

                options.RoutePrefix = "swagger";

                options.DocumentTitle =
                    "Church Website API";
            });

            app.MapScalarApiReference(
                "/scalar",
                options =>
                {
                    options.Title =
                        "Church Website API";
                });
        }

        return app;
    }
}
```

### The important change from your old code

Your old code had:

```csharp
Reference = new OpenApiReference
{
    Id = "Bearer",
    Type = ReferenceType.SecurityScheme
}
```

That is the **old style**.

With Swashbuckle 10 / Microsoft.OpenApi 2.x, you use:

```csharp
new OpenApiSecuritySchemeReference(
    "Bearer",
    document)
```

And `AddSecurityRequirement` now takes a function:

```csharp
options.AddSecurityRequirement(
    document => new OpenApiSecurityRequirement
    {
        [
            new OpenApiSecuritySchemeReference(
                "Bearer",
                document)
        ] = []
    });
```

This is the v10 pattern documented by Swashbuckle. ([GitHub][1])

---

# What this actually does

This:

```csharp
options.AddSecurityDefinition("Bearer", ...)
```

creates the JWT security scheme in your OpenAPI document.

Conceptually:

```text
OpenAPI
   │
   └── Security Schemes
          │
          └── Bearer
                │
                ├── Type: HTTP
                ├── Scheme: bearer
                └── Format: JWT
```

Then:

```csharp
options.AddSecurityRequirement(...)
```

says:

> Operations using this API can require the `Bearer` security scheme.

That security metadata is what allows Swagger UI and Scalar to understand that JWT authentication exists. ([GitHub][2])

---

# But there's one VERY important issue

Right now, the security requirement above is **global**.

That means Swagger's OpenAPI document will describe the Bearer requirement globally.

But your login endpoint:

```text
POST /api/auth/login
```

is supposed to be public.

You don't want Swagger thinking:

```text
POST /api/auth/login 🔒
```

because that endpoint is precisely where you **obtain** the token.

And your Create Member endpoint:

```text
POST /api/members 🔒
```

should be protected.

So eventually we want the OpenAPI document to represent:

```text
POST /api/auth/login
    Public

POST /api/members
    🔒 Bearer required
```

That's why I don't want you to blindly put a global security requirement in and move on.

---

# What about Scalar?

Here's the nice part.

**Scalar reads the security schemes from your OpenAPI document.** It doesn't need us to create a completely separate JWT definition for Scalar. Scalar's own documentation says its authentication UI is based on the security schemes already defined in the OpenAPI specification. ([Scalar][3])

So our architecture is:

```text
                  JWT CONFIGURATION
                         │
                         ▼
                  ASP.NET Core Auth
                         │
                         │
                ┌────────┴────────┐
                │                 │
                ▼                 ▼
            Swagger           Scalar
                │                 │
                │                 │
                └────────┬────────┘
                         │
                         ▼
                 OpenAPI Document
                         │
                         ▼
                 Bearer Security
```

That's why **we configure the security scheme once in Swagger/OpenAPI**, and Scalar can consume it.

---

# What you should see after this

Run:

```bash
dotnet build
```

then:

```bash
dotnet run
```

Open:

```text
http://localhost:5001/swagger
```

You should see an **Authorize** button with a lock/key-style security UI.

Click it and you should get something along the lines of:

```text
Available authorizations

Bearer (HTTP, Bearer)

Value:
[____________________________]

[Authorize]
[Close]
```

You enter:

```text
Bearer eyJhbGciOiJIUzI1NiIs...
```

and Swagger can use that authorization when making requests.

---

## One thing I want you to do first

Don't modify your endpoint security yet.

Let's make **Stage C Step 1** only:

```text
Configure JWT security definition
             ↓
Configure Swagger security requirement
             ↓
Build
             ↓
Open /swagger
             ↓
Confirm 🔒 / Authorize appears
```

Then **Stage C Step 2** will be making the security requirement apply correctly to your actual `.RequireAuthorization()` endpoints, so that **login remains public while CreateMember shows as protected**.

That second part matters for your Vertical Slice API because the documentation should accurately reflect the security metadata of each endpoint, rather than simply putting a lock on everything.

[1]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/docs/migrating-to-v10.md?utm_source=chatgpt.com "Swashbuckle.AspNetCore/docs/migrating-to-v10.md at master · domaindrivendev/Swashbuckle.AspNetCore · GitHub"
[2]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/docs/configure-and-customize-swaggergen.md?utm_source=chatgpt.com "Swashbuckle.AspNetCore/docs/configure-and-customize-swaggergen.md at master · domaindrivendev/Swashbuckle.AspNetCore · GitHub"
[3]: https://scalar.com/products/api-references/integrations/aspnetcore/integration?utm_source=chatgpt.com "API Reference for .NET ASP.NET Core"


#####################################################################################
Exactly — and **that tells us our current Stage C configuration is too broad**.

Your Swagger **Authorize** button is correct, but if `/api/auth/login` is showing the 🔒 security indicator too, we should fix the OpenAPI metadata so Swagger reflects your actual authorization rules.

The desired result is:

```text
POST /api/auth/login       🔓 Public
POST /api/members          🔒 Protected
GET  /api/Test             depends on endpoint
GET  /api/Test/string      depends on endpoint
POST /api/Test/new-member  depends on endpoint
```

### Why login got locked

We currently have:

```csharp
options.AddSecurityRequirement(
    document => new OpenApiSecurityRequirement
    {
        [
            new OpenApiSecuritySchemeReference(
                "Bearer",
                document)
        ] = []
    });
```

That is effectively telling the OpenAPI document:

> "Bearer authentication is required."

Swagger therefore doesn't know that your login endpoint is an exception.

But your actual ASP.NET Core endpoint doesn't require authentication:

```csharp
endpoints.MapPost(
    "/api/auth/login",
    ...
);
```

while your member endpoint has:

```csharp
.RequireAuthorization();
```

So **runtime security and Swagger documentation are currently out of sync.**

---

# Stage C — Step 2

The clean approach for your project is to let `.RequireAuthorization()` drive the OpenAPI security metadata.

Since you're using **Minimal APIs**, we can add an OpenAPI operation transformer that checks the endpoint's authorization metadata and only adds the Bearer security requirement to protected endpoints.

First, **remove this global requirement** from `ConfigureJwt`:

```csharp
options.AddSecurityRequirement(
    document => new OpenApiSecurityRequirement
    {
        [
            new OpenApiSecuritySchemeReference(
                "Bearer",
                document)
        ] = []
    });
```

Keep the security definition:

```csharp
options.AddSecurityDefinition(
    "Bearer",
    new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description =
            "Enter your JWT token. Example: Bearer {token}",

        In = ParameterLocation.Header,

        Type = SecuritySchemeType.Http,

        Scheme = "bearer",

        BearerFormat = "JWT"
    });
```

Then we'll configure **operation-level security**.

But before I give you that code, there is one important distinction:

### Your Swagger document currently says:

```text
OAS 3.0
```

while your project also has:

```csharp
builder.Services.AddOpenApi();
```

That is the **Microsoft ASP.NET Core OpenAPI pipeline**, whereas:

```csharp
builder.Services.AddSwaggerGen(...)
```

is the **Swashbuckle pipeline**.

You currently have both:

```text
AddOpenApi()
     ↓
MapOpenApi()

AND

AddSwaggerGen()
     ↓
UseSwagger()
     ↓
Swagger UI
```

That's why I want to handle this carefully rather than mixing the two OpenAPI systems.

### For your immediate goal

Your **runtime authorization is already correct** if your endpoints are:

```csharp
.MapPost("/api/auth/login", ...)
```

and:

```csharp
.MapPost("/api/members", ...)
.RequireAuthorization();
```

The next task is simply to make Swagger's documentation match that runtime behavior:

```text
             ASP.NET Core endpoint
                     │
                     ▼
              RequireAuthorization?
                  /          \
                NO            YES
                │              │
                ▼              ▼
             🔓 Login       🔒 Members
```

So **don't add `[Authorize]` to Login** and don't remove your JWT setup.

Your login endpoint is supposed to remain public.

If you want, the next code change should be **the exact Swagger configuration for Swashbuckle 10.2.3 that makes only `.RequireAuthorization()` endpoints show 🔒**, while keeping the global **Authorize** button available.

#####################################################################################
Yes. I checked the current Swashbuckle v10 documentation before giving you the code. Swashbuckle v10 uses the newer `OpenApiSecuritySchemeReference` / `Func<OpenApiDocument,...>` APIs, and operation-level security is the correct mechanism when only some endpoints require authentication. ([GitHub][1])

For **your Minimal API + `.RequireAuthorization()`** setup, let's do this cleanly.

## Stage C — Step 2: Lock only protected endpoints

The goal is:

```text
Swagger
────────────────────────────
Authorize 🔐       ← global button

POST /api/auth/login
                 ← NO LOCK 🔓

POST /api/members
                 ← LOCK 🔒
```

ASP.NET Core's Minimal APIs support `.RequireAuthorization()` as endpoint authorization metadata. ([Microsoft Learn][2])

### 1. Create this operation filter

Create:

```text
src/api/
└── Authentication/
    └── Swagger/
        └── JwtSecurityOperationFilter.cs
```

Put this inside:

```csharp id="58321"
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Authentication.Swagger;

public sealed class JwtSecurityOperationFilter : IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        var requiresAuthorization =
            context.ApiDescription
                .ActionDescriptor
                .EndpointMetadata
                .OfType<IAuthorizeData>()
                .Any();

        if (!requiresAuthorization)
        {
            return;
        }

        operation.Security =
        [
            new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecuritySchemeReference(
                        "Bearer",
                        context.Document)
                ] = []
            }
        ];
    }
}
```

### What this filter does

This is the important line:

```csharp
var requiresAuthorization =
    context.ApiDescription
        .ActionDescriptor
        .EndpointMetadata
        .OfType<IAuthorizeData>()
        .Any();
```

We're asking:

> "Does this endpoint have authorization metadata?"

For your member endpoint:

```csharp
.RequireAuthorization();
```

ASP.NET Core attaches authorization metadata to that endpoint.

So the filter sees:

```text
/api/members
      ↓
IAuthorizeData found
      ↓
YES
      ↓
Add Bearer security requirement
      ↓
Swagger displays 🔒
```

For login:

```text
/api/auth/login
      ↓
IAuthorizeData found?
      ↓
NO
      ↓
return
      ↓
No security requirement
      ↓
Swagger displays 🔓
```

This follows Swashbuckle's documented approach of applying security requirements at the **operation level** when authentication applies only to certain operations. ([GitHub][1])

---

# 2. Update `ApiUiExtensions`

Now your `ConfigureJwt()` should **NOT** contain the global `AddSecurityRequirement`.

Use this:

```csharp id="81427"
private static void ConfigureJwt(
    SwaggerGenOptions options)
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Description =
                "Enter your JWT token. Example: Bearer {token}",

            In = ParameterLocation.Header,

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT"
        });

    options.OperationFilter<JwtSecurityOperationFilter>();
}
```

Notice that we have:

```csharp
options.OperationFilter<JwtSecurityOperationFilter>();
```

but **we removed**:

```csharp
options.AddSecurityRequirement(...)
```

That global requirement was what caused your login endpoint to get the lock.

---

# 3. Your complete `ApiUiExtensions.cs`

To make this easy, your current file should now look like:

```csharp id="27164"
using Authentication.Swagger;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class ApiUiExtensions
{
    public static WebApplicationBuilder UiServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MFM Ijaiye web-api",
                Version = "v1",
                Description =
                    "REST API for the Church Website Management System.",

                Contact = new OpenApiContact
                {
                    Name = "Onwumelu Chijioke \n 0802-9696-733",
                }
            });

            ConfigureJwt(opt);
        });

        return builder;
    }

    private static void ConfigureJwt(
        SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",

                Description =
                    "Enter your JWT token. Example: Bearer {token}",

                In = ParameterLocation.Header,

                Type = SecuritySchemeType.Http,

                Scheme = "bearer",

                BearerFormat = "JWT"
            });

        options.OperationFilter<JwtSecurityOperationFilter>();
    }

    public static WebApplication UiPipline(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "Church Website API v1");

                options.RoutePrefix = "swagger";

                options.DocumentTitle =
                    "Church Website API";
            });

            app.MapScalarApiReference(
                "/scalar",
                options =>
                {
                    options.Title =
                        "Church Website API";
                });
        }

        return app;
    }
}
```

---

# 4. Make sure your Member endpoint actually has authorization

Your endpoint should contain:

```csharp id="1n5v6k"
endpoints.MapPost(
    "/api/members",
    async (
        CreateMemberCommand request,
        AppDbContext dbContext,
        CancellationToken cancellationToken) =>
    {
        // ...
    })
    .RequireAuthorization();
```

This is what actually protects the endpoint at runtime. The Swagger filter merely **documents that protection**. ASP.NET Core itself uses `.RequireAuthorization()` to protect Minimal API routes. ([Microsoft Learn][2])

So keep these as two separate concepts:

```text
Runtime security
────────────────────────
.RequireAuthorization()
        ↓
ASP.NET Core blocks unauthorized requests


Swagger documentation
────────────────────────
JwtSecurityOperationFilter
        ↓
Swagger displays 🔒
```

The Swagger lock **doesn't protect your API**. Your ASP.NET authorization middleware does.

---

# 5. Build

Run:

```bash
dotnet build
```

Then:

```bash
dotnet run
```

Open:

```text
http://localhost:5001/swagger
```

You should now see:

```text
MFM Ijaiye web-api

Authorize 🔐


Authentication

POST /api/auth/login
                     ← 🔓


apis

POST /api/members
                  ← 🔒
```

And the important part:

### The global Authorize button remains

You can click:

```text
Authorize 🔐
```

enter:

```text
Bearer eyJhbGciOiJIUzI1Ni...
```

and Swagger will send the token when executing protected operations.

### Login remains public

You can still execute:

```text
POST /api/auth/login
```

without an existing JWT.

### Members is protected

You need the JWT to execute:

```text
POST /api/members
```

---

## And Scalar?

Scalar consumes the OpenAPI security metadata, so the same Bearer scheme/operation security information can be surfaced there as well. The important part is that we are now putting the security requirement **on the protected operation instead of globally on the whole document**.

So the architecture is now:

```text
                    ASP.NET CORE
                         │
             .RequireAuthorization()
                         │
                         ▼
                 Endpoint Metadata
                         │
                         ▼
              JwtSecurityOperationFilter
                         │
                         ▼
                  OpenAPI document
                    /           \
                   /             \
                  ▼               ▼
             Swagger            Scalar
                │                  │
                🔒                 🔒
          /api/members       /api/members

                🔓                 🔓
          /api/auth/login    /api/auth/login
```

This is a much better fit for your **Vertical Slice Minimal API** architecture because each slice declares its own authorization requirement, and the documentation reflects that requirement instead of maintaining a separate hard-coded list of protected routes. ([GitHub][1])

**Do this first and show me what Swagger displays after the rebuild.** Then we can verify the Scalar behavior separately rather than changing several things at once.

[1]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/docs/configure-and-customize-swaggergen.md?utm_source=chatgpt.com "Swashbuckle.AspNetCore/docs/configure-and-customize-swaggergen.md at master · domaindrivendev/Swashbuckle.AspNetCore · GitHub"
[2]: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-10.0&utm_source=chatgpt.com "Minimal APIs quick reference | Microsoft Learn"
