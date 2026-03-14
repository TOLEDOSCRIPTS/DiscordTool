using System.Text;
using System.Text.RegularExpressions;

namespace DiscordTool.Discord;

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
    public static string Italic(string text) => $"*{text}*";
    public static string BoldItalic(string text) => $"***{text}***";
    public static string Underline(string text) => $"__{text}__";
    public static string Strikethrough(string text) => $"~~{text}~~";
    public static string Spoiler(string text) => $"||{text}||";
    public static string Code(string text) => $"`{text}`";
    public static string CodeBlock(string text, string? language = null) => $"```{language ?? ""}\n{text}\n```";
    public static string BlockQuote(string text) => $"> {text}";
    public static string MultiBlockQuote(string text) => $">>> {text}";
    public static string Header1(string text) => $"# {text}";
    public static string Header2(string text) => $"## {text}";
    public static string Header3(string text) => $"### {text}";
    public static string MaskedLink(string text, string url) => $"[{text}]({url})";
    public static string BulletList(string text) => $"- {text}";
    public static string NumberedList(int number, string text) => $"{number}. {text}";
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

    public static string BuildEmbedDescription(
        string? title = null,
        string? description = null,
        string[]? fields = null,
        string? footer = null)
    {
        var sb = new StringBuilder();
        
        if (!string.IsNullOrEmpty(title))
        {
            sb.AppendLine($"**{title}**");
            sb.AppendLine();
        }
        
        if (!string.IsNullOrEmpty(description))
        {
            sb.AppendLine(description);
            sb.AppendLine();
        }
        
        if (fields != null)
        {
            foreach (var field in fields)
            {
                sb.AppendLine(field);
                sb.AppendLine();
            }
        }
        
        if (!string.IsNullOrEmpty(footer))
        {
            sb.AppendLine($"_{footer}_");
        }
        
        return sb.ToString();
    }
}
