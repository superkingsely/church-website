using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class JwtDependencyInjection
{
    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["Jwt:SecretKey"]!
                        )
                    ),

                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();
        // -----------------------------
        // Bind appsettings.json "Jwt" section to JwtOptions
        // IOptions<JwtOptions> originate from here
        services.Configure<JwtOptions>(
    configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<JwtTokenGenerator>();
        services.AddScoped<LoginHandler>();

        return services;
    }
}