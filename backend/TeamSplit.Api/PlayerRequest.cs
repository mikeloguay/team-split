namespace TeamSplit.Api;

public record CreatePlayerRequest(string Name, int Level);
public record UpdatePlayerRequest(int Level);
