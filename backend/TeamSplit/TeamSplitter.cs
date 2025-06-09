using Microsoft.Extensions.Logging;

namespace TeamSplit;

public class TeamSplitter(ILogger<TeamSplitter> logger) : ITeamSplitter
{
    public Versus BestSplitRandomFromTops(HashSet<string> playerNames)
    {
        HashSet<Player> players = [.. PlayersDatabase.Players.Where(p => playerNames.Contains(p.Name))];
        return BestSplitRandomFromTops([.. players]);
    }

    public Versus BestSplitRandomFromTops(HashSet<Player> players)
    {
        HashSet<Versus> topSplits = TopSplits(players);
        var best = topSplits.ElementAt(Random.Shared.Next(0, topSplits.Count));
        logger.LogInformation("Best random split from tops: {BestSplit}", best);
        return best;
    }

    public HashSet<Versus> TopSplits(HashSet<Player> players)
    {
        logger.LogInformation("Generating splits for {PlayerCount} players...", players.Count);
        ValidatePlayers(players);

        // Shuffle players to ensure randomness
        players = [.. players.OrderBy(_ => Random.Shared.Next())];

        HashSet<Versus> allPossibleVersus = GenerateAllVersus(players);
        logger.LogInformation("{SplitCount} splits generated", allPossibleVersus.Count);

        HashSet<Versus> allSplitsOrdered = [.. allPossibleVersus
            .OrderBy(v => v.LevelDiff)];

        int minLevelDiff = allSplitsOrdered.First().LevelDiff;
        logger.LogInformation("Min level diff: {MinLevelDiff}", minLevelDiff);

        HashSet<Versus> topSplits = [.. allSplitsOrdered
            .Where(v => v.LevelDiff == minLevelDiff)];

        logger.LogInformation("{TopSplitCount} splits with min level diff", topSplits.Count);

        return topSplits;
    }

    private void ValidatePlayers(HashSet<Player> players)
    {
        if (players.Count <= 0 || players.Count % 2 != 0) throw new ArgumentException("El número de jugadores debe ser par y mayor que cero.");
    }

    public HashSet<Versus> GenerateAllVersus(HashSet<Player> players)
    {
        logger.LogInformation("Generating all possible teams for {PlayerCount} players...", players.Count);
        HashSet<Team> allPossibleTeams = GenerateAllPossibleTeams(players, players.Count / 2);
        logger.LogInformation("{TeamCount} teams generated.", allPossibleTeams.Count);

        logger.LogInformation("Completing teams with rivals...");
        var withRivals = CompleteWithRivals(allPossibleTeams, players);
        logger.LogInformation("{VersusCount} versus generated.", withRivals.Count);
        return withRivals;
    }

    public HashSet<Team> GenerateAllPossibleTeams(HashSet<Player> players, int numPlayersPerTeam)
    {
        // Caso base: si el número de jugadores a elegir es igual al total, solo hay un equipo posible
        if (players.Count == numPlayersPerTeam)
            return [new Team(players)];

        // Caso base: si solo hay que elegir uno, cada jugador es un equipo
        if (numPlayersPerTeam == 1)
            return [.. players.Select(p => new Team([p]))];

        var result = new HashSet<Team>();

        // Convierte a lista para acceso por índice y evitar enumeraciones repetidas
        var playersList = players.ToList();

        for (int i = 0; i <= playersList.Count - numPlayersPerTeam; i++)
        {
            var current = playersList[i];
            // Crea un subconjunto con los jugadores restantes
            var remaining = new HashSet<Player>(playersList.Skip(i + 1));
            // Llama recursivamente para los siguientes jugadores
            foreach (var subTeam in GenerateAllPossibleTeams(remaining, numPlayersPerTeam - 1))
            {
                var teamPlayers = new HashSet<Player>(subTeam.Players) { current };
                result.Add(new Team(teamPlayers));
            }
        }

        return result;
    }

    public HashSet<Versus> CompleteWithRivals(HashSet<Team> allPossibleTeams, HashSet<Player> allPlayers)
    {
        return [..allPossibleTeams
            .Select(team1 =>
            {
                HashSet<Player> remainingPlayers = [.. allPlayers];
                remainingPlayers.ExceptWith(team1.Players);

                return new Versus
                {
                    Team1 = team1,
                    Team2 = new Team(remainingPlayers)
                };
            })];
    }
}