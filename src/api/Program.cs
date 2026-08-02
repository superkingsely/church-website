

var builder=WebApplication.CreateBuilder(args);

builder.Configurservice();

var app=builder.Build();

app.ConfigureAppPipline();

app.Run();

















// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();


// app.Run();


