


public static class ApplicationServiceExtention
{
    public static IServiceCollection ConfigureAppService(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        return builder.Services;
    }
    public static WebApplication ConfigureApp1Pipline(this WebApplication app)
    {

        app.MapControllers();
        app.MapCreateMember();

        return app;
    }
}