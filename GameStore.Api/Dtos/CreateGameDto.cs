using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDto(
    [Required] [StringLength(50)] string Title,
    [StringLength(250)] string? Thumbnail,
    [StringLength(500)] string? Description,
    [Required] int GenreId,
    decimal Price,
    [StringLength(50)] string? Platform,
    [StringLength(50)] string? Publisher,
    [StringLength(50)] string? Developer,
    DateOnly ReleaseDate
);
