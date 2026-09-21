

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity;

public static class JwtDependencyInjection
{
    public static IServiceCollection AddAuth(
        this IServiceCollection services)
    {
        services
            .AddAuthentication();
        services.AddAuthorization();
        
           

        return services;
    }
}