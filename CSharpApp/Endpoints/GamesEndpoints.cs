using CSharpApp.Data;
using CSharpApp.Dtos;
using CSharpApp.Entities;
using CSharpApp.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CSharpApp.Endpoints;

public static class GamesEndpoints
{
    //extend this class
   ///put things that should not be in program.cs
   const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app) 
    {
      //use group builder to define the common things
       var group = app.MapGroup("games").WithParameterValidation(); 
         
       group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                           .Include(game => game.Genre)
                           .Select(game => game.ToGameSummaryDto())
                           .AsNoTracking()
                           .ToListAsync()
       );

       group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => 
       {
        //GameDto? game = games.Find(game => game.Id == id);
        Game? game = await dbContext.Games.FindAsync(id);

        return game is null ? 
             Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
       })
       .WithName(GetGameEndpointName);

       //one can use _ = rest of the code
       group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => 
       {
         //create a new game by taking the CreateGameDto
         //and adding it to the list of type GameDto
         //update the count + 1 and add it to games list
         //and do the return the results uisng IResult
         //do DI with dbContext
         Game game = newGame.ToEntity();
         game.Genre = dbContext.Genres.Find(newGame.GenreId)!;
 
         dbContext.Games.Add(game);
         await dbContext.SaveChangesAsync();
 
         return Results.CreatedAtRoute(
                      GetGameEndpointName,
                      new {id = game.Id}, 
                      game.ToGameDetailsDto());
        });
 
      group.MapPut("/{id}", async (int id, UpdateGameDtos updateGameDtos, GameStoreContext dbContext) => 
      {
        var existingGame = await dbContext.Games.FindAsync(id);

        if(existingGame is null)
        {
          return Results.NotFound();
        }

        dbContext.Entry(existingGame)
                 .CurrentValues
                 .SetValues(updateGameDtos.ToEntity(id));

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
      });

       group.MapDelete("/{id}", async (int id, GameStoreContext dbConstext) => 
       {
        await dbConstext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
 
        return Results.NoContent();
       });

     return group;
    }
}
