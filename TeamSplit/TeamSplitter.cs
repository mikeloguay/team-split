

namespace TeamSplit;

public class TeamSplitter : ITeamSplitter
{
    public (Team Team1, Team Team2) Split(List<Player> players, string teamName1, string teamName2)
    {
        ValidatePlayers(players);

        return (
            new Team
            {
                Name = teamName1,
                Players = [..players.Take(players.Count / 2)]
            },
            new Team
            {
                Name = teamName2,
                Players = [..players.Skip(players.Count / 2).Take(players.Count / 2)]
            }
        );
    }

    private void ValidatePlayers(List<Player> players)
    {
        if (players.Count % 2 != 0)
        {
            throw new ArgumentException("The number of players must be even to split into two teams.");
        }
    }
}