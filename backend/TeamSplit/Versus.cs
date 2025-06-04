namespace TeamSplit;

public class Versus : IEquatable<Versus>
{
    public required Team Team1 { get; init; }
    public required Team Team2 { get; init; }
    public int LevelDiff => Math.Abs(Team1.Level - Team2.Level);

    public bool Equals(Versus? other) =>
        other is not null &&
        (
            Team1.Equals(other.Team1) && Team2.Equals(other.Team2)
            || Team1.Equals(other.Team2) && Team2.Equals(other.Team1)
        );
    
    public override bool Equals(object? obj) => Equals((Versus?)obj);

    public override int GetHashCode() => HashCode.Combine(Team1, Team2);

    public override string ToString() => $"Con petos: {Team1}{Environment.NewLine}"
        + $"Sin petos: {Team2}{Environment.NewLine}";

    public string ToStringWithLevelDiff() => $"Con petos: {Team1}{Environment.NewLine}"
        + $"Sin petos: {Team2}{Environment.NewLine}"
        + $"Diferencia de nivel: {LevelDiff}";
}