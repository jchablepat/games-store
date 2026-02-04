using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        group.MapGet("/", async (GameStoreContext db) =>
            await db.Genres
                .Select(g => new GenreDto(g.Id, g.Name))
                .AsNoTracking()
                .ToListAsync());
    }
}
