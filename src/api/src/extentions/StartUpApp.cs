



public static class StartupApp
{
    public static WebApplicationBuilder Configurservices(this WebApplicationBuilder builder)
    {

        builder.UiServices();
        builder.ConfigureAppService();
        builder.ConfigCorsServices();
        builder.Services.AddDatabase(builder.Configuration);
        return builder;
    }

    public static WebApplication ConfigurePiplines(this WebApplication app)
    {
        app.UiPipline();
        app.ConfigureMiddlewarePipline();
        // app.UseHttpsRedirection();
        app.configCorsPipline();
        app.ConfigureApp1Pipline();
        return app;
    }
}

// public static class Startup
// {
//     extension(WebApplicationBuilder builder)
//     {
//         public WebApplicationBuilder Configurservice(this WebApplicationBuilder builder)
//         {

//             builder.UiServices();
//             builder.ConfigureAppService();
//             builder.ConfigCorsServices();
//             return builder;
//         }
//     }

//     public static WebApplication ConfigurePipline(this WebApplication app)
//     {
//         app.UiPipline();
//         app.ConfigureMiddlewarePipline();
//         // app.UseHttpsRedirection();
//         app.configCorsPipline();
//         app.ConfigureApp1Pipline();
//         return app;
//     }
// }