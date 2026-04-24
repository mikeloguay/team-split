using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TeamSplit;
using TeamSplit.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ - ";
    options.SingleLine = true;
    options.IncludeScopes = false;
    options.UseUtcTimestamp = true;
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<ITeamSplitter, TeamSplitter>();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=teamsplit.db"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                "https://teamsplit.onrender.com",
                "http://localhost:7070",
                "http://localhost:5500"
            )
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Players.Any())
    {
        db.Players.AddRange(PlayersDatabase.Players.Select(p => new PlayerEntity { Name = p.Name, Level = p.Level }));
        db.SaveChanges();
    }
}

app.UseExceptionHandler();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    // This is to fix openapi documentation not working behind a reverse proxy
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseCors();
app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/players", async (AppDbContext db) =>
{
    var players = await db.Players.OrderBy(p => p.Name).ToListAsync();
    return players.Select(p => new PlayerResponse(p.Name, p.Level));
});

app.MapGet("/players/{name}", async (string name, AppDbContext db) =>
{
    var player = await db.Players.FindAsync(name);
    return player is null
        ? Results.NotFound()
        : Results.Ok(new PlayerResponse(player.Name, player.Level));
});

app.MapPost("/players", async (CreatePlayerRequest request, AppDbContext db) =>
{
    if (await db.Players.AnyAsync(p => p.Name == request.Name))
        return Results.Conflict();

    var entity = new PlayerEntity { Name = request.Name, Level = request.Level };
    db.Players.Add(entity);
    await db.SaveChangesAsync();
    return Results.Created($"/players/{entity.Name}", new PlayerResponse(entity.Name, entity.Level));
});

app.MapPut("/players/{name}", async (string name, UpdatePlayerRequest request, AppDbContext db) =>
{
    var player = await db.Players.FindAsync(name);
    if (player is null) return Results.NotFound();

    player.Level = request.Level;
    await db.SaveChangesAsync();
    return Results.Ok(new PlayerResponse(player.Name, player.Level));
});

app.MapDelete("/players/{name}", async (string name, AppDbContext db) =>
{
    var player = await db.Players.FindAsync(name);
    if (player is null) return Results.NotFound();

    db.Players.Remove(player);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPost("/players/split", async (ITeamSplitter teamSplitter, AppDbContext db,
    [FromBody] SplitRequest request) =>
{
    var playerEntities = await db.Players
        .Where(p => request.Players.Contains(p.Name))
        .ToListAsync();

    var players = playerEntities
        .Select(p => new Player { Name = p.Name, Level = p.Level })
        .ToHashSet();

    Versus versus = teamSplitter.BestSplitRandomFromTops(players);
    return new VersusResponse(
        new TeamResponse([.. versus.Team1.Players.Select(p => p.Name).OrderBy(p => p)]),
        new TeamResponse([.. versus.Team2.Players.Select(p => p.Name).OrderBy(p => p)])
    );
});

app.Run();
