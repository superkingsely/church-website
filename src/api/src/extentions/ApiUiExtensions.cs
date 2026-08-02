

using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class ApiUiExtensions
{
    public static WebApplicationBuilder UiServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {

            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MFM Ijaiye web-api",
                Version = "v1",
                Description = "REST API for the Church Website Management System.",
                Contact = new OpenApiContact
                {
                    Name = "Onwumelu Chijioke \n 0802-9696-733",

                }
            });

            // ConfigureJwt(opt);

        }
        );

        return builder;
    }

    //  private static void ConfigureJwt(SwaggerGenOptions options)
    // {
    //     var jwtScheme = new OpenApiSecurityScheme
    //     {
    //         Name = "Authorization",
    //         Description = "Enter JWT Bearer Token",

    //         In = ParameterLocation.Header,

    //         Type = SecuritySchemeType.Http,

    //         Scheme = "Bearer",

    //         BearerFormat = "JWT",

    //         // dont config swagger yet till after auth
            

    //         Reference = new OpenApiReference
    //         {
    //             Id = "Bearer",
    //             Type = ReferenceType.SecurityScheme
    //         }
    //     };

    //     options.AddSecurityDefinition("Bearer", jwtScheme);

    //     options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //     {
    //         {
    //             jwtScheme,
    //             Array.Empty<string>()
    //         }
    //     });
    // }

    public static WebApplication UiPipline(this WebApplication app)
    {

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            // Swagger JSON
            app.UseSwagger();

            // Swagger UI
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Church Website API v1");

                options.RoutePrefix = "swagger";
                options.DocumentTitle = "Church Website API";
            });

            app.MapScalarApiReference("/scalar",options =>
            {
                options.Title = "Church Website API";
            });
            // app.MapScalarApiReference(options =>
            // {
            //     options.Title = "Church Website API";
            // });
        }

        return app;
    }
}