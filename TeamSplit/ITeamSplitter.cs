namespace TeamSplit;

public interface ITeamSplitter
{
    Versus BestSplit(HashSet<Player> players);
    HashSet<Versus> TopSplits(HashSet<Player> players, int numSplits);
    HashSet<Team> GenerateAllPossibleTeams(HashSet<Player> players, int numPlayersPerTeam);
}