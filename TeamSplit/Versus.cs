namespace TeamSplit;

public class Versus
{
    public required Team Team1 { get; init; }
    public required Team Team2 { get; init; }
    public int LevelDiff => Math.Abs(Team1.Level - Team2.Level);

    public override string ToString() => $"Con petos: {Team1}{Environment.NewLine}"
        + $"Sin petos: {Team2}{Environment.NewLine}"
        + $"Diferencia de nivel: {LevelDiff}";
}