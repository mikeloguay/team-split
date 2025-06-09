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

        string splitsMessage = string.Join($"{Environment.NewLine}{Environment.NewLine}", topSplits.Select(v => v.ToString()));
        logger.LogInformation("Splits {Splits}", splitsMessage);

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
        if (numPlayersPerTeam == 1) return [.. players.Select(p => new Team { Players = [p] })];

        HashSet<Team> result = [];

        foreach (Player player in players)
        {
            HashSet<Player> playersReduced = [.. players.Where(p => p != player)];
            HashSet<Team> allPossibleTeams = GenerateAllPossibleTeams(playersReduced, numPlayersPerTeam - 1);
            result.UnionWith(allPossibleTeams.Select(t => new Team(t.AddPlayer(player))));
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