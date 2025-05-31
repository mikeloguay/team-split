namespace TeamSplit;

public class Player : IEquatable<Player>
{
    public required string Name { get; set; }
    public required int Level { get; set; }

    public bool Equals(Player? other) =>
        other is not null &&
        Name == other.Name &&
        Level == other.Level;
        
    public override bool Equals(object? obj) => Equals((Player?)obj);
    public override int GetHashCode() =>
        HashCode.Combine(Name, Level);
}