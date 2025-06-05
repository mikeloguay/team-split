namespace TeamSplit;

public class Team : IEquatable<Team>
{
    public Team(Team other) { Players = [.. other.Players]; }

    public Team(HashSet<Player> players) { Players = [.. players]; }

    public Team() { }

    public HashSet<Player> Players { get; init; } = [];
    public int Level => Players.Sum(p => p.Level);

    public Team AddPlayer(Player player)
    {
        Players.Add(player);
        return this;
    }

    public override string ToString() => $"[{string.Join(", ", Players.Select(p => p.ToString()))}]";

    public bool Equals(Team? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Players.Count != other.Players.Count) return false;

        return Players.SetEquals(other.Players);
    }

    public override bool Equals(object? obj) => Equals((Team?)obj);

    public override int GetHashCode() => Players.Aggregate(0, (hash, player) => hash ^ player.GetHashCode());
}
