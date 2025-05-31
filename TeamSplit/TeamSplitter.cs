

namespace TeamSplit;

public class TeamSplitter : ITeamSplitter
{
    public Versus Split(List<Player> players, string teamName1, string teamName2)
    {
        ValidatePlayers(players);

        List<Versus> allPossibleVersus = GenerateAllVersus(players, 5);

        return allPossibleVersus
            .OrderBy(v => v.LevelDiff)
            .First();  
    }

    private void ValidatePlayers(List<Player> players)
    {
        if (players.Count % 2 != 0)
        {
            throw new ArgumentException("The number of players must be even to split into two teams.");
        }
    }

    private List<Versus> GenerateAllVersus(List<Player> players, int numPlayersPerTeam)
    {
        HashSet<Team> allPossibleTeams = GenerateAllPossibleTeams(players, numPlayersPerTeam);
        return CompleteWithRivals(allPossibleTeams, players, numPlayersPerTeam);
    }

    public HashSet<Team> GenerateAllPossibleTeams(List<Player> players, int numPlayersPerTeam)
    {
        if (numPlayersPerTeam == 0) return [];
        if (numPlayersPerTeam == 1) return [.. players.Select(p => new Team { Players = [p] })];

        HashSet<Team> result = [];

        for (int i = 0; i < players.Count; i++)
        {
            var currentPlayer = players[i];
            List<Player> playersReduced = [.. players.Where(p => p != currentPlayer)];
            HashSet<Team> allPossibleTeams = GenerateAllPossibleTeams(playersReduced, numPlayersPerTeam - 1);
            result.UnionWith(allPossibleTeams.Select(t => new Team(t.AddPlayer(currentPlayer))));
        }

        return result;
    }

    private List<Versus> CompleteWithRivals(HashSet<Team> allPossibleTeams, List<Player> players, int numPlayersPerTeam)
    {
        throw new NotImplementedException();
    }
}