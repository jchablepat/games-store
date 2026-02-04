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
app.MigrateDatabase();

app.Run();
