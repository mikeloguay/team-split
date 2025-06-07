using Microsoft.AspNetCore.Mvc;
using TeamSplit;
using TeamSplit.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ITeamSplitter, TeamSplitter>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

app.MapGet("/players", () =>
{
    return PlayersDatabase.Players;
});

app.MapPost("/players/split", (ITeamSplitter teamSplitter,
    [FromBody] SplitRequest request) =>
{
    Versus versus = teamSplitter.BestSplitRandomFromTops(request.Players);
    return new VersusResponse(
        new TeamResponse("Con petos", [.. versus.Team1.Players.Select(p => p.Name)]),
        new TeamResponse("Sin petos", [.. versus.Team2.Players.Select(p => p.Name)])
    );
});

app.Run();
