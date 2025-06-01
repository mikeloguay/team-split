using Microsoft.AspNetCore.Mvc;
using TeamSplit;
using TeamSplit.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ITeamSplitter, TeamSplitter>();

var app = builder.Build();

app.MapGet("/players", () =>
{
    return PlayersDatabase.Players;
});

app.MapGet("/players/split-default", (ITeamSplitter teamSplitter) =>
{
    return teamSplitter.BestSplitRandomFromTops([.. PlayersDatabase.Players]);
});

app.MapPost("/players/split", (ITeamSplitter teamSplitter,
    [FromBody] PlayersRequest request) =>
{
    return teamSplitter.BestSplitRandomFromTops([.. request.Players]);
});

app.Run();
