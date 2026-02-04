using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameRouteName = "GetGameById";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        app.MapGet("/", () => "Hello World!");

        // GET /games
        group.MapGet("/", async (GameStoreContext dbcontext) 
            => await dbcontext.Games
            .Include(g => g.Genre)
            .Select(game => new GameSummaryDto(
                game.Id,
                game.Title,
                game.Thumbnail,
                game.Description,
                game.Genre!.Name,
                game.Price,
                game.Platform,
                game.Publisher,
                game.Developer,
                game.ReleaseDate
            ))
            .AsNoTracking()
            .ToListAsync()
        );

        // GET /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbcontext) =>
        {
            var game = await dbcontext.Games.FindAsync(id);

            return game is not null ? Results.Ok(new GameDetailsDto(
                game.Id,
                game.Title,
                game.Thumbnail,
                game.Description,
                game.GenreId,
                game.Price,
                game.Platform,
                game.Publisher,
                game.Developer,
                game.ReleaseDate
            )) : Results.NotFound(new { Message = "Game not found." });
        }).WithName(GetGameRouteName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbcontext) =>
        {
            var game = new Game() {
                Title = newGame.Title,
                Thumbnail = newGame.Thumbnail,
                Description = newGame.Description,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                Platform = newGame.Platform,
                Publisher = newGame.Publisher,
                Developer = newGame.Developer,
                ReleaseDate = newGame.ReleaseDate
            };

            dbcontext.Games.Add(game);
            dbcontext.SaveChanges();

            var gameDetails = new GameDetailsDto(
                game.Id,
                game.Title,
                game.Thumbnail,
                game.Description,
                game.GenreId,
                game.Price,
                game.Platform,
                game.Publisher,
                game.Developer,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameRouteName, new { id = gameDetails.Id }, gameDetails);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", async (int id, CreateGameDto updatedGame, GameStoreContext dbcontext) =>
        {
            var existingGame = await dbcontext.Games.FindAsync(id);
            if (existingGame == null)
            {
                return Results.NotFound(new { Message = "Game not found." });
            }

            existingGame.Title = updatedGame.Title;
            existingGame.Thumbnail = updatedGame.Thumbnail;
            existingGame.Description = updatedGame.Description;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.Platform = updatedGame.Platform;
            existingGame.Publisher = updatedGame.Publisher;
            existingGame.Developer = updatedGame.Developer;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbcontext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbcontext) =>
        {
            await dbcontext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
            
            return Results.NoContent();
        });
    }

}
