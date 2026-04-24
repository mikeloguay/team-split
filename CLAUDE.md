# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Team Split** is a web application that splits a list of soccer players into two balanced teams based on skill levels. It uses a combinatorial algorithm to find the optimal team split with minimum level difference.

- Live frontend: https://teamsplit.onrender.com
- Live API: https://teamsplit-api.onrender.com

## Commands

### Backend

```bash
dotnet build backend                                              # Build solution
dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj  # Run API on port 8080
dotnet test backend                                               # Run all tests
dotnet test backend --filter "FullyQualifiedName~ClassName"       # Run a single test class
```

### Frontend

Static HTML/CSS/JS — open `frontend/index.html` directly in a browser or use any local static server (e.g., VS Code Live Server on port 5500). The frontend switches between `http://localhost:8080` (dev) and `https://teamsplit-api.onrender.com` (prod) based on hostname.

## Architecture

### Backend (`backend/`)

Four projects in the solution:

- **`TeamSplit`** — Core library. Contains the domain models (`Player`, `Team`, `Versus`) and the `TeamSplitter` algorithm. No external dependencies.
- **`TeamSplit.Api`** — ASP.NET Core 9 Minimal API. Exposes two endpoints and depends on the core library.
- **`TeamSplit.Test`** — xUnit tests for the algorithm.
- **`TeamSplit.Console`** — Simple CLI tool for manually testing the algorithm.

### Algorithm (`TeamSplit/TeamSplitter.cs`)

1. Generates all possible C(n, n/2) team combinations recursively.
2. Pairs each team with the remaining players to form a `Versus`.
3. Finds all `Versus` instances with the minimum `LevelDiff`.
4. Returns one at random from the top splits (or all of them via `TopSplits`).

### API Endpoints (`TeamSplit.Api/Program.cs`)

| Method | Path | Description |
|--------|------|-------------|
| `GET` | `/players` | Returns sorted player names from `PlayersDatabase` |
| `POST` | `/players/split` | Accepts `{players: string[]}`, returns `{team1, team2}` |
| `GET` | `/scalar` | Scalar API docs UI |

Errors are normalized to RFC 7807 `ProblemDetails` via a global exception handler. An odd player count throws `ArgumentException` → 400.

### Frontend (`frontend/`)

Single-page vanilla JS. Player checkbox states are persisted in `localStorage`. A `fetchWithRetry` utility handles cold starts on Render's free tier. Teams are rendered as a football-field visualization with jersey icons.

### Player Data

Players are defined as a static `HashSet<Player>` in `TeamSplit/PlayersDatabase.cs`. Each player has a `Name` and a numeric `Level`. This is the only place to add/modify players.

### CORS

Allowed origins: `https://teamsplit.onrender.com`, `http://localhost:7070`, `http://localhost:5500`.

### CI/CD

GitHub Actions (`.github/workflows/ci-backend.yml`) runs on push to `main` when backend files change: restore → build (Release) → test.
