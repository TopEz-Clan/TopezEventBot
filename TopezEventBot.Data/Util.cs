namespace TopezEventBot.Data;

public static class Util
{
    public static string DbPath()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        return Path.Join(path, "topez.db");
    }
}