

using Api.Middlewares;

public static class MiddlewareExtensions
{

    public static WebApplication ConfigureMiddlewarePipline(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        return app;
    }
}