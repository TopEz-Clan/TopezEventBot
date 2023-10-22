namespace TopezEventBot.Data.Model;

public class AccountLink
{
    public Guid Id { get; set; }
    public long DiscordMemberId { get; set; }
    public string RunescapeName { get; set; }
}