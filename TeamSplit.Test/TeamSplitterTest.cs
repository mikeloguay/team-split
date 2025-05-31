namespace TeamSplit.Test;

public class TeamSplitterTest
{
    private readonly ITeamSplitter _teamSplitter;

    public TeamSplitterTest()
    {
        _teamSplitter = new TeamSplitter();
    }

    [Fact]
    public void OddNumPlayer_Split_Error()
    {
        List<Player> players =
        [
            new Player { Name = "Miki", Level = 10},
            new Player { Name = "Ale", Level = 10},
            new Player { Name = "Antonio", Level = 10},
        ];

        var exception = Assert.Throws<ArgumentException>(() => _teamSplitter.Split(players, "Team A", "Team B"));
    }

    [Fact]
    public void EvenNumPlayer_Split_OK()
    {
        List<Player> players =
        [
            new Player { Name = "Miki", Level = 10},
            new Player { Name = "Ale", Level = 10},
            new Player { Name = "Antonio", Level = 10},
            new Player { Name = "Dani", Level = 10},
        ];

        Versus teamsCombination = _teamSplitter.Split(players, "Team A", "Team B");
        Assert.Equal("Team A", teamsCombination.Team1.Name);
        Assert.Equal("Team B", teamsCombination.Team2.Name);
        Assert.Equal(2, teamsCombination.Team1.Players.Count);
        Assert.Equal(2, teamsCombination.Team2.Players.Count);
    }

    [Fact]
    public void EasySplit_Split_OK()
    {
        List<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
            new Player { Name = "Roberto", Level = 10},
        ];

        Versus teamsCombination = _teamSplitter.Split(players, "Team A", "Team B");
        Assert.Equal("Team A", teamsCombination.Team1.Name);
        Assert.Equal(2, teamsCombination.Team1.Players.Count);
        Assert.Equal("Team B", teamsCombination.Team2.Name);
        Assert.Equal(2, teamsCombination.Team2.Players.Count);

        Team canijoTeam = teamsCombination.Team1.Players.Any(p => p.Name == "Canijo") ?
            teamsCombination.Team1 :
            teamsCombination.Team2;
        Assert.Contains(canijoTeam.Players, p => p.Name == "Roberto");
    }

    [Fact]
    public void HappyPath_GenerateAllPossibleTeams_OK()
    {
        List<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
            new Player { Name = "Roberto", Level = 10},
        ];

        List<Team> teams = _teamSplitter.GenerateAllPossibleTeams(players, 2);
        Assert.Equal(6, teams.Count);
    }
}
