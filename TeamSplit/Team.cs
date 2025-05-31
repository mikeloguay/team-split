namespace TeamSplit;

public class Team : IEquatable<Team>
{
    public Team(Team other)
    {
        Name = other.Name;
        Players = [.. other.Players];
    }

    public Team() { }

    public string? Name { get; init; }
    public List<Player> Players { get; init; } = [];
    public int Level => Players.Sum(p => p.Level);

    public Team AddPlayer(Player player)
    {
        Players.Add(player);
        return this;
    }

    public override string ToString() => $"{Name}: [{string.Join(", ", Players.Select(p => p.Name))}]";

    public bool Equals(Team? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Players.Count != other.Players.Count) return false;

        var thisPlayers = Players.Select(p => p.Name).OrderBy(n => n);
        var otherPlayers = other.Players.Select(p => p.Name).OrderBy(n => n);

        return thisPlayers.SequenceEqual(otherPlayers);
    }

    public override bool Equals(object? obj) => Equals((Team?)obj);

    public override int GetHashCode() => 
        HashCode.Combine(Name,
            HashCode.Combine(Players
                                .Select(p => p.Name)
                                .OrderBy(n => n)
                                .Aggregate(0, (h, n) => HashCode.Combine(h, n))));
}
