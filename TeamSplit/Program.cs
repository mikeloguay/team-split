using TeamSplit;

List<Player> players =
[
    new Player { Name = "Roberto", Level = 1},
    new Player { Name = "Diego", Level = 60},
    new Player { Name = "Ale", Level = 80},
    new Player { Name = "Miki", Level = 80},
    new Player { Name = "Antonio", Level = 20},
    new Player { Name = "Canijo", Level = 100},
    new Player { Name = "Dani", Level = 75},
    new Player { Name = "Tito", Level = 40},
    new Player { Name = "Jose", Level = 75},
    new Player { Name = "DaniJ", Level = 60},
];

ITeamSplitter teamSplitter = new TeamSplitter();
Versus versus = teamSplitter.Split([.. players]);

Console.WriteLine(versus);