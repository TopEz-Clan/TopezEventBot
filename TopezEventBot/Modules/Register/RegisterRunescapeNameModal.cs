using Discord;
using Discord.Interactions;

namespace TopezEventBot.Modules;

public class RegisterRunescapeNameModal : IModal
{
    public string Title { get; } = "Register your account name";
    
    [InputLabel("Account Name")]
    [RequiredInput]
    [ModalTextInput("account_name")]
    public string AccountName { get; set; }
}