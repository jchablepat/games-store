using System;

namespace GameStore.Api.Dtos;

public record GameDetailsDto(
    int Id,
    string Title,
    string? Thumbnail,
    string? Description,
    int GenreId,
    decimal Price,
    string? Platform,
    string? Publisher,
    string? Developer,
    DateOnly ReleaseDate
);
