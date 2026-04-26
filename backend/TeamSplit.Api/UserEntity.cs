namespace TeamSplit.Api;

public class UserEntity
{
    public required string Id { get; set; }    // Google "sub" claim
    public required string Email { get; set; }
    public required string Name { get; set; }
}
