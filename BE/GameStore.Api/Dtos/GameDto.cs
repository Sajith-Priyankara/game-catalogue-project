namespace GameStore.Api.Dtos;

public record class GameDto(
    int Id,
    string Name,
    string Gennr,
    decimal Price,
    DateOnly ReleaseDate
);
