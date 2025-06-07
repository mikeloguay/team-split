namespace TeamSplit.Api;

public readonly record struct SplitResponse(HashSet<VersusResponse> Versuses);
public readonly record struct VersusResponse(TeamResponse Team1, TeamResponse Team2);
public readonly record struct TeamResponse(string Name, HashSet<string> Players);