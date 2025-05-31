

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
        List<Team> allPossibleTeams = GenerateAllPossibleTeams(players, numPlayersPerTeam);
        return CompleteWithRivals(allPossibleTeams, players, numPlayersPerTeam);
    }

    public List<Team> GenerateAllPossibleTeams(List<Player> players, int numPlayersPerTeam)
    {
        if (numPlayersPerTeam == 0) return [];
        if (numPlayersPerTeam == 1) return [.. players.Select(p => new Team { Players = [p] })];

        List<Team> result = [];

        var firstPlayer = players[0];
        players.RemoveAt(0);

        List<Team> allPossibleTeams = GenerateAllPossibleTeams(players, numPlayersPerTeam - 1);

        foreach (var team in allPossibleTeams)
        {
            result.Add(team.AddPlayer(firstPlayer));
        }

        return result;
    }

    private List<Versus> CompleteWithRivals(List<Team> allPossibleTeams, List<Player> players, int numPlayersPerTeam)
    {
        throw new NotImplementedException();
    }
}