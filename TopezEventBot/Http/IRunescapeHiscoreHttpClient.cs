using TopezEventBot.Http.Models;

namespace TopezEventBot.Http;

public interface IRunescapeHiscoreHttpClient
{
    Task<Player> LoadPlayer(string userName);
}