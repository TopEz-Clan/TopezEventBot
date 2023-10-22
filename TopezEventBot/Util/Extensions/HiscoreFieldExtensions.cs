using System.Reflection;

namespace TopezEventBot.Util.Extensions;

public static class HiscoreFieldExtensions
{
    public static string GetDisplayName(this HiscoreField field)
    {
        var lookupData = GetAttr(field);
        return lookupData != null ? lookupData.NiceName : field.ToString();
    }

    public static string GetBannerUrl(this HiscoreField field)
    {
        var lookupData = GetAttr(field);
        return lookupData != null ? lookupData.BannerUrl : Constants.INFORMATIONAL_FOOTER_LOGO_URL;
    }
    
    public static string GetThumbnailUrl(this HiscoreField field)
    {
        var lookupData = GetAttr(field);
        return lookupData != null ? lookupData.ThumbnailUrl : Constants.TOPEZ_LOGO_URL;
    }
    
    private static LookupAttribute? GetAttr(HiscoreField f)
    {
        return Attribute.GetCustomAttribute(ForValue(f), typeof(LookupAttribute)) as LookupAttribute;
    }
    
    private static MemberInfo? ForValue(HiscoreField field)
    {
        return typeof(HiscoreField).GetField(Enum.GetName(typeof(HiscoreField), field) ?? string.Empty);
    }
 
    
}