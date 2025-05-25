namespace TeamSplit;

public interface ITeamSplitter
{
    (Team Team1, Team Team2) Split(List<Player> players, string teamName1, string teamName2);
}