using System;
using System.Web;
using CrawlerDbPersistence.Configurations;
using CrawlerRepoInterfaces;
using RobotsTxt;

namespace DoCrawler;

public static class UrlNameHelper
{
    //pre-check-ისთვის: მითითებულ batch part-ში ეს გვერდი უკვე გაანალიზებულია თუ არა.
    public static bool IsPageAlreadyAnalyzed(ICrawlerRepository crawlerRepository, int batchPartId, string pageAddress)
    {
        return crawlerRepository.GetContentAnalysisByUrlName(batchPartId, ToCheckedUrlName(pageAddress)) is not null;
    }

    //URL-ის სტრიქონიდან აგებს იმავე "checked" სახელს, რასაც GetUrlData იყენებს —
    //რომ pre-check-ისა და გაშვების დროს URL-ის რეზოლუცია ემთხვეოდეს.
    internal static string ToCheckedUrlName(string rawUrl)
    {
        Uri? uri = UriFactory.GetUri(rawUrl);
        if (uri is null)
        {
            return "InvalidUri";
        }

        string? checkedUrName = HttpUtility.UrlDecode(uri.AbsoluteUri).Truncate(UrlModelConfiguration.TermTextLength);
        return string.IsNullOrWhiteSpace(checkedUrName) ? "InvalidUri" : checkedUrName;
    }
}
