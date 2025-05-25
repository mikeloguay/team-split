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
}
