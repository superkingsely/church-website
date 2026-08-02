


public static class GlobalServiceExtention
{
    public static WebApplicationBuilder Configurservice(this WebApplicationBuilder builder)
    {

        builder.UiServices();
        builder.ConfigureAppService();
        builder.ConfigCorsServices();
        return builder;
    }
    public static WebApplication ConfigureAppPipline(this WebApplication app)
    {
        app.UiPipline();
        // app.UseHttpsRedirection();
        app.configCorsPipline();
        app.ConfigureApp1Pipline();
        return app;
    }
}