using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    /// <summary>
    /// Applies any pending migrations for the context to the database. Will create the database if it does not already exist.
    /// </summary>
    /// <param name="app"></param>
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        
        // If we are using Postgres (likely in production), we might want to skip migrations if they are incompatible
        // or ensure the database is created.
        // For simple prototyping on Render, EnsureCreated is safer if migrations are SQLite-specific.
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var databaseUrl = configuration["DATABASE_URL"];

        if (!string.IsNullOrEmpty(databaseUrl))
        {
             // In production with Postgres, just ensure created to avoid migration conflicts
             dbContext.Database.EnsureCreated();
        }
        else
        {
             // Local development with SQLite
             dbContext.Database.Migrate();
        }
    }

    /// <summary>
    /// Adds the GameStore database context to the service collection with SQLite or PostgreSQL provider and seeds initial data.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        Action<DbContextOptionsBuilder> configureDbContext;
        var databaseUrl = builder.Configuration["DATABASE_URL"];

        if (!string.IsNullOrEmpty(databaseUrl))
        {
            // Use PostgreSQL
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var connectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
            
            configureDbContext = options => options.UseNpgsql(connectionString)
                                                   .UseSeeding(SeedData);
        }
        else
        {
            // Use SQLite (Local fallback)
            var connectionString = builder.Configuration.GetConnectionString("GameStoreConnection");
            configureDbContext = options => options.UseSqlite(connectionString)
                                                   .UseSeeding(SeedData);
        }

        builder.Services.AddDbContext<GameStoreContext>(configureDbContext);
    }

    private static void SeedData(DbContext ctx, bool _)
    {
        // Seed initial data
        if (!ctx.Set<Genre>().Any())
        {
            ctx.Set<Genre>().AddRange(
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Strategy" },
                new Genre { Name = "Simulation" },
                new Genre { Name = "Sports" },
                new Genre { Name = "Puzzle" },
                new Genre { Name = "Horror" },
                new Genre { Name = "Fighting" },
                new Genre { Name = "Platformer" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Shooter" }
            );
            ctx.SaveChanges();
        }
    }
}
