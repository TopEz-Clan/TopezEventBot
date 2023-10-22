using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TopezEventBot.Http.Models;
using TopezEventBot.Util;

namespace TopezEventBot.Http;

public class RunescapeHiscoreHttpClient : IRunescapeHiscoreHttpClient
{
    private HttpClient _client;
    public RunescapeHiscoreHttpClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Player> LoadPlayer(string userName)
    {
        var rawPlayerDataStream = await _client.GetStreamAsync($"?player={userName}");
        var stringData = await _client.GetStringAsync($"?player={userName}");
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = ",",
            NewLine = "\n"
        };
        using var streamReader = new StreamReader(rawPlayerDataStream);
        using var csv = new CsvReader(streamReader, config);
        var idx = 0;
        
        var player = new Player
        {
            UserName = userName
        };
        
        while (await csv.ReadAsync())
        {
            switch (idx)
            {
                case <= (int)HiscoreField.Construction:
                    player.Skills[(HiscoreField)idx++] = csv.GetRecord<SkillStats>();
                    break;
                case < (int)HiscoreField.Riftsclosed:
                    player.Activities[(HiscoreField)idx++] = csv.GetRecord<ActivityStats>();
                    break;
                default:
                    player.Bosses[(HiscoreField)idx++] = csv.GetRecord<ActivityStats>();
                    break;
            }
        }

        return player;
    }
}