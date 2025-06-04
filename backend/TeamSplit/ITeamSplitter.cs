namespace TeamSplit;

public interface ITeamSplitter
{
    Versus BestSplitRandomFromTops(HashSet<Player> players);
    HashSet<Versus> TopSplits(HashSet<Player> players);
    HashSet<Team> GenerateAllPossibleTeams(HashSet<Player> players, int numPlayersPerTeam);
    HashSet<Versus> GenerateAllVersus(HashSet<Player> players, int numPlayersPerTeam);
    HashSet<Versus> CompleteWithRivals(HashSet<Team> allPossibleTeams, HashSet<Player> allPlayers, int numPlayersPerTeam);
}