using Microsoft.EntityFrameworkCore;

namespace CSharpApp.Data;
//used during the startup
public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        //migrate the database using APIs
        //and should have a scope to use it
        using var scope = app.Services.CreateScope();
        //give instances of certain services
        //we create a scope of the services and us the service provider to get the database
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();//provides access to the database and related opertions
    }
}
