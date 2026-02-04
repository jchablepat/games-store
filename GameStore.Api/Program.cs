using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add validation services if using data annotations, otherwise validation attributes will be ignored.
builder.Services.AddValidation();

builder.Services.AddCors(options => 
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
);

// Add DB Context
builder.AddGameStoreDb();

var app = builder.Build();
app.UseCors();
app.MapGamesEndpoints();
app.MapGenresEndpoints();

try 
{
    app.MigrateDatabase();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating the database.");
}

app.Run();
