

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/api/auth/login",
            async (
                LoginRequest request,
                 [FromServices] LoginHandler handler) =>
            {
                var result = await handler.HandleAsync(request);

                if (result is null)
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(result);
            });

        return endpoints;
    }
}