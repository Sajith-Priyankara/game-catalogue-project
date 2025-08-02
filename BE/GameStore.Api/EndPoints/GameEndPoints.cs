using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.EndPoints;

//this is the entension method
public static class GameEndPoints
{
    private const string GetGameEndPointName = "GetGame";
    private readonly static List<GameDto> games = [
        new(
            1,
            "Street Fighter III",
            "Fighting",
            19.99M,
            new DateOnly (1997, 1, 1)),
        new(
            2,
            "Final Fantacy XIV",
            "Roleplaying",
            90.33M,
            new DateOnly (2000, 1,1)),
        new(
            3,
            "FIFA 23",
            "Sport",
            69.55M,
            new DateOnly(2006, 1,1))
    ];

    public static RouteGroupBuilder MapGameEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation();
        //GET /games;;
        group.MapGet("/", () => games);

        //GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            // if id exsist give GameDto ot if not give null
            GameDto? game = games.Find(games => games.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);

        })
        .WithName(GetGameEndPointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            //convert CreateGamneDto to normal GameDto
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Gennr,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);

            //Provide Location headr to client then he know where this can find
            return Results.AcceptedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
        });

        //PUT /game/2
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            int index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
            else
            {
                games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Gennr,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

                return Results.NoContent();
            }


        });


        //DELETE /games/2
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
