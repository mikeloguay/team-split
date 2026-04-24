namespace TeamSplit;

public interface ITeamSplitter
{
    Versus BestSplitRandomFromTops(HashSet<Player> players);
    HashSet<Versus> TopSplits(HashSet<Player> players);
}
