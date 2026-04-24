using Microsoft.Extensions.Logging;

namespace TeamSplit;

public class TeamSplitter(ILogger<TeamSplitter> logger) : ITeamSplitter
{
    public Versus BestSplitRandomFromTops(HashSet<Player> players)
    {
        var tops = TopSplits(players);
        var best = tops.ElementAt(Random.Shared.Next(tops.Count));
        logger.LogInformation("Best split: {Split}", best);
        return best;
    }

    public HashSet<Versus> TopSplits(HashSet<Player> players)
    {
        Validate(players);
        logger.LogInformation("Splitting {Count} players", players.Count);

        var arr = players.ToArray();
        int n = arr.Length;
        int half = n / 2;
        int total = arr.Sum(p => p.Level);

        // dp[i,j,s] = true if we can pick exactly j players from arr[0..i-1] with sum s
        bool[,,] dp = BuildDP(arr, half, total);

        // Find minimum achievable diff by scanning s in [0, total/2]
        // (for s <= total/2, diff = total - 2*s >= 0 and decreases as s grows)
        int minDiff = int.MaxValue;
        for (int s = total / 2; s >= 0; s--)
        {
            if (dp[n, half, s])
            {
                minDiff = total - 2 * s;
                break;
            }
        }

        // Collect all optimal partitions; restrict to s <= total/2 to avoid
        // generating mirror splits (T1 vs T2 and T2 vs T1 are the same Versus)
        var result = new HashSet<Versus>();
        var selected = new bool[n];
        for (int s = 0; s <= total / 2; s++)
            if (dp[n, half, s] && total - 2 * s == minDiff)
                Backtrack(arr, dp, n, half, s, selected, result);

        logger.LogInformation("Found {Count} optimal splits with diff {Diff}", result.Count, minDiff);
        return result;
    }

    private static bool[,,] BuildDP(Player[] arr, int half, int total)
    {
        int n = arr.Length;
        var dp = new bool[n + 1, half + 1, total + 1];
        dp[0, 0, 0] = true;

        for (int i = 0; i < n; i++)
        {
            int level = arr[i].Level;
            for (int j = 0; j <= half; j++)
            {
                for (int s = 0; s <= total; s++)
                {
                    if (!dp[i, j, s]) continue;
                    dp[i + 1, j, s] = true;                          // skip arr[i]
                    if (j < half && s + level <= total)
                        dp[i + 1, j + 1, s + level] = true;          // take arr[i]
                }
            }
        }

        return dp;
    }

    // Reconstructs all selections of j players from arr[0..i-1] with sum s
    private static void Backtrack(
        Player[] arr, bool[,,] dp, int i, int j, int s,
        bool[] selected, HashSet<Versus> result)
    {
        if (i == 0)
        {
            var team1 = new HashSet<Player>();
            var team2 = new HashSet<Player>();
            for (int k = 0; k < arr.Length; k++)
                (selected[k] ? team1 : team2).Add(arr[k]);
            result.Add(new Versus { Team1 = new Team(team1), Team2 = new Team(team2) });
            return;
        }

        int level = arr[i - 1].Level;

        if (dp[i - 1, j, s])                                          // arr[i-1] was skipped
            Backtrack(arr, dp, i - 1, j, s, selected, result);

        if (j > 0 && s >= level && dp[i - 1, j - 1, s - level])      // arr[i-1] was taken
        {
            selected[i - 1] = true;
            Backtrack(arr, dp, i - 1, j - 1, s - level, selected, result);
            selected[i - 1] = false;
        }
    }

    private static void Validate(HashSet<Player> players)
    {
        if (players.Count <= 0 || players.Count % 2 != 0)
            throw new ArgumentException("El número de jugadores debe ser par y mayor que cero.");
    }
}
