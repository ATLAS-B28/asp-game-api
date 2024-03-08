using CSharpApp.Dtos;

namespace CSharpApp.Endpoints;

public static class GamesEndpoints
{
    //extend this class
   ///put things that should not be in program.cs
   const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
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

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app) 
    {
      //use group builder to define the common things
       var group = app.MapGroup("games");   
       group.MapGet("/", () => games);

       group.MapGet("/{id}", (int id) => 
       {
        GameDto? game = games.Find(game => game.Id == id);
 
        return game is null ? Results.NotFound() : Results.Ok(game);
       })
       .WithName("GetGame");

//one can use _ = rest of the code
       group.MapPost("/", (CreateGameDto newGame) => 
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
       })
       .WithParameterValidation();
 
       group.MapPut("/{id}", (int id, UpdateGameDtos updatedGame) =>
       {
          var index = games.FindIndex(game => game.Id == id);
  
          if (index != -1)
          {
           games[index] = new GameDto(//set the new values by accessing the id of the game
              id,//put the same id only
              updatedGame.Name,
              updatedGame.Genre,
              updatedGame.Price,
              updatedGame.ReleaseDate
             );

           return Results.NoContent();
          }
 
          return Results.NotFound();
         });

       group.MapDelete("/{id}", (int id) => 
       {
        games.RemoveAll(game => game.Id == id);
 
        return Results.NoContent();
       });

     return group;
    }
}
