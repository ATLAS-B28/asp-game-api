using CSharpApp.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        1,
        "Street Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new (
        2,
        "Final Fantasy XIV",
        "JRPG",
        59.89M,
        new DateOnly(2014, 11, 18)
    )
];

app.MapGet("games", () => games);

app.MapGet("games/{id}", (int id) => 
{
    GameDto? game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName("GetGame");

app.MapPost("games", (CreateGameDto newGame) => 
{
    //create a new game by taking the CreateGameDto
    //and adding it to the list of type GameDto
    //update the count + 1 and add it to games list
    //and do the return the results uisng IResult
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
});

app.MapPut("games/{id}", (int id, UpdateGameDtos updatedGame) => 
{
    var index = games.FindIndex(game => game.Id == id);
    games[index] = new GameDto(//set the new values by accessing the id of the game
        id,//put the same id only
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

app.MapDelete("games/{id}", (int id) => 
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.MapGet("/", () => "Hello World!");

app.Run();
