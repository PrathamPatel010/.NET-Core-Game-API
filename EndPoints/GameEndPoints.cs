using System;
using FirstDotNetCoreAPI.DTOs;

namespace FirstDotNetCoreAPI.EndPoints;

public static class GameEndPoints
{
    private readonly static List<GameDto> games = [
    new (1, "The Legend of Zelda: Breath of the Wild", "Action-Adventure", 59.99M, new DateOnly(2017, 3, 3)),
    new (2, "God of War", "Action", 49.99M, new DateOnly(2018, 4, 20)),
    new (3, "RDR2", "Action-Adventure", 39.99M, new DateOnly(2018, 10, 26)),
    new (4, "The Witcher 3", "RPG", 29.99M, new DateOnly(2015, 5, 19)),
    new (5, "Minecraft", "Sandbox", 26.95M, new DateOnly(2011, 11, 18))
    ];

    public static WebApplication MapGameEndPoints(this WebApplication app)
    {
        // GET /games
        app.MapGet("/games", () =>
        {
            return games;
        });

        // GET /games/1
        app.MapGet("/games/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        });

        // POST /games
        app.MapPost("/games", (CreateGameDto newGameData) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGameData.Name,
                newGameData.Genre,
                newGameData.Price,
                newGameData.ReleaseDate
                );
            games.Add(game);
            return Results.Json(game, statusCode: 201);
        });

        // PUT /games/1
        app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        // DELETE /games/1
        app.MapDelete("/games/{id}", (int id) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }

            games.Remove(games[index]);
            return Results.NoContent();
        });

        return app;
    }
}
