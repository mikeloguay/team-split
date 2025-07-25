﻿using Microsoft.Extensions.Logging;
using Moq;

namespace TeamSplit.Test;

public class TeamSplitterTest
{
    private readonly ITeamSplitter _teamSplitter;
    private readonly Mock<ILogger<TeamSplitter>> _loggerMock = new();

    public TeamSplitterTest()
    {
        _teamSplitter = new TeamSplitter(_loggerMock.Object);
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

        var exception = Assert.Throws<ArgumentException>(() => _teamSplitter.BestSplitRandomFromTops(players));
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

        Versus teamsCombination = _teamSplitter.BestSplitRandomFromTops(players);
        Assert.Equal(2, teamsCombination.Team1.Players.Count);
        Assert.Equal(2, teamsCombination.Team2.Players.Count);
    }

    [Fact]
    public void FourPlayers_Split_OK()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
            new Player { Name = "Roberto", Level = 10},
        ];

        Versus teamsCombination = _teamSplitter.BestSplitRandomFromTops(players);
        Assert.Equal(players.Count / 2, teamsCombination.Team1.Players.Count);
        Assert.Equal(players.Count / 2, teamsCombination.Team2.Players.Count);

        Team canijoTeam = teamsCombination.Team1.Players.Any(p => p.Name == "Canijo") ?
            teamsCombination.Team1 :
            teamsCombination.Team2;
        Assert.Contains(canijoTeam.Players, p => p.Name == "Roberto");
    }

    [Fact]
    public void TenPlayers_Split_OK()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Roberto", Level = 10},
            new Player { Name = "Ale1", Level = 50},
            new Player { Name = "Ale2", Level = 50},
            new Player { Name = "Ale3", Level = 50},
            new Player { Name = "Ale4", Level = 50},
            new Player { Name = "Ale5", Level = 50},
            new Player { Name = "Ale6", Level = 50},
            new Player { Name = "Ale7", Level = 50},
            new Player { Name = "Ale8", Level = 50},
        ];

        Versus teamsCombination = _teamSplitter.BestSplitRandomFromTops(players);
        Assert.Equal(players.Count / 2, teamsCombination.Team1.Players.Count);
        Assert.Equal(players.Count / 2, teamsCombination.Team2.Players.Count);

        Team canijoTeam = teamsCombination.Team1.Players.Any(p => p.Name == "Canijo") ?
            teamsCombination.Team1 :
            teamsCombination.Team2;
        Assert.Contains(canijoTeam.Players, p => p.Name == "Roberto");
    }

    [Fact]
    public void TwelvePlayers_Split_OK()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Roberto", Level = 10},
            new Player { Name = "Ale1", Level = 50},
            new Player { Name = "Ale2", Level = 50},
            new Player { Name = "Ale3", Level = 50},
            new Player { Name = "Ale4", Level = 50},
            new Player { Name = "Ale5", Level = 50},
            new Player { Name = "Ale6", Level = 50},
            new Player { Name = "Ale7", Level = 50},
            new Player { Name = "Ale8", Level = 50},
            new Player { Name = "Ale9", Level = 50},
            new Player { Name = "Ale10", Level = 50},
        ];

        Versus teamsCombination = _teamSplitter.BestSplitRandomFromTops(players);
        Assert.Equal(players.Count / 2, teamsCombination.Team1.Players.Count);
        Assert.Equal(players.Count / 2, teamsCombination.Team2.Players.Count);

        Team canijoTeam = teamsCombination.Team1.Players.Any(p => p.Name == "Canijo") ?
            teamsCombination.Team1 :
            teamsCombination.Team2;
        Assert.Contains(canijoTeam.Players, p => p.Name == "Roberto");
    }

    [Fact]
    public void FourteenPlayers_Split_OK()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Roberto", Level = 10},
            new Player { Name = "Ale1", Level = 50},
            new Player { Name = "Ale2", Level = 50},
            new Player { Name = "Ale3", Level = 50},
            new Player { Name = "Ale4", Level = 50},
            new Player { Name = "Ale5", Level = 50},
            new Player { Name = "Ale6", Level = 50},
            new Player { Name = "Ale7", Level = 50},
            new Player { Name = "Ale8", Level = 50},
            new Player { Name = "Ale9", Level = 50},
            new Player { Name = "Ale10", Level = 50},
            new Player { Name = "Ale11", Level = 50},
            new Player { Name = "Ale12", Level = 50},
        ];

        Versus teamsCombination = _teamSplitter.BestSplitRandomFromTops(players);
        Assert.Equal(players.Count / 2, teamsCombination.Team1.Players.Count);
        Assert.Equal(players.Count / 2, teamsCombination.Team2.Players.Count);

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
    public void TwelvePlayersBy6_GenerateAllPossibleTeams_924()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Player01", Level = 1},
            new Player { Name = "Player02", Level = 5},
            new Player { Name = "Player03", Level = 10},
            new Player { Name = "Player04", Level = 15},
            new Player { Name = "Player05", Level = 20},
            new Player { Name = "Player06", Level = 25},
            new Player { Name = "Player07", Level = 30},
            new Player { Name = "Player08", Level = 35},
            new Player { Name = "Player09", Level = 40},
            new Player { Name = "Player10", Level = 45},
            new Player { Name = "Player11", Level = 50},
            new Player { Name = "Player12", Level = 55},
        ];

        HashSet<Team> teams = _teamSplitter.GenerateAllPossibleTeams(players, 6);
        Assert.Equal(924, teams.Count);
    }

    [Fact]
    public void FourteenPlayersBy7_GenerateAllPossibleTeams_924()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Player01", Level = 1},
            new Player { Name = "Player02", Level = 5},
            new Player { Name = "Player03", Level = 10},
            new Player { Name = "Player04", Level = 15},
            new Player { Name = "Player05", Level = 20},
            new Player { Name = "Player06", Level = 25},
            new Player { Name = "Player07", Level = 30},
            new Player { Name = "Player08", Level = 35},
            new Player { Name = "Player09", Level = 40},
            new Player { Name = "Player10", Level = 45},
            new Player { Name = "Player11", Level = 50},
            new Player { Name = "Player12", Level = 55},
            new Player { Name = "Player13", Level = 60},
            new Player { Name = "Player14", Level = 65},
        ];

        HashSet<Team> teams = _teamSplitter.GenerateAllPossibleTeams(players, 7);
        Assert.Equal(3432, teams.Count);
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

        HashSet<Versus> versusList = _teamSplitter.TopSplits(players);
        Assert.Equal([.. versusList.OrderBy(v => v.LevelDiff)], versusList);
    }

    [Fact]
    public void FourPlayers_GenerateAllVersus_3NoRepeated()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100},
            new Player { Name = "Ale", Level = 50},
            new Player { Name = "Antonio", Level = 50},
            new Player { Name = "Roberto", Level = 10},
        ];

        HashSet<Versus> versusList = _teamSplitter.GenerateAllVersus(players);
        Assert.Equal(3, versusList.Count);
    }

    [Fact]
    public void TenPlayers_GenerateAllVersus_5NoRepeated()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Roberto", Level = 1 },
            new Player { Name = "Ale", Level = 80 },
            new Player { Name = "Miki", Level = 78 },
            new Player { Name = "Antonio", Level = 25 },
            new Player { Name = "Canijo", Level = 100 },
            new Player { Name = "Dani", Level = 75 },
            new Player { Name = "Jose", Level = 78 },
            new Player { Name = "DaniJ", Level = 65 },
            new Player { Name = "Juani", Level = 50 },
            new Player { Name = "Pablo", Level = 42 }
    ];

        HashSet<Versus> versusList = _teamSplitter.GenerateAllVersus(players);
        Assert.Equal(126, versusList.Count);
    }

    [Fact]
    public void TwoTeamsDifferentOrder_Equals_Same()
    {
        Team team1 = new()
        {
            Players =
            [
                new Player { Name = "Canijo", Level = 100 },
                new Player { Name = "Ale", Level = 50 }
            ]
        };

        Team team2 = new()
        {
            Players =
            [
                new Player { Name = "Ale", Level = 50 },
                new Player { Name = "Canijo", Level = 100 }
            ]
        };

        Assert.Equal(team1, team2);
    }

    [Fact]
    public void TwoVersusDifferentOrder_Equals_Same()
    {
        Team team1 = new()
        {
            Players =
            [
                new Player { Name = "Canijo", Level = 100 },
                new Player { Name = "Ale", Level = 50 }
            ]
        };

        Team team2 = new()
        {
            Players =
            [
                new Player { Name = "Ale", Level = 50 },
                new Player { Name = "Canijo", Level = 100 }
            ]
        };

        Versus versus1 = new()
        {
            Team1 = team1,
            Team2 = team2
        };

        Versus versus2 = new()
        {
            Team1 = team2,
            Team2 = team1
        };

        Versus versus3 = new()
        {
            Team1 = new Team(team1),
            Team2 = new Team(team2)
        };

        Assert.Equal(versus1, versus2);
        Assert.Equal(versus1, versus3);
        Assert.Equal(versus3, versus2);
    }

    [Fact]
    public void HappyPath_CompleteWithRivals_OK()
    {
        HashSet<Team> allPossibleTeams =
        [
            new Team
            {
                Players =
                [
                    new Player { Name = "Canijo", Level = 100 },
                    new Player { Name = "Ale", Level = 50 }
                ]
            },
            new Team
            {
                Players =
                [
                    new Player { Name = "Antonio", Level = 50 },
                    new Player { Name = "Roberto", Level = 10 }
                ]
            }
        ];

        HashSet<Player> allPlayers =
        [
            new Player { Name = "Canijo", Level = 100 },
            new Player { Name = "Ale", Level = 50 },
            new Player { Name = "Antonio", Level = 50 },
            new Player { Name = "Roberto", Level = 10 }
        ];

        HashSet<Versus> versusList = _teamSplitter.CompleteWithRivals(allPossibleTeams, allPlayers);
        Assert.Single(versusList);
    }

    [Fact]
    public void RepeatedPlayers_Set_DoesNotAllowRepeated()
    {
        HashSet<Player> players =
        [
            new Player { Name = "Canijo", Level = 100 },
            new Player { Name = "Ale", Level = 50 },
            new Player { Name = "Ale", Level = 50 },
        ];
        players.Add(new Player { Name = "Ale", Level = 50 });

        Assert.Equal(2, players.Count);
    }

    [Fact]
    public void RepeatedTeams_Set_DoesNotAllowRepeated()
    {
        HashSet<Team> teams =
        [
            new Team
            {
                Players =
                [
                    new Player { Name = "Canijo", Level = 100 },
                    new Player { Name = "Ale", Level = 50 }
                ]
            },
            new Team
            {
                Players =
                [
                    new Player { Name = "Antonio", Level = 50 },
                    new Player { Name = "Roberto", Level = 10 }
                ]
            },
            new Team
            {
                Players =
                [
                    new Player { Name = "Antonio", Level = 50 },
                    new Player { Name = "Roberto", Level = 10 }
                ]
            },
            new Team
            {
                Players =
                [
                    new Player { Name = "Roberto", Level = 10 },
                    new Player { Name = "Antonio", Level = 50 },
                ]
            }
        ];

        Assert.Equal(2, teams.Count);
    }

    [Fact]
    public void TwoVersusDifferentOrders_Equals_Same()
    {
        Versus versus1 = new()
        {
            Team1 = new()
            {
                Players = [
                new Player { Name = "Ale", Level = 80 },
                new Player { Name = "Roberto", Level = 1 },
            ]
            },
            Team2 = new()
            {
                Players = [
                new Player { Name = "Diego", Level = 50 },
                new Player { Name = "Miki", Level = 75 },
            ]
            },
        };

        Versus versus2 = new()
        {
            Team1 = new()
            {
                Players = [
                new Player { Name = "Miki", Level = 75 },
                new Player { Name = "Diego", Level = 50 },
            ]
            },
            Team2 = new()
            {
                Players = [
                new Player { Name = "Roberto", Level = 1 },
                new Player { Name = "Ale", Level = 80 },
            ]
            },
        };

        Assert.Equal(versus1.GetHashCode(), versus2.GetHashCode());
    }

    [Fact]
    public void TwoPlayersSame_HashCode_Equals()
    {
        Player player1 = new() { Name = "Ale", Level = 80 };
        Player player2 = new() { Name = "Ale", Level = 80 };

        Assert.Equal(player1.GetHashCode(), player2.GetHashCode());
        Assert.Equal(player1, player2);
    }

    [Fact]
    public void TwoTeamsSame_HashCode_Equals()
    {
        Team team1 = new()
        {
            Players =
            [
                new Player { Name = "Ale", Level = 80 },
                new Player { Name = "Roberto", Level = 1 },
            ]
        };

        Team team2 = new()
        {
            Players =
            [
                new Player { Name = "Roberto", Level = 1 },
                new Player { Name = "Ale", Level = 80 },
            ]
        };

        Assert.Equal(team1.GetHashCode(), team2.GetHashCode());
        Assert.Equal(team1, team2);
    }
}
