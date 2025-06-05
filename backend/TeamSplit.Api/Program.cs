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
    [FromBody] PlayersRequest request) =>
{
    return teamSplitter.BestSplitRandomFromTops(request.Players);
});

app.Run();
