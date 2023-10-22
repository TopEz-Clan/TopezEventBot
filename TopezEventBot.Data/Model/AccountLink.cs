namespace TopezEventBot.Data.Model;

public class AccountLink
{
    public Guid Id { get; set; }
    public ulong DiscordMemberId { get; set; }
    public string RunescapeName { get; set; }
}