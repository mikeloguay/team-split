namespace TeamSplit;

public interface ITeamSplitter
{
    Versus Split(List<Player> players, string teamName1, string teamName2);
    List<Team> GenerateAllPossibleTeams(List<Player> players, int numPlayersPerTeam);
}