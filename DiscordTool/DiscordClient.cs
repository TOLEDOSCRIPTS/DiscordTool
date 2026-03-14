using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace DiscordTool;

public class DiscordClient
{
    private readonly string _token;
    private readonly ulong _channelId;
    private readonly HttpClient _httpClient;
    private bool _isConnected;

    public bool IsConnected => _isConnected;

    public DiscordClient(string token, ulong channelId)
    {
        _token = token;
        _channelId = channelId;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {token}");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "DiscordBot (DiscordTool, 1.0)");
    }

    public async Task ConnectAsync()
    {
        var response = await _httpClient.GetAsync($"https://discord.com/api/v10/channels/{_channelId}");
        _isConnected = response.IsSuccessStatusCode;
        if (!_isConnected)
            throw new Exception($"Failed to connect: {response.StatusCode}");
    }

    public async Task DisconnectAsync()
    {
        _isConnected = false;
        await Task.CompletedTask;
    }

    public async Task SendMessageAsync(string content)
    {
        var payload = new { content };
        await SendPayloadAsync(payload);
    }

    public async Task SendEmbedAsync(Embed embed)
    {
        var payload = new { embeds = new[] { embed } };
        await SendPayloadAsync(payload);
    }

    public async Task SendImageAsync(string imageUrl, string? caption = null)
    {
        var content = string.IsNullOrEmpty(caption) ? imageUrl : $"{caption}\n{imageUrl}";
        var payload = new { content };
        await SendPayloadAsync(payload);
    }

    private async Task SendPayloadAsync(object payload)
    {
        var json = JsonConvert.SerializeObject(payload);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(
            $"https://discord.com/api/v10/channels/{_channelId}/messages",
            httpContent
        );

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to send: {response.StatusCode} - {errorContent}");
        }
    }
}

public class Embed
{
    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("color")]
    public int? Color { get; set; }

    [JsonProperty("timestamp")]
    public DateTime? Timestamp { get; set; }

    [JsonProperty("footer")]
    public EmbedFooter? Footer { get; set; }

    [JsonProperty("image")]
    public EmbedImage? Image { get; set; }

    [JsonProperty("thumbnail")]
    public EmbedImage? Thumbnail { get; set; }

    [JsonProperty("author")]
    public EmbedAuthor? Author { get; set; }

    [JsonProperty("fields")]
    public List<EmbedField> Fields { get; set; } = new();
}

public class EmbedFooter
{
    [JsonProperty("text")]
    public string? Text { get; set; }

    [JsonProperty("icon_url")]
    public string? IconUrl { get; set; }
}

public class EmbedImage
{
    [JsonProperty("url")]
    public string? Url { get; set; }
}

public class EmbedAuthor
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("icon_url")]
    public string? IconUrl { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }
}

public class EmbedField
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("value")]
    public string? Value { get; set; }

    [JsonProperty("inline")]
    public bool Inline { get; set; }
}
