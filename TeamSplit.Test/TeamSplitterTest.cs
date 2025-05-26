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

        var (team1, team2) = _teamSplitter.Split(players, "Team A", "Team B");
        Assert.Equal("Team A", team1.Name);
        Assert.Equal("Team B", team2.Name);
        Assert.Equal(2, team1.Players.Count);
        Assert.Equal(2, team2.Players.Count);
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

        var (team1, team2) = _teamSplitter.Split(players, "Team A", "Team B");
        Assert.Equal("Team A", team1.Name);
        Assert.Equal(2, team1.Players.Count);
        Assert.Equal("Team B", team2.Name);
        Assert.Equal(2, team2.Players.Count);

        Team canijoTeam = team1.Players.Any(p => p.Name == "Canijo") ? team1 : team2;
        Assert.Contains(canijoTeam.Players, p => p.Name == "Roberto");
    }
}
