# Terminal Settings

This directory contains user-configurable terminal behavior settings resources.

## Available Settings Presets

### default.tres
**Balanced settings for general use**

- Command history: 100 commands
- Tab width: 4 spaces
- Cursor: Block (Insert mode)
- Selection: Keep after copy
- Paste: Strip newlines, convert tabs
- Max paste: 1024 characters
- Auto-scroll: Enabled
- Bell: Disabled

Use for: General terminal usage, command-line work

### programming.tres
**Optimized for coding and scripting**

- Command history: 500 commands (more for development)
- Tab width: 4 spaces (modern standard)
- Cursor: Block (Insert mode)
- Selection: Keep after copy
- Paste: **Preserve newlines and tabs** (for code)
- Max paste: 10000 characters (large code blocks)
- Auto-scroll: Enabled
- Bell: Disabled

Use for: Software development, script editing, code review

### classic.tres
**Traditional terminal behavior (80s/90s style)**

- Command history: 50 commands (limited like old systems)
- Tab width: 8 spaces (traditional)
- Cursor: Block (Insert mode)
- Selection: **Clear after copy** (traditional behavior)
- Paste: Strip newlines, convert tabs
- Max paste: 512 characters (conservative)
- Auto-scroll: Enabled
- Bell: **Enabled** (classic terminal beep)

Use for: Authentic retro experience, DOS-like behavior

## Settings Reference

### Input Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `CommandHistorySize` | int (10-1000) | 100 | Number of commands to remember |
| `TabWidth` | int (2-8) | 4 | Spaces per Tab key press |
| `DefaultCursorStyle` | CursorStyle | Block | Initial cursor shape (Block/Underline/VerticalBar) |

### Selection and Clipboard

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ClearSelectionAfterCopy` | bool | false | Whether to deselect after Ctrl+C |
| `PasteStripNewlines` | bool | true | Convert newlines to spaces when pasting |
| `PasteConvertTabs` | bool | true | Convert tabs to spaces when pasting |
| `MaxPasteLength` | int (0-10000) | 1024 | Maximum characters allowed in paste (0=unlimited) |

### Output Settings

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `AutoScrollOnOutput` | bool | true | Scroll to bottom when new output arrives |
| `BellEnabled` | bool | false | Play sound for bell character (ASCII 7) |

## Using Settings in Scenes

### In Godot Editor (Inspector)

1. Select TerminalDisplay node
2. Find "Settings" property
3. Choose "Load" and select a preset (.tres file)
4. Or create "New TerminalSettings" for custom values

### In Code

```csharp
// Load preset
var settings = GD.Load<TerminalSettings>("res://assets/settings/default.tres");
display.Settings = settings;

// Create programmatically
var settings = TerminalSettings.CreateDefault();
// or
var settings = TerminalSettings.CreateProgramming();
// or
var settings = TerminalSettings.CreateClassic();

display.Settings = settings;

// Access setting values
int historySize = display.Settings.CommandHistorySize;
bool autoCopy = display.Settings.ClearSelectionAfterCopy;
```

## Creating Custom Settings

### Method 1: In Godot Editor

1. Create new Resource in inspector
2. Choose "TerminalSettings" as type
3. Adjust properties to your liking
4. Save as .tres file

### Method 2: Copy and Modify

1. Copy an existing .tres file
2. Edit values in text editor
3. Change UID to avoid conflicts
4. Load in your scene

### Method 3: Code

```csharp
var customSettings = new TerminalSettings
{
    CommandHistorySize = 200,
    TabWidth = 2,
    DefaultCursorStyle = CursorStyle.VerticalBar,
    ClearSelectionAfterCopy = true,
    PasteStripNewlines = false,
    MaxPasteLength = 5000
};

// Save to file (optional)
ResourceSaver.Save(customSettings, "res://assets/settings/custom.tres");
```

## Settings vs Config

**TerminalSettings** (this directory):
- User behavior preferences
- Runtime-changeable
- Per-user customization
- Example: History size, tab width, bell enabled

**TerminalConfig** (`assets/themes/`):
- Display/visual properties
- Terminal dimensions, fonts, colors
- Theme presets (IBM PC, C64, VGA, VT100)
- Example: 80×25 columns/rows, font atlas, palette

A terminal display typically has **both**:
```csharp
[Export] public TerminalConfig? Config { get; set; }     // How it looks
[Export] public TerminalSettings? Settings { get; set; } // How it behaves
```

## Future Settings (Phase 2)

Settings planned for later phases:

- **Scrollback buffer size**: Lines of output history
- **Persistent history**: Save/load command history between sessions
- **Custom key bindings**: Remap keyboard shortcuts
- **Word wrap mode**: How long lines are handled
- **Multi-line input**: Enable/disable multi-line commands

These will be added as features are implemented.

## Notes

- Settings are stored as Godot Resources (.tres files)
- Can be edited in Godot inspector or text editor
- Changes take effect immediately when Settings property is set
- Default values are sensible for most use cases
- Classic preset matches traditional terminal behavior (DOS, Unix shells)
- Programming preset optimized for development workflows
