namespace TopezEventBot.Util;

public class Constants {

    // Are you running the Test Server Discord?
    public static bool DebugMode = false;
		
		
    public static  string BOT_NAME = "TopEz Events Bot";
    public static  string TOPEZ_LOGO_URL = "https://i.imgur.com/COycEYl.png";
    public static  string INFORMATIONAL_FOOTER_LOGO_URL = "https://i.imgur.com/VIa5lWF.png";

    public static  string COMMAND_PREFIX = "!";
		
    /**
     * Id's of the roles found in the TopEz server.
     * (Make sure you have Developer Mode toggled On in your Discord settings to be able to right click and copy role/player id's etc)
     */
    public static  long COORDINATOR_ROLE_ID = 1031834561216249886L;
    public static  long MANAGEMENT_ROLE_ID = 1017112048842326066L;	
    public static  long EVENTS_ROLE_ID = 1034464324795772938L;

    /**
    * Id's of the channels found in the TopEz server.
    * (Make sure you have Developer Mode toggled On in your Discord settings to be able to right click and copy role/player id's etc)
    */
    public static  long EVENTS_CHANNEL_ID = 1022117899223179304L;
    public static  long CLAN_BANK_CHANNEL_ID = 1117882614343290881L;
    public static  long EVENTS_BOT_CHANNEL_ID = 1162232489625002034L;
    public static  long RUNESCAPE_GENERAL_CHANNEL_ID = 1018182484137087059L;
    public static  long STAR_GENERAL_CHANNEL_ID = 1018825374311923712L;

    /**
    * Used for testing the bot on a personal server to avoid spamming the chat
    */
    public static  long DEBUG_COORDINATOR_ROLE_ID = 1161517427327254528L;
    public static  long DEBUG_MANAGEMENT_ROLE_ID = 1114460369025499196L;	
    public static  long DEBUG_EVENTS_ROLE_ID = 1162236513917079664L;

    public static  long DEBUG_EVENTS_CHANNEL_ID = 1022117899223179304L;
    public static  long DEBUG_CLAN_BANK_CHANNEL_ID = 1117882614343290881L;
    public static  long DEBUG_EVENTS_BOT_CHANNEL_ID = 1162232489625002034L;
    public static  long DEBUG_RUNESCAPE_GENERAL_CHANNEL_ID = 1018182484137087059L;
    public static  long DEBUG_STAR_GENERAL_CHANNEL_ID = 1163037319339724861L;
    public static  string RSN_LINKER_MODAL_NAME = "rsn-linker-modal";

    /**
     * The data associated with BotW
     * {@code { bossName, thumbnailUrl, bannerUrl } }
     */

    // Make sure this token never gets shared
    public static  string DISCORD_BOT_TOKEN = "MTExOTc3MzUzOTgxNDM0MjY1OA.GLlkQV.Jeaa8lpvkCEvc66Z47EXqmeV3YAW2Bn7qckMT8";
}

public record RunescapeActivityLookupData(string niceName, string thum)
{
}