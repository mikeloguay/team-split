# Team Split

cambio 1

- Web deployed at [Team Split](https://teamsplit.onrender.com/)
- API deployed at [API](https://teamsplit-api.onrender.com/scalar/)
- CLI just for testing purposes.

Web application that take a list of players, and split them into 2 teams, based on levels of each player.

Very simple and minimalistic, it is based on:
- Backend: ASP.NET API that do the calculations
- Frontend: just a simple HTML, CSS and vanilla JavaScript call the API and render the results.

## Functional features

- Players list defined in a static list, with levels between 1 and 100
- The API calculates the best combination of teams, based on the level of players. It return a random one if there are more than one best option

## Limitations and future development

- UI general improvements: teams organized in a football field, with pictures, etc.
- Be able to add new players dinamically
- User login and storage, so every user could have its own list of players
- ...

## Build and run backend

```sh
dotnet build backend
```

```sh
dotnet run --project backend/TeamSplit.Api/TeamSplit.Api.csproj
```