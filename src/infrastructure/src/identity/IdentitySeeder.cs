

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services
            .GetRequiredService<UserManager<AppUser>>();

        const string email = "admin@mfmijaiye.com";
        const string password = "Admin@12345";

        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return;
        }

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(
            user,
            password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(error => error.Description));

            throw new InvalidOperationException(
                $"Failed to create development user: {errors}");
        }
    }
}