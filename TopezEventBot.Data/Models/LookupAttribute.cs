namespace TopezEventBot.Util;

public class LookupAttribute : Attribute
{
    public string NiceName { get; }
    public string ThumbnailUrl { get; }
    public string BannerUrl { get; }

    internal LookupAttribute(string niceName, string thumbnailUrl, string bannerUrl)
    {
        NiceName = niceName;
        ThumbnailUrl = thumbnailUrl;
        BannerUrl = bannerUrl;
    }
}