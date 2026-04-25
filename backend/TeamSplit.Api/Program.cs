using Microsoft.AspNetCore.Authentication.JwtBearer;
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

var databaseUrl = builder.Configuration["DATABASE_URL"]
    ?? throw new InvalidOperationException("DATABASE_URL not configured");

var connectionString = databaseUrl.StartsWith("postgresql://") || databaseUrl.StartsWith("postgres://")
    ? ConvertDatabaseUrl(databaseUrl)
    : databaseUrl;

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

var googleClientId = builder.Configuration["GOOGLE_CLIENT_ID"]
    ?? throw new InvalidOperationException("GOOGLE_CLIENT_ID not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.Audience = googleClientId;
        options.MapInboundClaims = false;
    });

builder.Services.AddAuthorization();

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
}

app.UseExceptionHandler();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/players", async (AppDbContext db, HttpContext ctx) =>
{
    var userId = ctx.User.FindFirst("sub")!.Value;
    var players = await db.Players
        .Where(p => p.UserId == userId)
        .OrderBy(p => p.Name)
        .ToListAsync();
    return players.Select(p => new PlayerResponse(p.Name, p.Level));
}).RequireAuthorization();

app.MapGet("/players/{name}", async (string name, AppDbContext db, HttpContext ctx) =>
{
    var userId = ctx.User.FindFirst("sub")!.Value;
    var player = await db.Players.FindAsync(userId, name);
    return player is null
        ? Results.NotFound()
        : Results.Ok(new PlayerResponse(player.Name, player.Level));
}).RequireAuthorization();

app.MapPost("/players", async (CreatePlayerRequest request, AppDbContext db, HttpContext ctx) =>
{
    var userId = ctx.User.FindFirst("sub")!.Value;
    if (await db.Players.AnyAsync(p => p.UserId == userId && p.Name == request.Name))
        return Results.Conflict();

    var email = ctx.User.FindFirst("email")?.Value;
    var entity = new PlayerEntity { UserId = userId, Name = request.Name, Level = request.Level, Email = email };
    db.Players.Add(entity);
    await db.SaveChangesAsync();
    return Results.Created($"/players/{entity.Name}", new PlayerResponse(entity.Name, entity.Level));
}).RequireAuthorization();

app.MapPut("/players/{name}", async (string name, UpdatePlayerRequest request, AppDbContext db, HttpContext ctx) =>
{
    var userId = ctx.User.FindFirst("sub")!.Value;
    var player = await db.Players.FindAsync(userId, name);
    if (player is null) return Results.NotFound();

    player.Level = request.Level;
    await db.SaveChangesAsync();
    return Results.Ok(new PlayerResponse(player.Name, player.Level));
}).RequireAuthorization();

app.MapDelete("/players/{name}", async (string name, AppDbContext db, HttpContext ctx) =>
{
    var userId = ctx.User.FindFirst("sub")!.Value;
    var player = await db.Players.FindAsync(userId, name);
    if (player is null) return Results.NotFound();

    db.Players.Remove(player);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

app.MapPost("/players/split", async (ITeamSplitter teamSplitter, AppDbContext db, HttpContext ctx,
    [FromBody] SplitRequest request) =>
{
    var userId = ctx.User.FindFirst("sub")!.Value;
    var playerEntities = await db.Players
        .Where(p => p.UserId == userId && request.Players.Contains(p.Name))
        .ToListAsync();

    var players = playerEntities
        .Select(p => new Player { Name = p.Name, Level = p.Level })
        .ToHashSet();

    Versus versus = teamSplitter.BestSplitRandomFromTops(players);
    return new VersusResponse(
        new TeamResponse([.. versus.Team1.Players.Select(p => p.Name).OrderBy(p => p)]),
        new TeamResponse([.. versus.Team2.Players.Select(p => p.Name).OrderBy(p => p)])
    );
}).RequireAuthorization();

app.Run();

static string ConvertDatabaseUrl(string url)
{
    var uri = new Uri(url);
    var userInfo = uri.UserInfo.Split(':', 2);
    var host = uri.Host;
    var port = uri.IsDefaultPort ? 5432 : uri.Port;
    var database = uri.AbsolutePath.TrimStart('/');
    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
    var sslMode = query["sslmode"] switch
    {
        "require" or "verify-ca" or "verify-full" => "Require",
        "disable" => "Disable",
        _ => "Prefer"
    };
    return $"Host={host};Port={port};Database={database};Username={userInfo[0]};Password={userInfo[1]};SSL Mode={sslMode};Trust Server Certificate=true";
}
