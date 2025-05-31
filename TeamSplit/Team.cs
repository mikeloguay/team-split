namespace TeamSplit;

public class Team
{
    public string Name { get; init; }
    public required List<Player> Players { get; init; }
    public int Level => Players.Sum(p => p.Level);

    public Team AddPlayer(Player player)
    {
        Players.Add(player);
        return this;
    }
}
