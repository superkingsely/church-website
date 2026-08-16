

public static class CorsExtensions
{
    public static WebApplicationBuilder ConfigCorsServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
        }));
        return builder;
    }

    public static WebApplication configCorsPipline(this WebApplication app)
    {
        app.UseCors("CorsPolicy");
        return app;
    }
}