using System.ComponentModel.DataAnnotations;

namespace CSharpApp.Dtos;

public record class CreateGameDto(
    [Required][StringLength(50)] string Name,
    int GenreId,
    [Range(1,1000)] decimal Price,
    DateOnly ReleaseDate
);
