using System;

namespace GameStore.Api.Models;

public class Game
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Thumbnail { get; set; }
    public string? Description { get; set; }
    public Genre? Genre { get; set; }
    public int GenreId { get; set; } // Foreign key
    public decimal Price { get; set; }
    public string? Platform { get; set; }
    public string? Publisher { get; set; }
    public string? Developer { get; set; }
    public DateOnly ReleaseDate { get; set; }
}
