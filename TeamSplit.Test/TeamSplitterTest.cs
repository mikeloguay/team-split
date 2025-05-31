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
        HashSet<Player> players =
        [
            new Player { Name = "Miki", Level = 10},
            new Player { Name = "Ale", Level = 10},
            new Player { Name = "Antonio", Level = 10},
        ];

        var exception = Assert.Throws<ArgumentException>(() => _teamSplitter.BestSplit(players));
    }

    [Fact]
    public void EvenNumPlayer_Split_OK()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Miki", Level = 10},
            new Player { Name = "Ale", Level = 10},
            new Player { Name = "Antonio", Level = 10},
            new Player { Name = "Dani", Level = 10},
        ];

        Versus teamsCombination = _teamSplitter.BestSplit(players);
        Assert.Equal(2, teamsCombination.Team1.Players.Count);
        Assert.Equal(2, teamsCombination.Team2.Players.Count);
    }

    [Fact]
    public void EasySplit_Split_OK()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
            new Player { Name = "Roberto", Level = 10},
        ];

        Versus teamsCombination = _teamSplitter.BestSplit(players);
        Assert.Equal(2, teamsCombination.Team1.Players.Count);
        Assert.Equal(2, teamsCombination.Team2.Players.Count);

        Team canijoTeam = teamsCombination.Team1.Players.Any(p => p.Name == "Canijo") ?
            teamsCombination.Team1 :
            teamsCombination.Team2;
        Assert.Contains(canijoTeam.Players, p => p.Name == "Roberto");
    }

    [Fact]
    public void ThreePlayersBy2_GenerateAllPossibleTeams_3()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
        ];

        HashSet<Team> teams = _teamSplitter.GenerateAllPossibleTeams(players, 2);
        Assert.Equal(3, teams.Count);
    }

    [Fact]
    public void FourPlayersBy2_GenerateAllPossibleTeams_6()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
            new Player { Name = "Roberto", Level = 10},
        ];

        HashSet<Team> teams = _teamSplitter.GenerateAllPossibleTeams(players, 2);
        Assert.Equal(6, teams.Count);
    }

    [Fact]
    public void FivePlayersBy3_GenerateAllPossibleTeams_6()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
            new Player { Name = "Roberto", Level = 10},
            new Player { Name = "Tito", Level = 10},
        ];

        HashSet<Team> teams = _teamSplitter.GenerateAllPossibleTeams(players, 3);
        Assert.Equal(10, teams.Count);
    }

    [Fact]
    public void EasySplit_Top6Splits_AscendingDiffs()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
            new Player { Name = "Roberto", Level = 10},
        ];

        HashSet<Versus> versusList = _teamSplitter.TopSplits(players, 6);
        Assert.Equal([..versusList.OrderBy(v => v.LevelDiff)], versusList);
    }
}
