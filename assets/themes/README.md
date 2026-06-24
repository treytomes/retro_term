# Terminal Themes

Pre-configured terminal themes saved as Godot Resource files (`.tres`). These can be loaded directly in code or assigned in the Godot inspector.

## Available Themes

### ibm_pc.tres
**Classic IBM PC DOS Terminal** (80×25)

- **Font**: OEM437_12.png (8×12 pixels)
- **Scale**: 2× (1280×576 display)
- **Colors**: Light gray text on black
- **Style**: Standard DOS text mode
- **Scanlines**: 15% intensity
- **Best For**: Authentic DOS/BASIC experience

```csharp
var config = TerminalConfig.LoadIBMPC();
```

---

### commodore64.tres
**Commodore 64 PETSCII Style** (40×25)

- **Font**: OEM437_8.png (8×8 pixels)
- **Scale**: 3× (960×600 display)
- **Colors**: Light blue text on dark blue background
- **Style**: C64 classic aesthetic
- **Scanlines**: 20% intensity
- **Best For**: Retro home computer feel

```csharp
var config = TerminalConfig.LoadCommodore64();
```

---

### vt100.tres
**VT100 Unix Terminal** (80×24)

- **Font**: OEM437_12.png (8×12 pixels)
- **Scale**: 2× (1280×576 display)
- **Colors**: Green text on black (monochrome phosphor)
- **Style**: Classic Unix/Linux terminal
- **Scanlines**: 10% intensity
- **Best For**: Unix shell aesthetic, programming

```csharp
var config = TerminalConfig.LoadVT100();
```

---

### vga.tres
**IBM VGA Text Mode** (80×25)

- **Font**: OEM437_16.png (8×16 pixels)
- **Scale**: 1× (640×400 display, native VGA resolution)
- **Colors**: Light gray text on black
- **Style**: High-resolution PC text mode
- **Scanlines**: 12% intensity
- **Best For**: Maximum readability, modern displays

```csharp
var config = TerminalConfig.LoadVGA();
```

---

## Usage

### In Code

```csharp
using RetroTerm.Terminal;

// Load preset theme
var config = TerminalConfig.LoadIBMPC();

// Or load by name
var config = TerminalConfig.LoadTheme("vt100");

// Or load directly
var config = GD.Load<TerminalConfig>("res://assets/themes/ibm_pc.tres");
```

### In Godot Inspector

1. Select a node with a `TerminalConfig` property
2. In the Inspector, click the dropdown next to the property
3. Select "Load" → Navigate to `res://assets/themes/`
4. Choose a `.tres` file

### In Scene (.tscn file)

```
[ext_resource type="Resource" path="res://assets/themes/ibm_pc.tres" id="1"]

[node name="TerminalDisplay" type="Control"]
script = ExtResource("TerminalDisplay")
Config = ExtResource("1")
```

---

## Customizing Themes

### Edit in Inspector

1. Load a theme in the inspector
2. Click "Make Unique" to create an editable copy
3. Modify properties (colors, scanlines, etc.)
4. Save as a new `.tres` file

### Edit Manually

Open any `.tres` file in a text editor and modify properties:

```
ForegroundColor = Color(0, 1, 0, 1)    # Green
BackgroundColor = Color(0, 0, 0, 1)    # Black
ScanlineIntensity = 0.2                # 20% intensity
EnableCurvature = true                 # Enable screen curve
```

### Create New Theme

```csharp
var custom = new TerminalConfig
{
    Columns = 80,
    Rows = 25,
    CharacterWidth = 8,
    CharacterHeight = 12,
    DisplayScale = 2,
    FontAtlas = GD.Load<Texture2D>("res://assets/fonts/OEM437_12.png"),
    ForegroundColor = new Color(1, 0.5f, 0), // Orange
    BackgroundColor = new Color(0.1f, 0.1f, 0.1f), // Dark gray
    EnableScanlines = true,
    ScanlineIntensity = 0.15f
};

// Save to file
ResourceSaver.Save(custom, "res://assets/themes/custom_orange.tres");
```

---

## Theme Comparison

| Theme | Resolution | Font Height | Columns | Rows | Best For |
|-------|------------|-------------|---------|------|----------|
| **IBM PC** | 1280×576 | 12px | 80 | 25 | DOS programs |
| **C64** | 960×600 | 8px | 40 | 25 | Home computer feel |
| **VT100** | 1280×576 | 12px | 80 | 24 | Unix terminal |
| **VGA** | 640×400 | 16px | 80 | 25 | Readability |

---

## Creating Custom Presets

### Amber Terminal
```csharp
var amber = new TerminalConfig
{
    Columns = 80,
    Rows = 24,
    CharacterWidth = 8,
    CharacterHeight = 12,
    DisplayScale = 2,
    FontAtlas = GD.Load<Texture2D>("res://assets/fonts/OEM437_12.png"),
    ForegroundColor = new Color(1.0f, 0.75f, 0.0f), // Amber
    BackgroundColor = Colors.Black,
    CursorColor = new Color(1.0f, 0.75f, 0.0f),
    EnableScanlines = true,
    ScanlineIntensity = 0.1f
};
ResourceSaver.Save(amber, "res://assets/themes/vt100_amber.tres");
```

### Apple II
```csharp
var apple2 = new TerminalConfig
{
    Columns = 40,
    Rows = 24,
    CharacterWidth = 8,
    CharacterHeight = 8,
    DisplayScale = 3,
    FontAtlas = GD.Load<Texture2D>("res://assets/fonts/OEM437_8.png"),
    ForegroundColor = new Color(0.0f, 1.0f, 0.0f), // Green
    BackgroundColor = Colors.Black,
    EnableScanlines = true,
    ScanlineIntensity = 0.15f
};
ResourceSaver.Save(apple2, "res://assets/themes/apple2.tres");
```

### High Contrast
```csharp
var highContrast = new TerminalConfig
{
    Columns = 80,
    Rows = 25,
    CharacterWidth = 8,
    CharacterHeight = 16,
    DisplayScale = 1,
    FontAtlas = GD.Load<Texture2D>("res://assets/fonts/OEM437_16.png"),
    ForegroundColor = Colors.White,
    BackgroundColor = Colors.Black,
    EnableScanlines = false, // Disable for maximum clarity
    ScanlineIntensity = 0.0f
};
ResourceSaver.Save(highContrast, "res://assets/themes/high_contrast.tres");
```

---

## Theme Properties Reference

All themes support these configurable properties:

### Display
- `Columns` (20-132) - Terminal width in characters
- `Rows` (20-60) - Terminal height in characters
- `DisplayScale` (1-4) - Integer scaling multiplier

### Font
- `FontAtlas` - Character set texture (OEM437_8/12/16.png)
- `CharacterWidth` (4-16) - Character cell width in pixels
- `CharacterHeight` (8-16) - Character cell height in pixels
- `FontAtlasColumns` (8-32) - Grid columns (usually 16)
- `FontAtlasRows` (8-32) - Grid rows (usually 16)

### Colors
- `ForegroundColor` - Text color
- `BackgroundColor` - Background color
- `CursorColor` - Cursor color
- `CursorBlinkRate` (0.0-2.0) - Blink speed in seconds (0 = no blink)

### CRT Effects
- `EnableScanlines` (bool) - Show horizontal scanlines
- `ScanlineIntensity` (0.0-1.0) - Scanline darkness (0 = invisible, 1 = opaque)
- `EnableCurvature` (bool) - Apply screen curvature
- `CurvatureAmount` (0.0-1.0) - Barrel distortion amount

---

## Tips

### Readability vs Authenticity
- Higher font sizes (12px, 16px) = better readability
- Smaller sizes (8px) = more authentic retro look
- Adjust scanline intensity for comfort (0.1-0.2 is subtle)

### Performance
- All themes render at 60 FPS on modern hardware
- Display scaling affects resolution but not performance
- CRT shader effects add minimal overhead

### Accessibility
- Use VGA theme for best readability
- Disable scanlines for vision clarity
- High contrast theme available (create custom)
- Adjust cursor blink rate or disable blinking

---

## See Also

- [TerminalConfig.cs](../../scripts/Terminal/TerminalConfig.cs) - Configuration class
- [Font Assets](../fonts/README.md) - Available fonts
- [Shader Effects](../shaders/README.md) - CRT effects (when implemented)
