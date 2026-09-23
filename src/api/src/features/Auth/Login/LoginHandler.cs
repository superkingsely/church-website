using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;


public sealed class LoginHandler
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public LoginHandler(
        UserManager<AppUser> userManager,
        JwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponse?> HandleAsync(
        LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return null;
        }

        var passwordValid = await _userManager.CheckPasswordAsync(
            user,
            request.Password);

        if (!passwordValid)
        {
            return null;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResponse(
            token,
            DateTime.UtcNow.AddMinutes(60));
    }
}