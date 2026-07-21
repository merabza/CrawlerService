using System;

namespace RobotsTxt;

public static class UriFactory
{
    public static Uri? GetUri(string strRef)
    {
        Uri? newUri = null;
        try
        {
            newUri = new Uri(strRef);
        }
        catch (UriFormatException)
        {
            // Intentionally left blank: return null if format is invalid
        }

        //Linux-ზე "/"-ით დაწყებული ბილიკი წარმატებით პარსდება როგორც file:/// მისამართი,
        //Windows-ზე კი UriFormatException ვარდება და null ბრუნდება.
        //რომ ორივე OS-ზე ქცევა ერთნაირი იყოს, file სქემიანი შედეგი აქვე null-ად ითვლება.
        return newUri is null || newUri.Scheme == Uri.UriSchemeFile ? null : newUri;
    }

    public static Uri? GetUri(Uri baseUri, string relativeUrName)
    {
        Uri? newUri = null;
        try
        {
            newUri = new Uri(baseUri, relativeUrName);
        }
        catch (UriFormatException)
        {
            // Intentionally left blank: return null if format is invalid
        }

        return newUri;
    }
}
