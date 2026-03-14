using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Guna.UI2.WinForms;

namespace DiscordTool;

public partial class MainForm : Form
{
    private Guna2Panel sidebar = null!;
    private Guna2Panel mainP = null!;
    private Guna2TabControl tabs = null!;
    private Guna2ComboBox themeC = null!;
    private Guna2TextBox tokenTxt = null!;
    private Guna2TextBox channelTxt = null!;
    private Guna2Button connectBtn = null!;
    private Guna2Button disconnectBtn = null!;
    private Guna2Label statusLbl = null!;
    private Guna2TextBox msgTxt = null!;
    private Guna2Button sendBtn = null!;
    private Guna2TextBox embedTitle = null!;
    private Guna2TextBox embedDesc = null!;
    private Guna2TextBox embedUrl = null!;
    private Guna2TextBox embedFooter = null!;
    private Guna2TextBox embedAuthor = null!;
    private Guna2ColorPicker embedColor = null!;
    private Guna2Button sendEmbedBtn = null!;
    private Guna2TextBox imgUrl = null!;
    private Guna2PictureBox imgPreview = null!;
    private Guna2Button sendImgBtn = null!;
    private Guna2TextBox rawOut = null!;
    
    private HttpClient http = new();
    private string token = "";
    private ulong channelId;
    private bool connected;
    private string currentTheme = "Dark";
    
    public MainForm()
    {
        InitializeComponent();
        SetupUI();
    }

    private void SetupUI()
    {
        this.Text = "Discord Tool";
        this.Size = new Size(1200, 800);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(30, 30, 30);
        this.ForeColor = Color.White;

        sidebar = new Guna2Panel { Dock = DockStyle.Left, Width = 260, FillColor = Color.FromArgb(40, 40, 40) };
        this.Controls.Add(sidebar);

        var title = new Guna2Label { Text = "Discord Tool", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(20, 20), ForeColor = Color.White };
        sidebar.Controls.Add(title);

        var tLabel = new Guna2Label { Text = "Bot Token:", Location = new Point(20, 70), ForeColor = Color.White };
        sidebar.Controls.Add(tLabel);

        tokenTxt = new Guna2TextBox { Location = new Point(20, 95), Size = new Size(220, 35), PlaceholderText = "Enter bot token...", PasswordChar = '*' };
        sidebar.Controls.Add(tokenTxt);

        var cLabel = new Guna2Label { Text = "Channel ID:", Location = new Point(20, 140), ForeColor = Color.White };
        sidebar.Controls.Add(cLabel);

        channelTxt = new Guna2TextBox { Location = new Point(20, 165), Size = new Size(220, 35), PlaceholderText = "Channel ID..." };
        sidebar.Controls.Add(channelTxt);

        connectBtn = new Guna2Button { Text = "Connect", Location = new Point(20, 210), Size = new Size(100, 35), FillColor = Color.FromArgb(88, 101, 242), BorderRadius = 6 };
        connectBtn.Click += Connect;
        sidebar.Controls.Add(connectBtn);

        disconnectBtn = new Guna2Button { Text = "Disconnect", Location = new Point(130, 210), Size = new Size(100, 35), FillColor = Color.FromArgb(235, 69, 69), BorderRadius = 6, Enabled = false };
        disconnectBtn.Click += Disconnect;
        sidebar.Controls.Add(disconnectBtn);

        statusLbl = new Guna2Label { Text = "Not Connected", Location = new Point(20, 255), ForeColor = Color.Gray };
        sidebar.Controls.Add(statusLbl);

        var thLabel = new Guna2Label { Text = "Theme:", Location = new Point(20, 300), ForeColor = Color.White };
        sidebar.Controls.Add(thLabel);

        themeC = new Guna2ComboBox { Location = new Point(20, 325), Size = new Size(220, 35), DropDownStyle = ComboBoxStyle.DropDownList };
        themeC.Items.AddRange(new[] { "Dark", "Light", "Midnight", "Ocean", "Forest" });
        themeC.SelectedIndex = 0;
        themeC.SelectedIndexChanged += (s, e) => ApplyTheme(themeC.SelectedItem?.ToString() ?? "Dark");
        sidebar.Controls.Add(themeC);

        mainP = new Guna2Panel { Dock = DockStyle.Fill, FillColor = Color.FromArgb(30, 30, 30) };
        this.Controls.Add(mainP);

        tabs = new Guna2TabControl { Dock = DockStyle.Fill, Location = new Point(10, 10) };
        mainP.Controls.Add(tabs);

        SetupMessageTab();
        SetupEmbedTab();
        SetupImageTab();
        SetupRawTab();
    }

    private void SetupMessageTab()
    {
        var tab = new Guna2TabPage { Text = "Message" };
        tabs.TabPages.Add(tab);

        var lbl = new Guna2Label { Text = "Message:", Location = new Point(20, 20), ForeColor = Color.White };
        tab.Controls.Add(lbl);

        msgTxt = new Guna2TextBox { Location = new Point(20, 50), Size = new Size(600, 200), Multiline = true, PlaceholderText = "Type your message...", ScrollBars = ScrollBars.Vertical };
        tab.Controls.Add(msgTxt);

        var fmtPanel = new Guna2Panel { Location = new Point(20, 260), Size = new Size(600, 40) };
        tab.Controls.Add(fmtPanel);

        string[] btns = { "B", "I", "U", "S", "Spoiler", "Code", "Link" };
        string[] fmts = { "**", "*", "__", "~~", "||", "`", "[]" };
        int x = 0;
        for (int i = 0; i < btns.Length; i++)
        {
            var btn = new Guna2Button { Text = btns[i], Location = new Point(x, 0), Size = new Size(75, 35), BorderRadius = 4, FillColor = Color.FromArgb(60, 60, 60) };
            int fi = i;
            btn.Click += (s, e) => InsertFmt(fmts[fi]);
            fmtPanel.Controls.Add(btn);
            x += 80;
        }

        sendBtn = new Guna2Button { Text = "Send", Location = new Point(20, 320), Size = new Size(150, 40), FillColor = Color.FromArgb(88, 101, 242), BorderRadius = 6 };
        sendBtn.Click += SendMessage;
        tab.Controls.Add(sendBtn);
    }

    private void InsertFmt(string f)
    {
        int p = msgTxt.SelectionStart;
        string t = msgTxt.SelectedText;
        msgTxt.Text = msgTxt.Text.Insert(p, f + t + (f == "[]" ? "(url)" : f));
        msgTxt.SelectionStart = p + f.Length;
    }

    private void SetupEmbedTab()
    {
        var tab = new Guna2TabPage { Text = "Embed" };
        tabs.TabPages.Add(tab);

        int y = 20;

        tab.Controls.Add(new Guna2Label { Text = "Title:", Location = new Point(20, y), ForeColor = Color.White });
        embedTitle = new Guna2TextBox { Location = new Point(20, y + 25), Size = new Size(350, 35), PlaceholderText = "Title..." };
        tab.Controls.Add(embedTitle);
        y += 65;

        tab.Controls.Add(new Guna2Label { Text = "Description:", Location = new Point(20, y), ForeColor = Color.White });
        embedDesc = new Guna2TextBox { Location = new Point(20, y + 25), Size = new Size(350, 100), Multiline = true, PlaceholderText = "Description..." };
        tab.Controls.Add(embedDesc);
        y += 135;

        tab.Controls.Add(new Guna2Label { Text = "Color:", Location = new Point(20, y), ForeColor = Color.White });
        embedColor = new Guna2ColorPicker { Location = new Point(80, y - 3), Size = new Size(100, 30) };
        embedColor.Color = Color.FromArgb(88, 101, 242);
        tab.Controls.Add(embedColor);
        y += 45;

        tab.Controls.Add(new Guna2Label { Text = "Footer:", Location = new Point(20, y), ForeColor = Color.White });
        embedFooter = new Guna2TextBox { Location = new Point(20, y + 25), Size = new Size(350, 35), PlaceholderText = "Footer text..." };
        tab.Controls.Add(embedFooter);
        y += 65;

        tab.Controls.Add(new Guna2Label { Text = "Author:", Location = new Point(20, y), ForeColor = Color.White });
        embedAuthor = new Guna2TextBox { Location = new Point(20, y + 25), Size = new Size(350, 35), PlaceholderText = "Author name..." };
        tab.Controls.Add(embedAuthor);

        sendEmbedBtn = new Guna2Button { Text = "Send Embed", Location = new Point(20, 450), Size = new Size(150, 40), FillColor = Color.FromArgb(88, 101, 242), BorderRadius = 6 };
        sendEmbedBtn.Click += SendEmbed;
        tab.Controls.Add(sendEmbedBtn);
    }

    private void SetupImageTab()
    {
        var tab = new Guna2TabPage { Text = "Image" };
        tabs.TabPages.Add(tab);

        tab.Controls.Add(new Guna2Label { Text = "Image URL:", Location = new Point(20, 20), ForeColor = Color.White });

        imgUrl = new Guna2TextBox { Location = new Point(20, 50), Size = new Size(500, 35), PlaceholderText = "https://..." };
        imgUrl.TextChanged += (s, e) => { try { imgPreview.LoadAsync(imgUrl.Text); } catch { } };
        tab.Controls.Add(imgUrl);

        imgPreview = new Guna2PictureBox { Location = new Point(20, 100), Size = new Size(350, 250), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.FromArgb(40, 40, 40) };
        tab.Controls.Add(imgPreview);

        sendImgBtn = new Guna2Button { Text = "Send Image", Location = new Point(20, 370), Size = new Size(150, 40), FillColor = Color.FromArgb(88, 101, 242), BorderRadius = 6 };
        sendImgBtn.Click += SendImage;
        tab.Controls.Add(sendImgBtn);
    }

    private void SetupRawTab()
    {
        var tab = new Guna2TabPage { Text = "Raw" };
        tabs.TabPages.Add(tab);

        rawOut = new Guna2TextBox { Dock = DockStyle.Fill, Multiline = true, Font = new Font("Consolas", 10), PlaceholderText = "Raw output..." };
        tab.Controls.Add(rawOut);

        var genBtn = new Guna2Button { Text = "Generate JSON", Location = new Point(20, tab.Height - 50), Size = new Size(130, 35), FillColor = Color.FromArgb(88, 101, 242) };
        genBtn.Click += (s, e) => rawOut.Text = JsonSerializer.Serialize(new { content = msgTxt.Text }, new JsonSerializerOptions { WriteIndented = true });
        tab.Controls.Add(genBtn);
    }

    private async void Connect(object? sender, EventArgs e)
    {
        token = tokenTxt.Text.Trim();
        if (!ulong.TryParse(channelTxt.Text.Trim(), out channelId) || string.IsNullOrEmpty(token))
        {
            MessageBox.Show("Enter valid token and channel ID");
            return;
        }

        try
        {
            http.DefaultRequestHeaders.Clear();
            http.DefaultRequestHeaders.Add("Authorization", $"Bot {token}");
            var r = await http.GetAsync($"https://discord.com/api/v10/channels/{channelId}");
            
            if (r.IsSuccessStatusCode)
            {
                connected = true;
                statusLbl.Text = "Connected";
                statusLbl.ForeColor = Color.Green;
                connectBtn.Enabled = false;
                disconnectBtn.Enabled = true;
            }
            else
            {
                MessageBox.Show("Connection failed");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }

    private async void Disconnect(object? sender, EventArgs e)
    {
        connected = false;
        statusLbl.Text = "Not Connected";
        statusLbl.ForeColor = Color.Gray;
        connectBtn.Enabled = true;
        disconnectBtn.Enabled = false;
        await Task.CompletedTask;
    }

    private async void SendMessage(object? sender, EventArgs e)
    {
        if (!connected) { MessageBox.Show("Connect first!"); return; }
        
        var pay = JsonSerializer.Serialize(new { content = msgTxt.Text });
        var c = new StringContent(pay, Encoding.UTF8, "application/json");
        var r = await http.PostAsync($"https://discord.com/api/v10/channels/{channelId}/messages", c);
        
        if (r.IsSuccessStatusCode)
            MessageBox.Show("Sent!");
        else
            MessageBox.Show($"Failed: {await r.Content.ReadAsStringAsync()}");
    }

    private async void SendEmbed(object? sender, EventArgs e)
    {
        if (!connected) { MessageBox.Show("Connect first!"); return; }

        var emb = new Dictionary<string, object?>
        {
            ["title"] = embedTitle.Text,
            ["description"] = embedDesc.Text,
            ["color"] = embedColor.Color.ToArgb(),
            ["footer"] = string.IsNullOrEmpty(embedFooter.Text) ? null : new { text = embedFooter.Text },
            ["author"] = string.IsNullOrEmpty(embedAuthor.Text) ? null : new { name = embedAuthor.Text }
        };

        var pay = JsonSerializer.Serialize(new { embeds = new[] { emb } });
        var c = new StringContent(pay, Encoding.UTF8, "application/json");
        var r = await http.PostAsync($"https://discord.com/api/v10/channels/{channelId}/messages", c);

        if (r.IsSuccessStatusCode)
            MessageBox.Show("Embed sent!");
        else
            MessageBox.Show($"Failed: {await r.Content.ReadAsStringAsync()}");
    }

    private async void SendImage(object? sender, EventArgs e)
    {
        if (!connected) { MessageBox.Show("Connect first!"); return; }

        var pay = JsonSerializer.Serialize(new { content = imgUrl.Text });
        var c = new StringContent(pay, Encoding.UTF8, "application/json");
        var r = await http.PostAsync($"https://discord.com/api/v10/channels/{channelId}/messages", c);

        if (r.IsSuccessStatusCode)
            MessageBox.Show("Image sent!");
        else
            MessageBox.Show($"Failed: {await r.Content.ReadAsStringAsync()}");
    }

    private void ApplyTheme(string name)
    {
        Color bg, fg, side;
        switch (name)
        {
            case "Light": bg = Color.White; fg = Color.Black; side = Color.FromArgb(245, 245, 245); break;
            case "Midnight": bg = Color.FromArgb(20, 20, 40); fg = Color.FromArgb(200, 200, 220); side = Color.FromArgb(30, 30, 60); break;
            case "Ocean": bg = Color.FromArgb(10, 30, 50); fg = Color.FromArgb(180, 210, 230); side = Color.FromArgb(20, 50, 80); break;
            case "Forest": bg = Color.FromArgb(20, 35, 20); fg = Color.FromArgb(200, 220, 200); side = Color.FromArgb(30, 50, 30); break;
            default: bg = Color.FromArgb(30, 30, 30); fg = Color.White; side = Color.FromArgb(40, 40, 40); break;
        }
        this.BackColor = bg;
        this.ForeColor = fg;
        mainP.FillColor = bg;
        sidebar.FillColor = side;
    }

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
