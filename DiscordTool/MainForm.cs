using System.Drawing;
using System.Text;
using DiscordTool.Discord;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;

namespace DiscordTool;

public partial class MainForm : Form
{
    private Guna2Panel _sidebarPanel = null!;
    private Guna2Panel _mainPanel = null!;
    private Guna2TabControl _tabControl = null!;
    private Guna2ComboBox _themeComboBox = null!;
    private Guna2TextBox _botTokenTextBox = null!;
    private Guna2TextBox _channelIdTextBox = null!;
    private Guna2Button _connectButton = null!;
    private Guna2Button _disconnectButton = null!;
    private Guna2Label _connectionStatusLabel = null!;
    
    private Guna2TextBox _messageContentTextBox = null!;
    private Guna2Button _sendMessageButton = null!;
    private Guna2Panel _formattingPanel = null!;
    
    private Guna2TextBox _embedTitleTextBox = null!;
    private Guna2TextBox _embedDescriptionTextBox = null!;
    private Guna2TextBox _embedUrlTextBox = null!;
    private Guna2TextBox _embedFooterTextBox = null!;
    private Guna2TextBox _embedFooterIconTextBox = null!;
    private Guna2TextBox _embedAuthorTextBox = null!;
    private Guna2TextBox _embedAuthorIconTextBox = null!;
    private Guna2TextBox _embedThumbnailUrlTextBox = null!;
    private Guna2TextBox _embedImageUrlTextBox = null!;
    private Guna2ColorPicker _embedColorPicker = null!;
    private Guna2DateTimePicker _embedTimestampPicker = null!;
    private Guna2Button _addEmbedFieldButton = null!;
    private Guna2Panel _embedFieldsPanel = null!;
    private Guna2Button _sendEmbedButton = null!;
    private Guna2Button _previewEmbedButton = null!;
    
    private Guna2TextBox _imageUrlTextBox = null!;
    private Guna2Button _loadImageButton = null!;
    private Guna2PictureBox _imagePreviewBox = null!;
    private Guna2TextBox _imageCaptionTextBox = null!;
    private Guna2Button _sendImageButton = null!;
    
    private Guna2Panel _rawOutputPanel = null!;
    private Guna2TextBox _rawOutputTextBox = null!;
    private Guna2Button _copyRawButton = null!;
    private Guna2Button _clearOutputButton = null!;
    
    private DiscordClient? _discordClient;
    private readonly List<EmbedField> _embedFields = new();
    private string _currentTheme = "Dark";
    
    public MainForm()
    {
        InitializeComponent();
        SetupForm();
    }

    private void SetupForm()
    {
        this.Text = "Discord Message & Embed Tool";
        this.Size = new Size(1400, 900);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.MinimumSize = new Size(1200, 700);
        
        SetupSidebar();
        SetupMainContent();
        ApplyTheme("Dark");
    }

    private void SetupSidebar()
    {
        _sidebarPanel = new Guna2Panel
        {
            Dock = DockStyle.Left,
            Width = 280,
            Padding = new Padding(10)
        };
        this.Controls.Add(_sidebarPanel);

        var titleLabel = new Guna2Label
        {
            Text = "🔷 Discord Tool",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            Location = new Point(20, 20),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(titleLabel);

        var settingsLabel = new Guna2Label
        {
            Text = "⚙️ Settings",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Location = new Point(20, 70),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(settingsLabel);

        var tokenLabel = new Guna2Label
        {
            Text = "Bot Token:",
            Location = new Point(20, 110),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(tokenLabel);

        _botTokenTextBox = new Guna2TextBox
        {
            Location = new Point(20, 135),
            Size = new Size(240, 40),
            PlaceholderText = "Enter your bot token...",
            PasswordChar = '*',
            UseSystemPasswordChar = true
        };
        _sidebarPanel.Controls.Add(_botTokenTextBox);

        var channelLabel = new Guna2Label
        {
            Text = "Channel ID:",
            Location = new Point(20, 185),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(channelLabel);

        _channelIdTextBox = new Guna2TextBox
        {
            Location = new Point(20, 210),
            Size = new Size(240, 40),
            PlaceholderText = "Enter channel ID..."
        };
        _sidebarPanel.Controls.Add(_channelIdTextBox);

        _connectButton = new Guna2Button
        {
            Text = "Connect",
            Location = new Point(20, 260),
            Size = new Size(110, 40),
            FillColor = Color.FromArgb(88, 101, 242),
            BorderRadius = 8
        };
        _connectButton.Click += ConnectButton_Click;
        _sidebarPanel.Controls.Add(_connectButton);

        _disconnectButton = new Guna2Button
        {
            Text = "Disconnect",
            Location = new Point(150, 260),
            Size = new Size(110, 40),
            FillColor = Color.FromArgb(235, 69, 69),
            BorderRadius = 8,
            Enabled = false
        };
        _disconnectButton.Click += DisconnectButton_Click;
        _sidebarPanel.Controls.Add(_disconnectButton);

        _connectionStatusLabel = new Guna2Label
        {
            Text = "⚪ Not Connected",
            Location = new Point(20, 310),
            AutoSize = true,
            ForeColor = Color.Gray
        };
        _sidebarPanel.Controls.Add(_connectionStatusLabel);

        var themeLabel = new Guna2Label
        {
            Text = "🎨 Theme:",
            Location = new Point(20, 360),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(themeLabel);

        _themeComboBox = new Guna2ComboBox
        {
            Location = new Point(20, 385),
            Size = new Size(240, 40),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _themeComboBox.Items.AddRange(new[] { "Dark", "Light", "Midnight", "Ocean", "Forest" });
        _themeComboBox.SelectedIndex = 0;
        _themeComboBox.SelectedIndexChanged += ThemeComboBox_SelectedIndexChanged;
        _sidebarPanel.Controls.Add(_themeComboBox);

        var formatLabel = new Guna2Label
        {
            Text = "📖 Markdown Guide",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Location = new Point(20, 450),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(formatLabel);

        var guideText = @"**bold** or __bold__
*italic* or _italic_
***bold italic***
~~strikethrough~~
||spoiler||
# Header 1
## Header 2
### Header 3
> Block Quote
```code block```
[text](url)
- List item
1. Numbered list";

        var guideLabel = new Guna2Label
        {
            Text = guideText,
            Location = new Point(20, 480),
            AutoSize = false,
            Size = new Size(240, 200),
            Font = new Font("Consolas", 9)
        };
        _sidebarPanel.Controls.Add(guideLabel);
    }

    private void SetupMainContent()
    {
        _mainPanel = new Guna2Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10)
        };
        this.Controls.Add(_mainPanel);

        _tabControl = new Guna2TabControl
        {
            Dock = DockStyle.Fill,
            Location = new Point(10, 10),
            Style = TabStyle.Default
        };
        _mainPanel.Controls.Add(_tabControl);

        SetupMessageTab();
        SetupEmbedTab();
        SetupImageTab();
        SetupRawTab();
    }

    private void SetupMessageTab()
    {
        var messageTab = new Guna2TabPage { Text = "💬 Message" };
        _tabControl.TabPages.Add(messageTab);

        var contentLabel = new Guna2Label
        {
            Text = "Message Content:",
            Location = new Point(20, 20),
            AutoSize = true
        };
        messageTab.Controls.Add(contentLabel);

        _messageContentTextBox = new Guna2TextBox
        {
            Location = new Point(20, 50),
            Size = new Size(800, 200),
            Multiline = true,
            PlaceholderText = "Enter your Discord message here...",
            ScrollBars = ScrollBars.Vertical
        };
        messageTab.Controls.Add(_messageContentTextBox);

        _formattingPanel = new Guna2Panel
        {
            Location = new Point(20, 260),
            Size = new Size(800, 50)
        };
        messageTab.Controls.Add(_formattingPanel);

        SetupFormattingButtons();
        
        var previewLabel = new Guna2Label
        {
            Text = "Preview:",
            Location = new Point(20, 320),
            AutoSize = true
        };
        messageTab.Controls.Add(previewLabel);

        var previewBox = new Guna2Panel
        {
            Location = new Point(20, 345),
            Size = new Size(800, 150),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(54, 57, 63)
        };
        
        var previewRichTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(54, 57, 63),
            ForeColor = Color.White,
            ReadOnly = true,
            Font = new Font("Segoe UI", 11),
            WordWrap = true
        };
        previewBox.Controls.Add(previewRichTextBox);
        messageTab.Controls.Add(previewBox);

        _messageContentTextBox.TextChanged += (s, e) =>
        {
            var formatted = DiscordMarkdown.FormatForPreview(_messageContentTextBox.Text);
            previewRichTextBox.Rtf = formatted;
        };

        _sendMessageButton = new Guna2Button
        {
            Text = "📤 Send Message",
            Location = new Point(20, 510),
            Size = new Size(200, 50),
            FillColor = Color.FromArgb(88, 101, 242),
            BorderRadius = 10,
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };
        _sendMessageButton.Click += SendMessageButton_Click;
        messageTab.Controls.Add(_sendMessageButton);

        var insertButtonsPanel = new Guna2Panel
        {
            Location = new Point(250, 260),
            Size = new Size(570, 50)
        };
        messageTab.Controls.Add(insertButtonsPanel);
        SetupInsertButtons(insertButtonsPanel);
    }

    private void SetupFormattingButtons()
    {
        var buttons = new[]
        {
            ("Bold", "**", "**"),
            ("Italic", "*", "*"),
            ("Underline", "__", "__"),
            ("Strike", "~~", "~~"),
            ("Spoiler", "||", "||"),
            ("Code", "`", "`"),
            ("Code Block", "```\n", "\n```"),
            ("Quote", "> ", ""),
            ("Link", "[", "](url)")
        };

        int x = 0;
        foreach (var (label, prefix, suffix) in buttons)
        {
            var btn = new Guna2Button
            {
                Text = label,
                Location = new Point(x, 0),
                Size = new Size(80, 40),
                BorderRadius = 5,
                FillColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI", 8)
            };
            int start = x;
            btn.Click += (s, e) => InsertMarkdown(prefix, suffix);
            _formattingPanel.Controls.Add(btn);
            x += 85;
        }
    }

    private void SetupInsertButtons(Guna2Panel panel)
    {
        var buttons = new[]
        {
            ("H1", "# "),
            ("H2", "## "),
            ("H3", "### "),
            ("• List", "- "),
            ("1. List", "1. "),
            ("Block", ">>> ")
        };

        int x = 0;
        foreach (var (label, prefix) in buttons)
        {
            var btn = new Guna2Button
            {
                Text = label,
                Location = new Point(x, 0),
                Size = new Size(85, 40),
                BorderRadius = 5,
                FillColor = Color.FromArgb(60, 60, 60),
                Font = new Font("Segoe UI", 8)
            };
            btn.Click += (s, e) => InsertAtLineStart(prefix);
            panel.Controls.Add(btn);
            x += 90;
        }
    }

    private void InsertMarkdown(string prefix, string suffix)
    {
        var tb = _messageContentTextBox;
        int start = tb.SelectionStart;
        int length = tb.SelectionLength;
        
        if (length > 0)
        {
            var selected = tb.SelectedText;
            tb.Text = tb.Text.Insert(start, prefix + selected + suffix);
            tb.SelectionStart = start + prefix.Length;
            tb.SelectionLength = length;
        }
        else
        {
            tb.Text = tb.Text.Insert(start, prefix + suffix);
            tb.SelectionStart = start + prefix.Length;
        }
        tb.Focus();
    }

    private void InsertAtLineStart(string prefix)
    {
        var tb = _messageContentTextBox;
        int pos = tb.SelectionStart;
        
        int lineStart = tb.Text.LastIndexOf('\n', Math.Max(0, pos - 1)) + 1;
        
        tb.Text = tb.Text.Insert(lineStart, prefix);
        tb.SelectionStart = pos + prefix.Length;
        tb.Focus();
    }

    private void SetupEmbedTab()
    {
        var embedTab = new Guna2TabPage { Text = "📑 Embed Builder" };
        _tabControl.TabPages.Add(embedTab);

        int left = 20;
        int top = 20;
        int width = 380;

        var titleLabel = new Guna2Label { Text = "Title:", Location = new Point(left, top), AutoSize = true };
        embedTab.Controls.Add(titleLabel);
        
        _embedTitleTextBox = new Guna2TextBox
        {
            Location = new Point(left, top + 20),
            Size = new Size(width, 35),
            PlaceholderText = "Embed title..."
        };
        embedTab.Controls.Add(_embedTitleTextBox);
        top += 60;

        var descLabel = new Guna2Label { Text = "Description:", Location = new Point(left, top), AutoSize = true };
        embedTab.Controls.Add(descLabel);
        
        _embedDescriptionTextBox = new Guna2TextBox
        {
            Location = new Point(left, top + 20),
            Size = new Size(width, 120),
            Multiline = true,
            PlaceholderText = "Embed description...",
            ScrollBars = ScrollBars.Vertical
        };
        embedTab.Controls.Add(_embedDescriptionTextBox);
        top += 150;

        var urlLabel = new Guna2Label { Text = "Title URL:", Location = new Point(left, top), AutoSize = true };
        embedTab.Controls.Add(urlLabel);
        
        _embedUrlTextBox = new Guna2TextBox
        {
            Location = new Point(left, top + 20),
            Size = new Size(width, 35),
            PlaceholderText = "https://..."
        };
        embedTab.Controls.Add(_embedUrlTextBox);
        top += 60;

        var colorLabel = new Guna2Label { Text = "Color:", Location = new Point(left, top), AutoSize = true };
        embedTab.Controls.Add(colorLabel);
        
        _embedColorPicker = new Guna2ColorPicker
        {
            Location = new Point(left + 60, top - 3),
            Size = new Size(100, 30)
        };
        _embedColorPicker.Color = Color.FromArgb(88, 101, 242);
        embedTab.Controls.Add(_embedColorPicker);
        top += 45;

        var timestampLabel = new Guna2Label { Text = "Timestamp:", Location = new Point(left, top), AutoSize = true };
        embedTab.Controls.Add(timestampLabel);
        
        _embedTimestampPicker = new Guna2DateTimePicker
        {
            Location = new Point(left + 100, top - 3),
            Size = new Size(200, 30),
            Value = DateTime.Now
        };
        embedTab.Controls.Add(_embedTimestampPicker);
        top += 50;

        var authorLabel = new Guna2Label { Text = "Author:", Location = new Point(left, top), AutoSize = true };
        embedTab.Controls.Add(authorLabel);
        
        _embedAuthorTextBox = new Guna2TextBox
        {
            Location = new Point(left, top + 20),
            Size = new Size(width, 35),
            PlaceholderText = "Author name..."
        };
        embedTab.Controls.Add(_embedAuthorTextBox);
        
        _embedAuthorIconTextBox = new Guna2TextBox
        {
            Location = new Point(left, top + 60),
            Size = new Size(width, 35),
            PlaceholderText = "Author icon URL..."
        };
        embedTab.Controls.Add(_embedAuthorIconTextBox);
        top += 110;

        var footerLabel = new Guna2Label { Text = "Footer:", Location = new Point(left, top), AutoSize = true };
        embedTab.Controls.Add(footerLabel);
        
        _embedFooterTextBox = new Guna2TextBox
        {
            Location = new Point(left, top + 20),
            Size = new Size(width, 35),
            PlaceholderText = "Footer text..."
        };
        embedTab.Controls.Add(_embedFooterTextBox);
        
        _embedFooterIconTextBox = new Guna2TextBox
        {
            Location = new Point(left, top + 60),
            Size = new Size(width, 35),
            PlaceholderText = "Footer icon URL..."
        };
        embedTab.Controls.Add(_embedFooterIconTextBox);
        top += 110;

        int right = 450;
        top = 20;

        var imageLabel = new Guna2Label { Text = "Images:", Location = new Point(right, top), AutoSize = true };
        embedTab.Controls.Add(imageLabel);
        
        var thumbLabel = new Guna2Label { Text = "Thumbnail URL:", Location = new Point(right, top + 25), AutoSize = true };
        embedTab.Controls.Add(thumbLabel);
        
        _embedThumbnailUrlTextBox = new Guna2TextBox
        {
            Location = new Point(right, top + 45),
            Size = new Size(400, 35),
            PlaceholderText = "https://..."
        };
        embedTab.Controls.Add(_embedThumbnailUrlTextBox);
        
        var imgLabel = new Guna2Label { Text = "Image URL:", Location = new Point(right, top + 85), AutoSize = true };
        embedTab.Controls.Add(imgLabel);
        
        _embedImageUrlTextBox = new Guna2TextBox
        {
            Location = new Point(right, top + 105),
            Size = new Size(400, 35),
            PlaceholderText = "https://..."
        };
        embedTab.Controls.Add(_embedImageUrlTextBox);
        top += 160;

        var fieldsLabel = new Guna2Label { Text = "Fields:", Location = new Point(right, top), AutoSize = true };
        embedTab.Controls.Add(fieldsLabel);
        
        _addEmbedFieldButton = new Guna2Button
        {
            Text = "+ Add Field",
            Location = new Point(right + 70, top - 5),
            Size = new Size(100, 30),
            BorderRadius = 5,
            FillColor = Color.FromArgb(70, 70, 70)
        };
        _addEmbedFieldButton.Click += AddEmbedFieldButton_Click;
        embedTab.Controls.Add(_addEmbedFieldButton);
        
        _embedFieldsPanel = new Guna2Panel
        {
            Location = new Point(right, top + 30),
            Size = new Size(400, 200),
            AutoScroll = true
        };
        embedTab.Controls.Add(_embedFieldsPanel);
        top += 240;

        _previewEmbedButton = new Guna2Button
        {
            Text = "👁️ Preview Embed",
            Location = new Point(right, top),
            Size = new Size(150, 45),
            BorderRadius = 8,
            FillColor = Color.FromArgb(70, 70, 70)
        };
        _previewEmbedButton.Click += PreviewEmbedButton_Click;
        embedTab.Controls.Add(_previewEmbedButton);

        _sendEmbedButton = new Guna2Button
        {
            Text = "📤 Send Embed",
            Location = new Point(right + 160, top),
            Size = new Size(150, 45),
            BorderRadius = 8,
            FillColor = Color.FromArgb(88, 101, 242)
        };
        _sendEmbedButton.Click += SendEmbedButton_Click;
        embedTab.Controls.Add(_sendEmbedButton);
    }

    private void SetupImageTab()
    {
        var imageTab = new Guna2TabPage { Text = "🖼️ Image" };
        _tabControl.TabPages.Add(imageTab);

        var urlLabel = new Guna2Label
        {
            Text = "Image URL:",
            Location = new Point(20, 20),
            AutoSize = true
        };
        imageTab.Controls.Add(urlLabel);

        _imageUrlTextBox = new Guna2TextBox
        {
            Location = new Point(20, 45),
            Size = new Size(600, 40),
            PlaceholderText = "Enter image URL or browse file..."
        };
        imageTab.Controls.Add(_imageUrlTextBox);

        _loadImageButton = new Guna2Button
        {
            Text = "📁 Browse",
            Location = new Point(640, 45),
            Size = new Size(120, 40),
            BorderRadius = 8,
            FillColor = Color.FromArgb(70, 70, 70)
        };
        _loadImageButton.Click += LoadImageButton_Click;
        imageTab.Controls.Add(_loadImageButton);

        var previewLabel = new Guna2Label
        {
            Text = "Preview:",
            Location = new Point(20, 100),
            AutoSize = true
        };
        imageTab.Controls.Add(previewLabel);

        _imagePreviewBox = new Guna2PictureBox
        {
            Location = new Point(20, 125),
            Size = new Size(400, 300),
            BorderStyle = BorderStyle.FixedSingle,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(40, 40, 40)
        };
        imageTab.Controls.Add(_imagePreviewBox);

        _imageUrlTextBox.TextChanged += (s, e) => LoadImagePreview();

        var captionLabel = new Guna2Label
        {
            Text = "Caption (optional):",
            Location = new Point(450, 100),
            AutoSize = true
        };
        imageTab.Controls.Add(captionLabel);

        _imageCaptionTextBox = new Guna2TextBox
        {
            Location = new Point(450, 125),
            Size = new Size(400, 100),
            Multiline = true,
            PlaceholderText = "Add a caption with Discord formatting..."
        };
        imageTab.Controls.Add(_imageCaptionTextBox);

        _sendImageButton = new Guna2Button
        {
            Text = "📤 Send Image",
            Location = new Point(450, 350),
            Size = new Size(200, 50),
            BorderRadius = 10,
            FillColor = Color.FromArgb(88, 101, 242),
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };
        _sendImageButton.Click += SendImageButton_Click;
        imageTab.Controls.Add(_sendImageButton);
    }

    private void SetupRawTab()
    {
        var rawTab = new Guna2TabPage { Text = "📝 Raw Output" };
        _tabControl.TabPages.Add(rawTab);

        var outputLabel = new Guna2Label
        {
            Text = "Generated JSON / Markdown:",
            Location = new Point(20, 20),
            AutoSize = true
        };
        rawTab.Controls.Add(outputLabel);

        _rawOutputPanel = new Guna2Panel
        {
            Location = new Point(20, 50),
            Size = new Size(1000, 450)
        };
        rawTab.Controls.Add(_rawOutputPanel);

        _rawOutputTextBox = new Guna2TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Both,
            Font = new Font("Consolas", 10),
            PlaceholderText = "Raw output will appear here..."
        };
        _rawOutputPanel.Controls.Add(_rawOutputTextBox);

        var buttonPanel = new Guna2Panel
        {
            Location = new Point(20, 520),
            Size = new Size(1000, 50)
        };
        rawTab.Controls.Add(buttonPanel);

        _copyRawButton = new Guna2Button
        {
            Text = "📋 Copy to Clipboard",
            Location = new Point(0, 5),
            Size = new Size(180, 40),
            BorderRadius = 8,
            FillColor = Color.FromArgb(70, 70, 70)
        };
        _copyRawButton.Click += (s, e) =>
        {
            if (!string.IsNullOrEmpty(_rawOutputTextBox.Text))
            {
                Clipboard.SetText(_rawOutputTextBox.Text);
                MessageBox.Show("Copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };
        buttonPanel.Controls.Add(_copyRawButton);

        _clearOutputButton = new Guna2Button
        {
            Text = "🗑️ Clear",
            Location = new Point(190, 5),
            Size = new Size(120, 40),
            BorderRadius = 8,
            FillColor = Color.FromArgb(235, 69, 69)
        };
        _clearOutputButton.Click += (s, e) => _rawOutputTextBox.Clear();
        buttonPanel.Controls.Add(_clearOutputButton);

        var generateButtonsLabel = new Guna2Label
        {
            Text = "Generate Raw Output:",
            Location = new Point(350, 12),
            AutoSize = true
        };
        buttonPanel.Controls.Add(generateButtonsLabel);

        var genMsgButton = new Guna2Button
        {
            Text = "Message JSON",
            Location = new Point(480, 5),
            Size = new Size(130, 40),
            BorderRadius = 8,
            FillColor = Color.FromArgb(88, 101, 242)
        };
        genMsgButton.Click += (s, e) => GenerateMessageJson();
        buttonPanel.Controls.Add(genMsgButton);

        var genEmbedButton = new Guna2Button
        {
            Text = "Embed JSON",
            Location = new Point(620, 5),
            Size = new Size(130, 40),
            BorderRadius = 8,
            FillColor = Color.FromArgb(88, 101, 242)
        };
        genEmbedButton.Click += (s, e) => GenerateEmbedJson();
        buttonPanel.Controls.Add(genEmbedButton);

        var genMdButton = new Guna2Button
        {
            Text = "Markdown",
            Location = new Point(760, 5),
            Size = new Size(130, 40),
            BorderRadius = 8,
            FillColor = Color.FromArgb(88, 101, 242)
        };
        genMdButton.Click += (s, e) => GenerateMarkdown();
        buttonPanel.Controls.Add(genMdButton);
    }

    private void AddEmbedFieldButton_Click(object? sender, EventArgs e)
    {
        var fieldPanel = new Guna2Panel
        {
            Size = new Size(380, 80),
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 5, 0, 5),
            Tag = _embedFields.Count
        };

        var nameBox = new Guna2TextBox
        {
            Location = new Point(5, 5),
            Size = new Size(370, 30),
            PlaceholderText = "Field name..."
        };
        fieldPanel.Controls.Add(nameBox);

        var valueBox = new Guna2TextBox
        {
            Location = new Point(5, 40),
            Size = new Size(280, 30),
            PlaceholderText = "Field value..."
        };
        fieldPanel.Controls.Add(valueBox);

        var inlineCheck = new Guna2CheckBox
        {
            Text = "Inline",
            Location = new Point(290, 42),
            Checked = true
        };
        fieldPanel.Controls.Add(inlineCheck);

        _embedFieldsPanel.Controls.Add(fieldPanel);
        _embedFields.Add(new EmbedField());
    }

    private void PreviewEmbedButton_Click(object? sender, EventArgs e)
    {
        var embed = BuildEmbed();
        
        var sb = new StringBuilder();
        sb.AppendLine("╔══════════════════════════════════════╗");
        sb.AppendLine("║           EMBED PREVIEW               ║");
        sb.AppendLine("╚══════════════════════════════════════╝");
        
        if (!string.IsNullOrEmpty(embed.Author?.Name))
        {
            sb.AppendLine($"👤 {embed.Author.Name}");
            sb.AppendLine(new string('─', 40));
        }
        
        if (!string.IsNullOrEmpty(embed.Title))
        {
            sb.AppendLine($"**{embed.Title}**");
        }
        
        if (!string.IsNullOrEmpty(embed.Description))
        {
            sb.AppendLine(embed.Description);
        }
        
        if (embed.Fields.Count > 0)
        {
            sb.AppendLine();
            int i = 0;
            foreach (var field in embed.Fields)
            {
                sb.AppendLine($"**{field.Name}**");
                sb.AppendLine(field.Value);
                if (i < embed.Fields.Count - 1) sb.AppendLine();
                i++;
            }
        }
        
        if (!string.IsNullOrEmpty(embed.Footer?.Text))
        {
            sb.AppendLine();
            sb.AppendLine(new string('─', 40));
            sb.AppendLine($"📝 {embed.Footer.Text}");
        }
        
        _rawOutputTextBox.Text = sb.ToString();
    }

    private async void ConnectButton_Click(object? sender, EventArgs e)
    {
        var token = _botTokenTextBox.Text.Trim();
        var channelIdStr = _channelIdTextBox.Text.Trim();
        
        if (string.IsNullOrEmpty(token))
        {
            MessageBox.Show("Please enter a bot token.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        if (string.IsNullOrEmpty(channelIdStr) || !ulong.TryParse(channelIdStr, out var channelId))
        {
            MessageBox.Show("Please enter a valid channel ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        try
        {
            _connectButton.Enabled = false;
            _connectionStatusLabel.Text = "⏳ Connecting...";
            _connectionStatusLabel.ForeColor = Color.Yellow;
            
            _discordClient = new DiscordClient(token, channelId);
            await _discordClient.ConnectAsync();
            
            _connectionStatusLabel.Text = "🟢 Connected";
            _connectionStatusLabel.ForeColor = Color.Green;
            _disconnectButton.Enabled = true;
            _connectButton.Enabled = false;
            
            MessageBox.Show("Successfully connected to Discord!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _connectionStatusLabel.Text = "🔴 Connection Failed";
            _connectionStatusLabel.ForeColor = Color.Red;
            _connectButton.Enabled = true;
            MessageBox.Show($"Failed to connect: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void DisconnectButton_Click(object? sender, EventArgs e)
    {
        if (_discordClient != null)
        {
            await _discordClient.DisconnectAsync();
            _discordClient = null;
        }
        
        _connectionStatusLabel.Text = "⚪ Not Connected";
        _connectionStatusLabel.ForeColor = Color.Gray;
        _disconnectButton.Enabled = false;
        _connectButton.Enabled = true;
    }

    private async void SendMessageButton_Click(object? sender, EventArgs e)
    {
        if (_discordClient == null || !_discordClient.IsConnected)
        {
            MessageBox.Show("Please connect to Discord first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        var content = _messageContentTextBox.Text;
        if (string.IsNullOrWhiteSpace(content))
        {
            MessageBox.Show("Please enter a message.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        try
        {
            await _discordClient.SendMessageAsync(content);
            MessageBox.Show("Message sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to send message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void SendEmbedButton_Click(object? sender, EventArgs e)
    {
        if (_discordClient == null || !_discordClient.IsConnected)
        {
            MessageBox.Show("Please connect to Discord first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        try
        {
            var embed = BuildEmbed();
            await _discordClient.SendEmbedAsync(embed);
            MessageBox.Show("Embed sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to send embed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void SendImageButton_Click(object? sender, EventArgs e)
    {
        if (_discordClient == null || !_discordClient.IsConnected)
        {
            MessageBox.Show("Please connect to Discord first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        var imageUrl = _imageUrlTextBox.Text.Trim();
        if (string.IsNullOrEmpty(imageUrl))
        {
            MessageBox.Show("Please enter an image URL or select a file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        try
        {
            await _discordClient.SendImageAsync(imageUrl, _imageCaptionTextBox.Text);
            MessageBox.Show("Image sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to send image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadImageButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Image files|*.jpg;*.jpeg;*.png;*.gif;*.webp;*.bmp|All files|*.*",
            Title = "Select an image"
        };
        
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                _imagePreviewBox.ImageLocation = dialog.FileName;
                _imageUrlTextBox.Text = dialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void LoadImagePreview()
    {
        if (!string.IsNullOrEmpty(_imageUrlTextBox.Text))
        {
            try
            {
                _imagePreviewBox.LoadAsync(_imageUrlTextBox.Text);
            }
            catch
            {
                _imagePreviewBox.Image = null;
            }
        }
    }

    private void ThemeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _currentTheme = _themeComboBox.SelectedItem?.ToString() ?? "Dark";
        ApplyTheme(_currentTheme);
    }

    private void ApplyTheme(string themeName)
    {
        Color bgColor, fgColor, panelColor, accentColor;
        
        switch (themeName)
        {
            case "Light":
                bgColor = Color.FromArgb(255, 255, 255);
                fgColor = Color.FromArgb(30, 30, 30);
                panelColor = Color.FromArgb(245, 245, 245);
                accentColor = Color.FromArgb(88, 101, 242);
                break;
            case "Midnight":
                bgColor = Color.FromArgb(20, 20, 40);
                fgColor = Color.FromArgb(200, 200, 220);
                panelColor = Color.FromArgb(30, 30, 60);
                accentColor = Color.FromArgb(138, 180, 248);
                break;
            case "Ocean":
                bgColor = Color.FromArgb(10, 30, 50);
                fgColor = Color.FromArgb(180, 210, 230);
                panelColor = Color.FromArgb(20, 50, 80);
                accentColor = Color.FromArgb(0, 150, 200);
                break;
            case "Forest":
                bgColor = Color.FromArgb(20, 35, 20);
                fgColor = Color.FromArgb(200, 220, 200);
                panelColor = Color.FromArgb(30, 50, 30);
                accentColor = Color.FromArgb(80, 160, 80);
                break;
            default: // Dark
                bgColor = Color.FromArgb(30, 30, 30);
                fgColor = Color.FromArgb(220, 220, 220);
                panelColor = Color.FromArgb(40, 40, 40);
                accentColor = Color.FromArgb(88, 101, 242);
                break;
        }
        
        this.BackColor = bgColor;
        this.ForeColor = fgColor;
        
        _sidebarPanel.BackColor = panelColor;
        _mainPanel.BackColor = bgColor;
        
        foreach (Control ctrl in this.Controls)
        {
            UpdateControlTheme(ctrl, bgColor, fgColor, panelColor, accentColor);
        }
    }

    private void UpdateControlTheme(Control ctrl, Color bg, Color fg, Color panel, Color accent)
    {
        if (ctrl is Guna2Panel panelCtrl)
        {
            panelCtrl.FillColor = panel;
        }
        else if (ctrl is Guna2Label label)
        {
            label.ForeColor = fg;
        }
        else if (ctrl is Guna2Button btn)
        {
            if (btn.FillColor == Color.FromArgb(88, 101, 242) || 
                btn.FillColor == Color.FromArgb(235, 69, 69))
            {
                // Keep accent colors
            }
            else
            {
                btn.FillColor = panel;
            }
        }
        
        foreach (Control child in ctrl.Controls)
        {
            UpdateControlTheme(child, bg, fg, panel, accent);
        }
    }

    private Embed BuildEmbed()
    {
        var embed = new Embed
        {
            Title = _embedTitleTextBox.Text,
            Description = _embedDescriptionTextBox.Text,
            Url = _embedUrlTextBox.Text,
            Color = _embedColorPicker.Color,
            Timestamp = _embedTimestampPicker.Value
        };
        
        if (!string.IsNullOrEmpty(_embedAuthorTextBox.Text))
        {
            embed.Author = new EmbedAuthor
            {
                Name = _embedAuthorTextBox.Text,
                IconUrl = _embedAuthorIconTextBox.Text
            };
        }
        
        if (!string.IsNullOrEmpty(_embedFooterTextBox.Text))
        {
            embed.Footer = new EmbedFooter
            {
                Text = _embedFooterTextBox.Text,
                IconUrl = _embedFooterIconTextBox.Text
            };
        }
        
        if (!string.IsNullOrEmpty(_embedThumbnailUrlTextBox.Text))
        {
            embed.Thumbnail = new EmbedImage { Url = _embedThumbnailUrlTextBox.Text };
        }
        
        if (!string.IsNullOrEmpty(_embedImageUrlTextBox.Text))
        {
            embed.Image = new EmbedImage { Url = _embedImageUrlTextBox.Text };
        }
        
        int fieldIndex = 0;
        foreach (Control ctrl in _embedFieldsPanel.Controls)
        {
            if (ctrl is Guna2Panel fieldPanel && fieldIndex < _embedFields.Count)
            {
                string? name = null, value = null;
                bool inline = true;
                
                foreach (Control child in fieldPanel.Controls)
                {
                    if (child is Guna2TextBox textBox)
                    {
                        var tag = textBox.Tag?.ToString();
                        if (tag == "name") name = textBox.Text;
                        else if (tag == "value") value = textBox.Text;
                    }
                    else if (child is Guna2CheckBox checkBox && checkBox.Text == "Inline")
                    {
                        inline = checkBox.Checked;
                    }
                }
                
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    embed.Fields.Add(new EmbedField { Name = name, Value = value, Inline = inline });
                }
                fieldIndex++;
            }
        }
        
        return embed;
    }

    private void GenerateMessageJson()
    {
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(
            new { content = _messageContentTextBox.Text },
            Newtonsoft.Json.Formatting.Indented
        );
        _rawOutputTextBox.Text = json;
    }

    private void GenerateEmbedJson()
    {
        var embed = BuildEmbed();
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(
            new { embeds = new[] { embed } },
            Newtonsoft.Json.Formatting.Indented
        );
        _rawOutputTextBox.Text = json;
    }

    private void GenerateMarkdown()
    {
        _rawOutputTextBox.Text = _messageContentTextBox.Text;
    }

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
