

using Microsoft.Extensions.Logging;

namespace TeamSplit;

public class TeamSplitter(ILogger<TeamSplitter> logger) : ITeamSplitter
{
    public Versus BestSplit(HashSet<Player> players) => TopSplits(players, 1).First();

    public HashSet<Versus> TopSplits(HashSet<Player> players, int numSplits)
    {
        logger.LogInformation("Generating splits for {PlayerCount} players...", players.Count);
        ValidatePlayers(players);
        HashSet<Versus> allPossibleVersus = GenerateAllVersus(players, players.Count / 2);

        return [.. allPossibleVersus
            .OrderBy(v => v.LevelDiff)
            .Take(numSplits)];
    }

    private void ValidatePlayers(HashSet<Player> players)
    {
        if (players.Count % 2 != 0) throw new ArgumentException("Number of players must be even");
    }

    private HashSet<Versus> GenerateAllVersus(HashSet<Player> players, int numPlayersPerTeam)
    {
        HashSet<Team> allPossibleTeams = GenerateAllPossibleTeams(players, numPlayersPerTeam);
        return CompleteWithRivals(allPossibleTeams, players, numPlayersPerTeam);
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

    private HashSet<Versus> CompleteWithRivals(HashSet<Team> allPossibleTeams, HashSet<Player> allPlayers, int numPlayersPerTeam) => [..allPossibleTeams
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