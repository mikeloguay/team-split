# Team Split

Web application that takes a list of players and splits them into 2 balanced teams based on skill level.

- Web: [teamsplit.onrender.com](https://teamsplit.onrender.com/)
- API docs: [teamsplit-api.onrender.com/scalar](https://teamsplit-api.onrender.com/scalar/)

## Stack

- **Backend:** ASP.NET Core 10 Minimal API — team-balancing algorithm with SQLite persistence
- **Frontend:** React 19 + Vite — player management (add/edit/delete) and team splitting

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js LTS](https://nodejs.org/) (includes npm)

## Running locally

### Backend

```powershell
dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj
```

The API starts on `http://localhost:8080`. On first run it creates `teamsplit.db` and seeds it with the default player list.

### Frontend

```powershell
cd frontend
npm install
npm run dev
```

Open `http://localhost:7070`. The frontend automatically points to `http://localhost:8080` when running on localhost.

## Other commands

```powershell
dotnet build backend      # build all backend projects
dotnet test backend       # run backend tests

cd frontend
npm run build             # production build → frontend/dist/
npm run preview           # serve the production build on port 7070
```

## API

Interactive docs at `http://localhost:8080/scalar` while the backend is running.
