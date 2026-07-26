

public static class Startup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        
    }
    public static void ConfigureApp(this WebApplication app)
    {
        app.MapControllers();
    }
}