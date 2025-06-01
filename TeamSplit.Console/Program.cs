using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

var services = new ServiceCollection();
services.AddLogging(config => config.AddConsole());
services.AddTransient<ITeamSplitter, TeamSplitter>();
var provider = services.BuildServiceProvider();
ITeamSplitter teamSplitter = provider.GetRequiredService<ITeamSplitter>();

int numSplits = 3;
HashSet<Versus> versuses = teamSplitter.TopSplits([.. players], numSplits);
Console.WriteLine($"{numSplits} Posibles opciones:");
Console.WriteLine("----------------------------------");
Console.WriteLine();
Console.WriteLine(string.Join($"{Environment.NewLine}{Environment.NewLine}",
    versuses.Select(v => v.ToString())));