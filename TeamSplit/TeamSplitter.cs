

namespace TeamSplit;

public class TeamSplitter : ITeamSplitter
{
    public Versus Split(HashSet<Player> players)
    {
        ValidatePlayers(players);

        HashSet<Versus> allPossibleVersus = GenerateAllVersus(players, players.Count / 2);

        return allPossibleVersus
            .OrderBy(v => v.LevelDiff)
            .First();  
    }

    private void ValidatePlayers(HashSet<Player> players)
    {
        if (players.Count % 2 != 0)
        {
            throw new ArgumentException("The number of players must be even to split into two teams.");
        }
    }

    private HashSet<Versus> GenerateAllVersus(HashSet<Player> players, int numPlayersPerTeam)
    {
        HashSet<Team> allPossibleTeams = GenerateAllPossibleTeams(players, numPlayersPerTeam);
        return CompleteWithRivals(allPossibleTeams, players, numPlayersPerTeam);
    }

    public HashSet<Team> GenerateAllPossibleTeams(HashSet<Player> players, int numPlayersPerTeam)
    {
        if (numPlayersPerTeam == 0) return [];
        if (numPlayersPerTeam == 1) return [.. players.Select(p => new Team { Players = [p] })];

        HashSet<Team> result = [];

        for (int i = 0; i < players.Count; i++)
        {
            var currentPlayer = players.ElementAt(i);
            HashSet<Player> playersReduced = [.. players.Where(p => p != currentPlayer)];
            HashSet<Team> allPossibleTeams = GenerateAllPossibleTeams(playersReduced, numPlayersPerTeam - 1);
            result.UnionWith(allPossibleTeams.Select(t => new Team(t.AddPlayer(currentPlayer))));
        }

        return result;
    }

    private HashSet<Versus> CompleteWithRivals(HashSet<Team> allPossibleTeams, HashSet<Player> allPlayers, int numPlayersPerTeam) => [..allPossibleTeams
        .Select(team1 =>
        {
            List<Player> remainingPlayers = [.. allPlayers.Except(team1.Players)];
            return new Versus
            {
                Team1 = team1,
                Team2 = new Team
                {
                    Players = remainingPlayers
                }
            };
        })];
}