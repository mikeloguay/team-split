namespace TeamSplit;

public interface ITeamSplitter
{
    Versus BestSplitRandomFromTops(HashSet<string> playerNames);
    Versus BestSplitRandomFromTops(HashSet<Player> players);
    HashSet<Versus> TopSplits(HashSet<Player> players);
    HashSet<Team> GenerateAllPossibleTeams(HashSet<Player> players, int numPlayersPerTeam);
    HashSet<Versus> GenerateAllVersus(HashSet<Player> players);
    HashSet<Versus> CompleteWithRivals(HashSet<Team> allPossibleTeams, HashSet<Player> allPlayers);
}