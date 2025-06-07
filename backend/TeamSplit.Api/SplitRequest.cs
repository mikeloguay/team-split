namespace TeamSplit.Api;

public readonly record struct SplitRequest(HashSet<string> Players);