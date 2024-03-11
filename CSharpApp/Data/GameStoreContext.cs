using CSharpApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSharpApp.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
 : DbContext(options)
{
      public DbSet<Game> Games => Set<Game>();

      public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new {Id = 1, Name = "Fighting"},
            new {Id = 2, Name = "Shooter"},
            new {Id = 3, Name = "Platformer"},
            new {Id = 4, Name = "RPG"},
            new {Id = 5, Name = "Strategy"},
            new {Id = 6, Name = "Sport"},
            new {Id = 7, Name = "Action"},
            new {Id = 8, Name = "Puzzle"},
            new {Id = 9, Name = "Adventure"},
            new {Id = 10, Name = "Racing"},
            new {Id = 11, Name = "MMORPG"},
            new {Id = 12, Name = "MOBA"},
            new {Id = 13, Name = "Horror"},
            new {Id = 14, Name = "MMO"}

        );
    }
}
