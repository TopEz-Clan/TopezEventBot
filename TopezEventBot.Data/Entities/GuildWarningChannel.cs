namespace TopezEventBot.Data.Entities;

public class GuildWarningChannel
{
    public Guid Id { get; set; }
    public ulong GuildId { get; set; }
    public ulong WarningChannelId { get; set; }
}