# Team Split - Gemini Context

This project is a web application designed to split a list of players into two balanced teams based on their skill levels. It consists of a .NET 9 backend API and a vanilla JavaScript frontend.

## Project Overview

- **Purpose:** Automatically generate the best (most balanced) team combinations from a selected list of players.
- **Architecture:**
  - **Backend:** ASP.NET Core 9 Minimal API (`TeamSplit.Api`).
  - **Logic Layer:** A separate library (`TeamSplit`) handles the combinatorial logic for team splitting.
  - **Frontend:** A simple, single-page application (`frontend/`) using vanilla HTML, CSS, and JavaScript.
  - **Data:** Players are currently stored in a static list within `backend/TeamSplit/PlayersDatabase.cs`.
- **Key Algorithm:** The `TeamSplitter` generates all possible team combinations, calculates the total level for each, and selects a random option from those with the minimum level difference.

## Technologies

- **Backend:** .NET 9, ASP.NET Core, Scalar (API Documentation), Problem Details (Error Handling).
- **Frontend:** HTML5, CSS3, Vanilla JavaScript.
- **Testing:** xUnit (in `backend/TeamSplit.Test`).
- **API Tooling:** Bruno (collections in `bruno/`).
- **CI/CD:** GitHub Actions (defined in `.github/workflows/ci-backend.yml`).

## Building and Running

### Backend

To build the entire backend solution:
```sh
dotnet build backend
```

To run the API:
```sh
dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj
```
The API will be available at `http://localhost:8080` (as configured in `frontend/script.js` for local dev). Scalar documentation is available at `/scalar`.

### Testing

To run the backend tests:
```sh
dotnet test backend
```

### Frontend

The frontend is static. You can serve it using any local web server (e.g., VS Code Live Server) or simply open `frontend/index.html` in a browser. It expects the API to be running on `http://localhost:8080` for local development.

## Development Conventions

- **Minimal APIs:** The backend uses the Minimal API pattern for simplicity.
- **Dependency Injection:** Services like `ITeamSplitter` are registered as `Scoped`.
- **Error Handling:** Uses `GlobalExceptionHandler` and `ProblemDetails` for consistent API errors.
- **CORS:** Configured in `Program.cs` to allow requests from the deployed frontend and common local development ports.
- **Frontend API Calls:** Uses a custom `fetchWithRetry` utility in `script.js` to handle potential transient failures (useful for cold starts on free hosting like Render).
- **Styling:** Vanilla CSS with a "football field" visualization for the resulting teams.

## Key Files

- `backend/TeamSplit/TeamSplitter.cs`: Core logic for generating balanced teams.
- `backend/TeamSplit.Api/Program.cs`: API routes and configuration.
- `backend/TeamSplit/PlayersDatabase.cs`: Source of truth for player data.
- `frontend/script.js`: Handles UI interactions and API integration.
- `bruno/team-split/`: API request collection for manual testing.
