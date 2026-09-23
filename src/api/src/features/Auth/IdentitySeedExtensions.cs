

using Infrastructure.Identity;

namespace Authentication;

public static class IdentitySeedExtensions
{
    public static async Task SeedIdentityAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        await IdentitySeeder.SeedAsync(scope.ServiceProvider);
    }
}