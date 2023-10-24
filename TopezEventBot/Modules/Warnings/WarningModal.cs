using System.Net.Mime;
using Discord;
using Discord.Interactions;

namespace TopezEventBot.Modules.Warnings;

public class WarningModal : IModal
{
    public string Title { get; } = "Issue warning";
    
    [InputLabel("Reason")]
    [RequiredInput]
    [ModalTextInput("warning_reason", TextInputStyle.Paragraph)]
    public string Reason { get; set; }
    
    public ulong MemberId { get; set; }
}