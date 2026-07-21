using RobotsTxt.Enums;

namespace RobotsTxt;

internal static class EnumHelper
{
    internal static LineType GetLineType(string field)
    {
        return field switch
        {
            "USER-AGENT" => LineType.UserAgent,
            "ALLOW" or "DISALLOW" => LineType.AccessRule,
            "CRAWL-DELAY" => LineType.CrawlDelayRule,
            "SITEMAP" => LineType.Sitemap,
            _ => LineType.Unknown
        };
    }
}
