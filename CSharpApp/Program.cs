using CSharpApp.Data;
using CSharpApp.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");//for sqlite 

builder.Services.AddSqlite<GameStoreContext>(connString);
//this registers our entites and tells asp.net about it

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

app.MigrateDb();

app.Run();
