

Microsoft.AspNetCore.Identity.EntityFrameworkCore______infrastructure
Microsoft.AspNetCore.Authentication.JwtBearer_________api
because your API is responsible for configuring the ASP.NET Core authentication pipeline:

builder.Services
    .AddAuthentication(...)
    .AddJwtBearer(...);

builder.Services.AddAuthorization();

The API then receives:

Authorization: Bearer eyJ...

and validates it.