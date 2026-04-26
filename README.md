# Team Split

Web application that splits a list of team players into two balanced teams based on skill level.

- **Frontend:** https://teamsplit.onrender.com
- **API docs:** https://teamsplit-api.onrender.com/scalar

## Tech stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 10 Minimal API |
| Database | PostgreSQL + EF Core (Npgsql) |
| Auth | Google OAuth 2.0 (JWT) |
| Frontend | React 19 + TypeScript + Vite |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js LTS](https://nodejs.org/)
- A PostgreSQL instance
- A Google OAuth 2.0 client ID ([Google Cloud Console](https://console.cloud.google.com/))

## Environment variables

**Backend:** `DATABASE_URL`, `GOOGLE_CLIENT_ID`

**Frontend:** `VITE_GOOGLE_CLIENT_ID` — create `frontend/.env.local`:
```
VITE_GOOGLE_CLIENT_ID=your-client-id.apps.googleusercontent.com
```

## Running locally

```bash
# Backend — starts on http://localhost:8080
dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj

# Frontend — starts on http://localhost:7070
cd frontend && npm install && npm run dev
```

## Other commands

```bash
dotnet build backend
dotnet test backend

cd frontend
npm run build
npm run preview
```
