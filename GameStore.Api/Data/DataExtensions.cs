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
        dbContext.Database.Migrate();
    }

    /// <summary>
    /// Adds the GameStore database context to the service collection with SQLite provider and seeds initial data.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("GameStoreConnection");
        
        builder.Services.AddSqlite<GameStoreContext>(
            connectionString, 
            optionsAction: opts => opts.UseSeeding((ctx, __) =>
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
            })
        );
    }
}
