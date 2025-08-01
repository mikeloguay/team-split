using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
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
            .WithMethods("GET", "POST")
            .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    // This is to fix openapi documentation not working behind a reverse proxy
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseCors();
app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/players", () =>
{
    return new PlayersResponse([.. PlayersDatabase.Players.Select(p => p.Name).OrderBy(p => p)]);
});

app.MapPost("/players/split", (ITeamSplitter teamSplitter,
    [FromBody] SplitRequest request) =>
{
    Versus versus = teamSplitter.BestSplitRandomFromTops(request.Players);
    return new VersusResponse(
        new TeamResponse([.. versus.Team1.Players.Select(p => p.Name).OrderBy(p => p)]),
        new TeamResponse([.. versus.Team2.Players.Select(p => p.Name).OrderBy(p => p)])
    );
});

app.Run();
