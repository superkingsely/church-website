Yes. This should be our **next authentication task**, and I agree we should do it properly at the architecture level instead of adding `CreatedBy = ...` manually to every feature.

# 🔐 Stage D — Authenticated Audit Fields

Our goal is:

```text
Admin logs in
      ↓
JWT contains Admin's identity
      ↓
Admin creates Member
      ↓
API knows the authenticated user
      ↓
CreatedBy = Admin's Identity ID
```

And later:

```text
Admin edits Member
      ↓
UpdatedBy = Admin's Identity ID
```

So eventually our `BaseEntity` audit fields will be automatically populated for **Members, Events, Sermons, Giving, Attendance, etc.**

---

## Task D1 — Create the current-user abstraction

Before touching `Member`, let's create a small abstraction that answers:

> **"Who is currently logged into this HTTP request?"**

We'll put this in a place that can be reused by every feature.

I recommend:

```text
src/api/
└── Authentication/
    └── CurrentUser/
        ├── ICurrentUser.cs
        └── CurrentUser.cs
```

The reason we're doing this instead of directly writing:

```csharp
HttpContext.User.FindFirstValue(...)
```

inside every endpoint is **coupling**.

We don't want:

```text
CreateMemberEndpoint
      ↓
HttpContext
      ↓
Claims
      ↓
JWT
```

and then repeat that everywhere.

Instead:

```text
CreateMemberEndpoint
      ↓
ICurrentUser
      ↓
Current authenticated user
```

That's much cleaner for our vertical-slice architecture.

---

# Step 1 — Create `ICurrentUser`

Create:

```text
src/api/Authentication/CurrentUser/ICurrentUser.cs
```

Put:

```csharp
namespace Authentication.CurrentUser;

public interface ICurrentUser
{
    string? UserId { get; }

    string? Email { get; }

    bool IsAuthenticated { get; }
}
```

### What does this mean?

We're defining a contract:

```text
ICurrentUser
   │
   ├── UserId
   ├── Email
   └── IsAuthenticated
```

So any part of our API can ask:

```csharp
_currentUser.UserId
```

without knowing anything about:

* JWT
* `HttpContext`
* claims
* ASP.NET authentication internals

That is the important architectural benefit.

---

# Step 2 — Implement `CurrentUser`

Create:

```text
src/api/Authentication/CurrentUser/CurrentUser.cs
```

Put:

```csharp
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Authentication.CurrentUser;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email =>
        _httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated =>
        _httpContextAccessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated
        ?? false;
}
```

Now look at what this class does.

Our JWT generator already creates:

```csharp
new(ClaimTypes.NameIdentifier, user.Id)
```

and:

```csharp
new(ClaimTypes.Email, user.Email ?? string.Empty)
```

Therefore, after JWT authentication succeeds:

```text
JWT
 │
 ├── NameIdentifier → Admin User ID
 │
 └── Email → admin@mfmijaiye.com
```

`CurrentUser` retrieves those claims.

---

# Step 3 — Register `IHttpContextAccessor`

In our authentication dependency injection setup, add:

```csharp
services.AddHttpContextAccessor();
```

So our `AddAuth()` will contain this alongside our existing authentication configuration.

The important part is:

```csharp
public static IServiceCollection AddAuth(
    this IServiceCollection services,
    IConfiguration configuration)
{
    services.AddHttpContextAccessor();

    // existing JWT configuration...

    return services;
}
```

Why?

Because `CurrentUser` needs access to:

```csharp
HttpContext
```

and:

```csharp
HttpContext.User
```

contains the authenticated user.

---

# Step 4 — Register our abstraction

Also add:

```csharp
services.AddScoped<ICurrentUser, CurrentUser>();
```

So the DI flow becomes:

```text
ICurrentUser
     ↓
CurrentUser
     ↓
IHttpContextAccessor
     ↓
HttpContext.User
     ↓
JWT Claims
```

Now our feature doesn't need to know any of that.

---

# 🧠 Now let's see what happens during your request

You log in:

```http
POST /api/auth/login
```

Identity finds:

```text
admin@mfmijaiye.com
```

and validates:

```text
Admin@12345
```

Then `JwtTokenGenerator` creates:

```text
JWT
```

containing:

```text
NameIdentifier = <Admin Identity User ID>
Email = admin@mfmijaiye.com
```

You then send:

```http
POST /api/members
Authorization: Bearer <JWT>
```

ASP.NET Core validates the JWT.

Then:

```csharp
HttpContext.User
```

becomes populated.

So:

```csharp
_currentUser.UserId
```

now returns the Admin's Identity ID.

And:

```csharp
_currentUser.Email
```

returns:

```text
admin@mfmijaiye.com
```

---

# 🚨 But don't change `CreateMemberEndpoint` yet

This is important.

We have only completed the **current-user foundation**.

We haven't yet implemented automatic audit tracking.

Our current:

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = "System";

    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
```

still has:

```csharp
CreatedBy = "System";
```

and that's okay **for this step**.

The next task will be to move the responsibility for populating:

```text
CreatedBy
UpdatedBy
CreatedAt
UpdatedAt
```

into our persistence layer.

That gives us:

```text
CreateMember
CreateEvent
CreateSermon
CreateGiving
CreateAttendance
...
```

all automatically receiving the authenticated user.

---

# 🏗️ The architecture we're building

Eventually we'll have:

```text
                 JWT
                  ↓
          ASP.NET Authentication
                  ↓
            HttpContext.User
                  ↓
             CurrentUser
                  ↓
             ICurrentUser
                  ↓
          EF Core / AppDbContext
                  ↓
          ┌───────┴────────┐
          ↓                ↓
      CreatedBy        UpdatedBy
          ↓                ↓
       Admin ID         Admin ID
```

So the feature itself can remain clean:

```text
Features/
└── Members/
    └── Create/
        ├── CreateMemberCommand.cs
        ├── CreateMemberHandler.cs
        └── CreateMemberEndpoint.cs
```

The handler doesn't have to know:

> "How does JWT work?"

It only deals with:

> "Create a Member."

---

## 🎯 Your first task

For now, **only do these three things**:

### 1. Create

```text
src/api/Authentication/CurrentUser/ICurrentUser.cs
```

with the interface above.

### 2. Create

```text
src/api/Authentication/CurrentUser/CurrentUser.cs
```

with the implementation above.

### 3. Add these two registrations to `AddAuth()`:

```csharp
services.AddHttpContextAccessor();

services.AddScoped<ICurrentUser, CurrentUser>();
```

Then run:

```bash
dotnet build
```

**Don't modify `BaseEntity` or `CreateMemberEndpoint` yet.**

Once the build succeeds, we'll do **Task D2: automatic audit-field population through `AppDbContext`**, which is the part that will finally make your database show the logged-in Admin instead of `"System"`.
