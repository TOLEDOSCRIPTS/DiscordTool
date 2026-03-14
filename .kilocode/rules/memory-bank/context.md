# Active Context: Discord Message & Embed Tool

## Current State

**Project Status**: ✅ Complete

Created a Windows Forms desktop application in Visual Studio 2022 with Guna2 UI framework for Discord message/embed creation and sending.

## Recently Completed

- [x] Base Next.js 16 setup with App Router
- [x] TypeScript configuration with strict mode
- [x] Tailwind CSS 4 integration
- [x] ESLint configuration
- [x] Memory bank documentation
- [x] Recipe system for common features
- [x] **Discord Tool - Windows Forms app with Guna2**

## Current Structure

### Next.js Web Project (`src/`)
| File/Directory | Purpose | Status |
|----------------|---------|--------|
| `src/app/page.tsx` | Home page | ✅ Ready |
| `src/app/layout.tsx` | Root layout | ✅ Ready |
| `src/app/globals.css` | Global styles | ✅ Ready |
| `.kilocode/` | AI context & recipes | ✅ Ready |

### Discord Tool Desktop App (`DiscordTool/`)
| File/Directory | Purpose | Status |
|----------------|---------|--------|
| `DiscordTool.sln` | VS Solution | ✅ Ready |
| `DiscordTool/DiscordTool.csproj` | Project file with Guna2 | ✅ Ready |
| `DiscordTool/MainForm.cs` | Main UI form | ✅ Complete |
| `DiscordTool/Discord/DiscordClient.cs` | Discord API client | ✅ Complete |
| `DiscordTool/Discord/DiscordMarkdown.cs` | Markdown formatter | ✅ Complete |
| `DiscordTool/appsettings.json` | App configuration | ✅ Ready |

## Current Focus

Desktop Application: Discord Message & Embed Tool

### Discord Tool Features:
- **Message Tab**: Send plain messages with Discord Markdown formatting
- **Embed Builder Tab**: Create rich embeds with title, description, author, footer, images, fields
- **Image Tab**: Send images with captions
- **Raw Output Tab**: View generated JSON/Markdown
- **Themes**: Dark, Light, Midnight, Ocean, Forest themes
- **Formatting**: Bold, Italic, Underline, Strikethrough, Spoilers, Code, Headers, Lists, Block quotes, Links

## Quick Start Guide

### To run the Discord Tool:

1. Open `DiscordTool.sln` in Visual Studio 2022
2. Restore NuGet packages (Guna2.UI.WinForms, DSharpPlus, Newtonsoft.Json)
3. Build and run the application
4. Enter your Discord bot token and channel ID
5. Connect and start sending messages/embeds

### Discord Markdown Supported:
| Format | Syntax |
|--------|--------|
| Bold | `**text**` |
| Italic | `*text*` |
| Bold Italic | `***text***` |
| Underline | `__text__` |
| Strikethrough | `~~text~~` |
| Spoiler | `||text||` |
| Header 1 | `# text` |
| Header 2 | `## text` |
| Header 3 | `### text` |
| Block Quote | `> text` |
| Code | `` `code` `` |
| Code Block | ` ``` ``` ` |
| Link | `[text](url)` |
| List | `- item` |

## Available Recipes

| Recipe | File | Use Case |
|--------|------|----------|
| Add Database | `.kilocode/recipes/add-database.md` | Data persistence with Drizzle + SQLite |

## Pending Improvements

- [ ] Add more recipes (auth, email, etc.)
- [ ] Add example components
- [ ] Add testing setup recipe

## Session History

| Date | Changes |
|------|---------|
| Initial | Template created with base setup |
