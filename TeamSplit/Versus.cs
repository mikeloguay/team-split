namespace TeamSplit;

public class Versus
{
    public required Team Team1 { get; init; }
    public required Team Team2 { get; init; }
    public int LevelDiff => Math.Abs(Team1.Level - Team2.Level);

    public override string ToString() => $"{Team1} \n {Team2}";
}