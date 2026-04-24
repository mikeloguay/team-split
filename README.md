# Team Split

Web application that takes a list of players and splits them into 2 balanced teams based on skill level.

- Web: [teamsplit.onrender.com](https://teamsplit.onrender.com/)
- API docs: [teamsplit-api.onrender.com/scalar](https://teamsplit-api.onrender.com/scalar/)

## Stack

- **Backend:** ASP.NET Core 10 Minimal API — runs the team-balancing algorithm and exposes a REST API with SQLite persistence
- **Frontend:** Vanilla HTML, CSS, and JavaScript — calls the API and renders results as a football field visualization

## Running locally

**1. Start the backend**

```sh
dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj
```

The API starts on `http://localhost:8080`. On first run it creates `teamsplit.db` and seeds it with the default player list.

**2. Serve the frontend**

Opening `index.html` directly as a file causes CORS errors. Serve it from a local HTTP server instead — ports `7070` and `5500` are already in the API's CORS allowlist.

Option A — Python (no install required):
```sh
cd frontend
python -m http.server 7070
```
Then open `http://localhost:7070`.

Option B — VS Code Live Server extension: open the `frontend/` folder and click **Go Live** (serves on port 5500 by default).

The frontend automatically points to `http://localhost:8080` when running from localhost — no configuration needed.

## Other commands

```sh
dotnet build backend          # build all projects
dotnet test backend           # run tests
```

## API

Interactive docs available at `http://localhost:8080/scalar` while the backend is running.
