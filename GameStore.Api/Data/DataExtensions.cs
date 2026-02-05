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
        
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var databaseProvider = configuration["DatabaseProvider"] ?? "SQLite";

        // Check if we are explicitly using PostgreSQL or if DATABASE_URL is set (legacy/Render support)
        if (databaseProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(configuration["DATABASE_URL"]))
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
        var databaseProvider = builder.Configuration["DatabaseProvider"] ?? "SQLite";
        var databaseUrl = builder.Configuration["DATABASE_URL"];
        
        // Determine if we should use PostgreSQL
        if (databaseProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(databaseUrl))
        {
            // Use PostgreSQL
            string connectionString;
            
            // Priority 1: DATABASE_URL (Environment Variable - e.g., Render)
            if (!string.IsNullOrEmpty(databaseUrl))
            {
                connectionString = ConvertUrlToConnectionString(databaseUrl);
            }
            // Priority 2: PostgreSQLConnection (AppSettings)
            else
            {
                var configConnString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
                if (string.IsNullOrEmpty(configConnString))
                {
                     throw new InvalidOperationException("PostgreSQL provider selected but no connection string found in 'PostgreSQLConnection' or 'DATABASE_URL'.");
                }

                // Check if the config connection string is actually a URI (e.g. copied from Render to appsettings)
                if (Uri.TryCreate(configConnString, UriKind.Absolute, out var uri) && (uri.Scheme == "postgres" || uri.Scheme == "postgresql"))
                {
                    connectionString = ConvertUrlToConnectionString(configConnString);
                }
                else
                {
                    // Assume it's a standard Npgsql connection string
                    connectionString = configConnString;
                }
            }
            
            configureDbContext = options => options.UseNpgsql(connectionString).UseSeeding(SeedData);
        }
        else
        {
            // Use SQLite (Default/Local fallback)
            var connectionString = builder.Configuration.GetConnectionString("SQLiteConnection");
            configureDbContext = options => options.UseSqlite(connectionString).UseSeeding(SeedData);
        }

        builder.Services.AddDbContext<GameStoreContext>(configureDbContext);
    }

    private static string ConvertUrlToConnectionString(string url)
    {
        var databaseUri = new Uri(url);
        var userInfo = databaseUri.UserInfo.Split(':');
        var port = databaseUri.Port == -1 ? 5432 : databaseUri.Port;
        return $"Host={databaseUri.Host};Port={port};Database={databaseUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
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
