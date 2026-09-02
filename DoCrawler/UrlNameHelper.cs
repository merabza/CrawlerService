using System;
using System.IO;
using System.Web;
using CrawlerRepoInterfaces;
using CrawlerServiceDbPart.Db.Configurations;
using CrawlerServiceRoot.Domain.UrlModels;
using RobotsTxt;

namespace DoCrawler;

public static class UrlNameHelper
{
    //pre-check-ისთვის: მითითებულ batch part-ში ეს გვერდი უკვე გაანალიზებულია თუ არა.
    public static bool IsPageAlreadyAnalyzed(ICrawlerRepository crawlerRepository, int batchPartId, string pageAddress)
    {
        int? urlId = GetUrlId(crawlerRepository, pageAddress);
        if (urlId is null)
        {
            return false;
        }

        return crawlerRepository.GetContentAnalysis(batchPartId, urlId.Value) is not null;
    }

    private static int? GetUrlId(ICrawlerRepository crawlerRepository, string urName)
    {
        Uri? myUri = UriFactory.GetUri(urName);
        if (myUri is null)
        {
            return null;
        }

        string? host = myUri.Host.Truncate(HostModelConfiguration.HostNameLength);
        if (string.IsNullOrWhiteSpace(host))
        {
            //host = "InvalidHostName";
            return null;
        }

        string absolutePath = myUri.AbsolutePath;

        string? extension = Path.GetExtension(absolutePath).Truncate(ExtensionModelConfiguration.ExtensionNameLength);
        if (string.IsNullOrWhiteSpace(extension))
        {
            //extension = "NoExtension";
            return null;
        }

        string? scheme = myUri.Scheme.Truncate(SchemeModelConfiguration.SchemeNameLength);
        if (string.IsNullOrWhiteSpace(scheme))
        {
            //scheme = "InvalidSchemeName";
            return null;
        }

        int hostModelId = crawlerRepository.GetHostId(host);
        int extensionId = crawlerRepository.GetExtensionId(extension);
        int schemeInt = crawlerRepository.GetSchemeId(scheme);

        Uri? checkedUrlResult = ToCheckedUrlName(urName);
        if (checkedUrlResult is null)
        {
            //Invalid Uri
            return null;
        }

        int urlHashCode = checkedUrlResult.AbsoluteUri.GetDeterministicHashCode();

        //UrlModel? url = _procData.GetUrlByHashCode(urlHashCode);

        if (hostModelId == 0 || extensionId == 0 || schemeInt == 0)
        {
            return null;
        }

        UrlModel? url = crawlerRepository.GetUrl(hostModelId, extensionId, schemeInt, urlHashCode,
            checkedUrlResult.AbsoluteUri);

        return url?.UrlId;
    }

    //URL-ის სტრიქონიდან აგებს იმავე "checked" სახელს, რასაც GetUrlData იყენებს —
    //რომ pre-check-ისა და გაშვების დროს URL-ის რეზოლუცია ემთხვეოდეს.
    internal static Uri? ToCheckedUrlName(string rawUrl)
    {
        Uri? uri = UriFactory.GetUri(rawUrl);
        if (uri is null)
        {
            return null;
        }

        string? checkedUrName = HttpUtility.UrlDecode(uri.AbsoluteUri).Truncate(UrlModelConfiguration.TermTextLength);
        return string.IsNullOrWhiteSpace(checkedUrName) ? null : new Uri(checkedUrName);
    }
}
