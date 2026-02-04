namespace GameStore.Api.Dtos;

public record GameSummaryDto(
    int Id,
    string Title,
    string? Thumbnail,
    string? Description,
    string Genre,
    decimal Price,
    string? Platform,
    string? Publisher,
    string? Developer,
    DateOnly ReleaseDate
);
