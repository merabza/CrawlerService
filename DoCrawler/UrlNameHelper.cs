using System;
using System.IO;
using System.Web;
using CrawlerDbPersistence.Configurations;
using CrawlerDbModels;
using CrawlerRepoInterfaces;
using LanguageExt;
using RobotsTxt;

namespace DoCrawler;

public static class UrlNameHelper
{
    //pre-check-ისთვის: მითითებულ batch part-ში ეს გვერდი უკვე გაანალიზებულია თუ არა.
    public static bool IsPageAlreadyAnalyzed(ICrawlerRepository crawlerRepository, int batchPartId, string pageAddress)
    {
        Option<int> urlId = GetUrlId(crawlerRepository, pageAddress);
        if (urlId.IsNone)
        {
            return false;
        }

        return crawlerRepository.GetContentAnalysis(batchPartId, (int)urlId) is not null;
    }

    private static Option<int> GetUrlId(ICrawlerRepository crawlerRepository, string urName)
    {
        Uri? myUri = UriFactory.GetUri(urName);
        if (myUri is null)
        {
            return new Option<int>();
        }

        string? host = myUri.Host.Truncate(HostModelConfiguration.HostNameLength);
        if (string.IsNullOrWhiteSpace(host))
        {
            //host = "InvalidHostName";
            return new Option<int>();
        }

        string absolutePath = myUri.AbsolutePath;

        string? extension = Path.GetExtension(absolutePath).Truncate(ExtensionModelConfiguration.ExtensionNameLength);
        if (string.IsNullOrWhiteSpace(extension))
        {
            //extension = "NoExtension";
            return new Option<int>();
        }

        string? scheme = myUri.Scheme.Truncate(SchemeModelConfiguration.SchemeNameLength);
        if (string.IsNullOrWhiteSpace(scheme))
        {
            //scheme = "InvalidSchemeName";
            return new Option<int>();
        }

        int hostModelId = crawlerRepository.GetHostId(host);
        int extensionId = crawlerRepository.GetExtensionId(extension);
        int schemeInt = crawlerRepository.GetSchemeId(scheme);

        Option<Uri> checkedUrlResult = ToCheckedUrlName(urName);
        if (checkedUrlResult.IsNone)
        {
            //Invalid Uri
            return new Option<int>();
        }

        int urlHashCode = ((Uri)checkedUrlResult).AbsoluteUri.GetDeterministicHashCode();

        //UrlModel? url = _procData.GetUrlByHashCode(urlHashCode);

        if (hostModelId == 0 || extensionId == 0 || schemeInt == 0)
        {
            return new Option<int>();
        }

        UrlModel? url = crawlerRepository.GetUrl(hostModelId, extensionId, schemeInt, urlHashCode,
            ((Uri)checkedUrlResult).AbsoluteUri);

        if (url is not null)
        {
            return url.UrlId;
        }

        return new Option<int>();
    }

    //URL-ის სტრიქონიდან აგებს იმავე "checked" სახელს, რასაც GetUrlData იყენებს —
    //რომ pre-check-ისა და გაშვების დროს URL-ის რეზოლუცია ემთხვეოდეს.
    internal static Option<Uri> ToCheckedUrlName(string rawUrl)
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
