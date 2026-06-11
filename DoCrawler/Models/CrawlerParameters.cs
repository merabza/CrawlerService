//Created by ProjectParametersClassCreator at 4/22/2021 17:17:01

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DoCrawler.Models;

public sealed class CrawlerParameters
{

    private string _punctuationRegex = string.Empty;
    private string _segmentFinisherPunctuationRegex = string.Empty;
    private string _wordDelimiterRegex = string.Empty;
    public int LoadPagesMaxCount { get; set; }
    public string? Alphabet { get; set; }

    public Dictionary<string, PunctuationModel> Punctuations { get; init; } = [];
    public static CrawlerParameters? Create(IConfiguration configuration)
    {
        IConfigurationSection projectSettingsSection = configuration.GetSection("CrawlerParameters");
        return projectSettingsSection.Get<CrawlerParameters>();
    }

    public string GetSegmentFinisherPunctuationsRegex()
    {
        if (string.IsNullOrEmpty(_segmentFinisherPunctuationRegex))
        {
            _segmentFinisherPunctuationRegex =
                GetPunctuationRegex(Punctuations.Where(s => s.Value.PctSentenceFinisher));
        }

        return _segmentFinisherPunctuationRegex;
    }

    private static string GetPunctuationRegex(IEnumerable<KeyValuePair<string, PunctuationModel>> punctuations)
    {
        var rex = new StringBuilder();
        foreach (KeyValuePair<string, PunctuationModel> pun in punctuations.OrderBy(s => s.Value.PctSortId))
        {
            if (rex.Length > 0)
            {
                rex.Append('|');
            }

            rex.Append('(');
            rex.Append(pun.Value.PctRegexPattern ?? pun.Value.PctPunctuation);
            rex.Append(')');
        }

        return rex.ToString();
    }

    internal string GetPunctuationsRegex()
    {
        if (string.IsNullOrEmpty(_punctuationRegex))
        {
            _punctuationRegex = GetPunctuationRegex(Punctuations);
        }

        return _punctuationRegex;
    }

    internal string GetWordDelimiterRegex()
    {
        if (string.IsNullOrEmpty(_wordDelimiterRegex))
        {
            _wordDelimiterRegex = "({" + GetPunctuationRegex(Punctuations.Where(s => !s.Value.PctCanBePartOfWord)) +
                                  @"|(\s)|(\n)})";
        }

        return _wordDelimiterRegex;
    }
}
