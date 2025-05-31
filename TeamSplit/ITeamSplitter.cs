namespace TeamSplit;

public interface ITeamSplitter
{
    Versus Split(HashSet<Player> players);
    HashSet<Team> GenerateAllPossibleTeams(HashSet<Player> players, int numPlayersPerTeam);
}