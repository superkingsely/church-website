I like what you've started. Splitting `Program.cs` into extension methods is exactly how I structure my APIs.

However, I would **not** put everything into one `Startup` class. As the project grows, that file will become 500+ lines long.

Instead, let's organize it from the beginning.

---

# Folder Structure

```text
api
│
├── Controllers
│
├── Extensions
│   ├── ApplicationServiceExtensions.cs
│   ├── MiddlewareExtensions.cs
│   ├── SwaggerExtensions.cs
│   ├── CorsExtensions.cs
│   ├── AuthenticationExtensions.cs
│   ├── AuthorizationExtensions.cs
│   ├── DatabaseExtensions.cs
│   └── HealthCheckExtensions.cs
│
├── Middlewares
│   ├── GlobalExceptionMiddleware.cs
│   └── RequestLoggingMiddleware.cs
│
├── Configurations
│
├── Program.cs
│
└── appsettings.json
```

Notice that every concern has its own extension.

---

# Program.cs

I would make it very small.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApplicationServices();
builder.ConfigureSwagger();
builder.ConfigureCors();

var app = builder.Build();

app.ConfigureMiddleware();
app.ConfigureSwaggerUI();

app.Run();
```

You should be able to understand Program.cs in less than 30 seconds.

---

# ApplicationServiceExtensions.cs

```csharp
namespace Api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection ConfigureApplicationServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        return builder.Services;
    }
}
```

Later we'll add

* AutoMapper
* FluentValidation
* MediatR
* Services

here.

---

# SwaggerExtensions.cs

```csharp
namespace Api.Extensions;

public static class SwaggerExtensions
{
    public static WebApplicationBuilder ConfigureSwagger(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();

        return builder;
    }

    public static WebApplication ConfigureSwaggerUI(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        return app;
    }
}
```

Later we'll add

* Swagger
* Scalar
* JWT Security Definition

inside here.

---

# CorsExtensions.cs

For now

```csharp
namespace Api.Extensions;

public static class CorsExtensions
{
    public static WebApplicationBuilder ConfigureCors(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return builder;
    }
}
```

Later we'll restrict this.

---

# MiddlewareExtensions.cs

```csharp
namespace Api.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication ConfigureMiddleware(
        this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.MapControllers();

        return app;
    }
}
```

---

# What we'll add in future milestones

### API Configuration

```text
ApplicationServiceExtensions

SwaggerExtensions

CorsExtensions

MiddlewareExtensions
```

---

### Database Foundation

We'll create

```text
DatabaseExtensions
```

Containing

```csharp
builder.Services.AddDbContext<ChurchDbContext>();
```

---

### Identity Foundation

We'll create

```text
IdentityExtensions
```

Containing

```csharp
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>();
```

---

### Authentication

We'll create

```text
AuthenticationExtensions
```

Containing

```csharp
builder.Services
    .AddAuthentication(...)
    .AddJwtBearer(...);
```

---

### Authorization

We'll create

```text
AuthorizationExtensions
```

Containing

```csharp
builder.Services.AddAuthorization(...);
```

---

### Health Checks

We'll create

```text
HealthCheckExtensions
```

Containing

```csharp
builder.Services.AddHealthChecks();
```

---

### Logging

We'll create

```text
LoggingExtensions
```

Containing

```csharp
builder.Host.UseSerilog();
```

---

## One thing I would change in your current code

Instead of:

```csharp
public static void ConfigureServices(this WebApplicationBuilder builder)
```

I'd return the builder (or `IServiceCollection` where appropriate), because it makes the extensions composable and testable.

For example:

```csharp
public static WebApplicationBuilder ConfigureSwagger(
    this WebApplicationBuilder builder)
{
    builder.Services.AddOpenApi();

    return builder;
}
```

Then your `Program.cs` becomes a clear sequence of configuration steps.

---

## My recommendation for this milestone

Let's keep **`feature/api-configuration`** focused on these six pieces:

* ✅ `ApplicationServiceExtensions`
* ✅ `SwaggerExtensions` (OpenAPI + Swagger + Scalar)
* ✅ `CorsExtensions`
* ✅ `MiddlewareExtensions`
* ✅ `GlobalExceptionMiddleware`
* ✅ `API versioning` (optional, but we can decide together whether to include it now)

That gives us a solid, production-ready API foundation before we move on to the database and identity.
