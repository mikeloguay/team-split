# AGENTS.md

This file provides guidance to agents working with code in this repository.

## Project Overview

**Team Split** is a web application that splits a list of players into two balanced teams based on skill levels.

- **Live Frontend:** https://teamsplit.onrender.com
- **Live API:** https://teamsplit-api.onrender.com

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | .NET 10, ASP.NET Core Minimal API |
| Database | PostgreSQL + EF Core (Npgsql) |
| Auth | Google OAuth 2.0 (JWT) |
| Frontend | React 19 + TypeScript + Vite |

## Commands

### Backend

```bash
dotnet build backend                                              # Build solution
dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj  # Run API on port 8080
dotnet test backend                                               # Run all tests
dotnet test backend --filter "FullyQualifiedName~ClassName"       # Run a single test class
```

### Frontend

```bash
cd frontend
npm install          # Install dependencies
npm run dev          # Start dev server on http://localhost:7070
npm run build        # Build for production
npm run preview      # Preview production build
```

## Architecture

### Backend (`backend/`)

Three projects in the solution:

- **`TeamSplit`** — Core library. Contains domain models (`Player`, `Team`, `Versus`), the `TeamSplitter` algorithm, and static `PlayersDatabase`. No external dependencies.
- **`TeamSplit.Api`** — ASP.NET Core Minimal API. Exposes endpoints and depends on the core library.
- **`TeamSplit.Test`** — xUnit tests for the algorithm.

### Algorithm (`backend/TeamSplit/TeamSplitter.cs`)

1. Generates all possible C(n, n/2) team combinations recursively.
2. Pairs each team with the remaining players to form a `Versus`.
3. Finds all `Versus` instances with the minimum `LevelDiff`.
4. Returns one at random from the top splits (or all of them via `TopSplits`).

### API Endpoints (`backend/TeamSplit.Api/Program.cs`)

| Method | Path | Description |
|--------|------|-------------|
| `GET` | `/players` | Returns sorted player names from `PlayersDatabase` |
| `POST` | `/players/split` | Accepts `{players: string[]}`, returns `{team1, team2}` |
| `GET` | `/scalar` | Scalar API docs UI |

Errors are normalized to RFC 7807 `ProblemDetails` via a global exception handler. An odd player count throws `ArgumentException` → 400.

### Frontend (`frontend/`)

React SPA using TypeScript. Player selection state is persisted in `localStorage`.

Key files:
- `frontend/src/App.tsx` — Root component
- `frontend/src/api.ts` — API client (includes `fetchWithRetry` for Render cold starts)
- `frontend/src/components/Players.tsx` — Player selection UI
- `frontend/src/components/Splitter.tsx` — Team display

### Player Data

Players are defined as a static `HashSet<Player>` in `backend/TeamSplit/PlayersDatabase.cs`. Each player has a `Name` and a numeric `Level`. This is the only place to add/modify players.

## Environment Variables

**Backend:** `DATABASE_URL`, `GOOGLE_CLIENT_ID`

**Frontend:** `VITE_GOOGLE_CLIENT_ID` — create `frontend/.env.local`:
```
VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com
```

## CORS

Allowed origins: `https://teamsplit.onrender.com`, `http://localhost:7070`, `http://localhost:5500`.

## CI/CD

GitHub Actions (`.github/workflows/ci-backend.yml`) runs on push to `main` when backend files change: restore → build (Release) → test.

## Docker

`docker-compose.yml` provides a PostgreSQL 18 instance on port 5432 with credentials `teamsplit/teamsplit`.

## Conventions

- Backend uses Minimal API pattern (no controllers).
- Services like `ITeamSplitter` are registered as `Scoped`.
- Frontend uses `@react-oauth/google` for Google authentication.
- API tooling: Bruno collections in `bruno/` directory.
