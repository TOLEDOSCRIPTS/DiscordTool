using System.Text;
using System.Text.RegularExpressions;

namespace DiscordTool;

public static class DiscordMarkdown
{
    public static string FormatForPreview(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return "";

        var result = markdown;

        result = Regex.Replace(result, @"\*\*\*(.+?)\*\*\*", "$1");
        result = Regex.Replace(result, @"\*\*(.+?)\*\*", "$1");
        result = Regex.Replace(result, @"\*(.+?)\*", "$1");
        result = Regex.Replace(result, @"__(.+?)__", "$1");
        result = Regex.Replace(result, @"_(.+?)_", "$1");
        result = Regex.Replace(result, @"~~(.+?)~~", "$1");
        result = Regex.Replace(result, @"\|\|(.+?)\|\|", "$1");
        
        return result;
    }

    public static string Bold(string text) => $"**{text}**";
    public static string ItalicText(string text) => $"*{text}*";
    public static string BoldItalic(string text) => $"***{text}***";
    public static string UnderlineText(string text) => $"__{text}__";
    public static string StrikethroughText(string text) => $"~~{text}~~";
    public static string SpoilerText(string text) => $"||{text}||";
    public static string Code(string text) => $"`{text}`";
    public static string CodeBlock(string text, string? language = null) => $"```{language ?? ""}\n{text}\n```";
    public static string BlockQuote(string text) => $"> {text}";
    public static string MultiBlockQuoteText(string text) => $">>> {text}";
    public static string Header1(string text) => $"# {text}";
    public static string Header2(string text) => $"## {text}";
    public static string Header3(string text) => $"### {text}";
    public static string MaskedLink(string text, string url) => $"[{text}]({url})";
    public static string BulletList(string text) => $"- {text}";
    public static string NumberedListText(int number, string text) => $"{number}. {text}";
    public static string Subtext(string text) => $"-# {text}";

    public static string BuildFormattedMessage(params string[] parts)
    {
        var sb = new StringBuilder();
        foreach (var part in parts)
        {
            if (sb.Length > 0) sb.AppendLine();
            sb.Append(part);
        }
        return sb.ToString();
    }
}
