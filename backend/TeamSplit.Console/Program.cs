using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamSplit;

var services = new ServiceCollection();
services.AddLogging(config => config.AddConsole());
services.AddTransient<ITeamSplitter, TeamSplitter>();
var provider = services.BuildServiceProvider();
ITeamSplitter teamSplitter = provider.GetRequiredService<ITeamSplitter>();

Versus versus = teamSplitter.BestSplitRandomFromTops([
      "Roberto",
      "Diego",
      "Ale",
      "Miki",
      "Antonio",
      "Canijo",
      "Dani",
      "Tito",
      "Jose",
      "DaniJ"
  ] );

Console.WriteLine("Pulsa una tecla para salir...");
Console.ReadLine();