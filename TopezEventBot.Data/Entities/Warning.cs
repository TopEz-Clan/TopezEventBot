namespace TopezEventBot.Data.Entities;

public class Warning
{
    public Guid Id { get; set; }
    public ulong WarnedUser { get; set; }
    public ulong WarnedBy { get; set; }
    public string Reason { get; set; }
}