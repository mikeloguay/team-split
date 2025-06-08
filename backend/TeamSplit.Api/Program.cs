using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using TeamSplit;
using TeamSplit.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<ITeamSplitter, TeamSplitter>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                "https://team-split-site.onrender.com",
                "http://localhost:7070"
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
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
    | ForwardedHeaders.XForwardedHost
});

app.UseCors();

app.MapOpenApi();

var api = app.MapGroup("/api");
api.MapScalarApiReference();

api.MapGet("/players", () =>
{
    return new PlayersResponse([.. PlayersDatabase.Players.Select(p => p.Name)]);
});

api.MapPost("/players/split", (ITeamSplitter teamSplitter,
    [FromBody] SplitRequest request) =>
{
    Versus versus = teamSplitter.BestSplitRandomFromTops(request.Players);
    return new VersusResponse(
        new TeamResponse("Con petos", [.. versus.Team1.Players.Select(p => p.Name)]),
        new TeamResponse("Sin petos", [.. versus.Team2.Players.Select(p => p.Name)])
    );
});

app.Run();
