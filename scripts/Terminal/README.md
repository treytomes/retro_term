# Terminal Scripts

Core terminal emulation and rendering logic.

## TerminalConfig.cs

**Type**: `Resource` (Godot GlobalClass)

Configuration resource for terminal display settings. Defines visual and behavioral properties including dimensions, fonts, colors, and CRT effects.

### Key Features

- ✅ **Godot Resource** - Can be saved as `.tres` files for presets
- ✅ **Inspector Editable** - All properties exposed with proper hints
- ✅ **Multiple Font Support** - References OEM437 fonts (8, 12, 16 pixel heights)
- ✅ **Preset Themes** - Factory methods for IBM PC, C64, VT100, VGA
- ✅ **CRT Effects** - Configurable scanlines and curvature
- ✅ **Calculated Properties** - Auto-compute display size, atlas dimensions

### Usage

#### Creating Configurations

```csharp
using RetroTerm.Terminal;

// Use preset
var config = TerminalConfig.CreateIBMPC();

// Or create custom
var config = new TerminalConfig
{
    Columns = 80,
    Rows = 25,
    CharacterWidth = 8,
    CharacterHeight = 8,
    DisplayScale = 2,
    FontAtlas = GD.Load<Texture2D>("res://assets/fonts/OEM437_8.png"),
    ForegroundColor = Colors.Green,
    BackgroundColor = Colors.Black
};
```

#### Accessing Properties

```csharp
// Font properties
Texture2D font = config.FontAtlas;
int charW = config.CharacterWidth;      // 8
int charH = config.CharacterHeight;     // 8, 12, or 16

// Display properties
int displayW = config.DisplayWidth;     // Columns × CharW × Scale
int displayH = config.DisplayHeight;    // Rows × CharH × Scale

// Get character rectangle from atlas
Rect2 rectA = config.GetCharacterRect('A');  // (8, 64, 8, 8) for char 0x41
```

#### Saving as Resource

In Godot editor:
1. Create new `TerminalConfig` in inspector
2. Configure properties
3. Save as `res://assets/themes/my_theme.tres`

In code:
```csharp
var config = TerminalConfig.CreateVGA();
ResourceSaver.Save(config, "res://assets/themes/vga_theme.tres");
```

### Available Presets

#### `CreateIBMPC()`
- **80×25** terminal
- **8×8** font (OEM437_8.png)
- **Scale**: 2× (1280×400 display)
- **Colors**: Light gray on black
- **Style**: Classic DOS

#### `CreateCommodore64()`
- **40×25** terminal
- **8×8** font (OEM437_8.png)
- **Scale**: 3× (960×600 display)
- **Colors**: Light blue on dark blue
- **Style**: C64 PETSCII aesthetic

#### `CreateVT100()`
- **80×24** terminal
- **8×12** font (OEM437_12.png)
- **Scale**: 2× (1280×576 display)
- **Colors**: Green on black
- **Style**: Unix terminal

#### `CreateVGA()`
- **80×25** terminal
- **8×16** font (OEM437_16.png)
- **Scale**: 1× (640×400 display)
- **Colors**: Light gray on black
- **Style**: VGA text mode

### Configuration Properties

#### Terminal Dimensions
| Property | Type | Range | Default | Description |
|----------|------|-------|---------|-------------|
| `Columns` | int | 20-132 | 80 | Character columns |
| `Rows` | int | 20-60 | 25 | Character rows |

#### Character Properties
| Property | Type | Range | Default | Description |
|----------|------|-------|---------|-------------|
| `CharacterWidth` | int | 4-16 | 8 | Char cell width (pixels) |
| `CharacterHeight` | int | 8-16 | 8 | Char cell height (pixels) |
| `DisplayScale` | int | 1-4 | 2 | Integer scale multiplier |

#### Font Atlas
| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `FontAtlas` | Texture2D | null | Character set texture |
| `FontAtlasColumns` | int | 16 | Columns in atlas grid |
| `FontAtlasRows` | int | 16 | Rows in atlas grid |

#### Colors
| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ForegroundColor` | Color | LightGray | Text color |
| `BackgroundColor` | Color | Black | Background color |
| `CursorColor` | Color | White | Cursor color |
| `CursorBlinkRate` | float | 0.5 | Blink rate in seconds (0=off) |

#### CRT Effects
| Property | Type | Range | Default | Description |
|----------|------|-------|---------|-------------|
| `EnableScanlines` | bool | - | true | Show scanline effect |
| `ScanlineIntensity` | float | 0.0-1.0 | 0.15 | Scanline opacity |
| `EnableCurvature` | bool | - | false | Curve screen edges |
| `CurvatureAmount` | float | 0.0-1.0 | 0.15 | Curvature strength |

### Calculated Properties (Read-Only)

```csharp
int DisplayWidth { get; }       // Columns × CharacterWidth × DisplayScale
int DisplayHeight { get; }      // Rows × CharacterHeight × DisplayScale
int FontAtlasWidth { get; }     // FontAtlasColumns × CharacterWidth
int FontAtlasHeight { get; }    // FontAtlasRows × CharacterHeight
```

### Methods

#### `GetCharacterRect(int charCode)`

Returns the source rectangle for a character in the font atlas.

```csharp
// Get rectangle for 'A' (ASCII 65 = 0x41)
Rect2 rect = config.GetCharacterRect('A');
// Returns: (8, 64, 8, 8) for 8×8 font
// Position: col=1, row=4 → (1*8, 4*8, 8, 8)
```

**Parameters:**
- `charCode` - Character code (0-255 for CP437)

**Returns:** `Rect2` defining position in atlas (x, y, width, height)

### Example: Terminal Renderer

```csharp
public partial class TerminalDisplay : Control
{
    [Export]
    public TerminalConfig? Config { get; set; }
    
    public override void _Ready()
    {
        Config ??= TerminalConfig.CreateIBMPC();
        
        // Set display size
        CustomMinimumSize = new Vector2(
            Config.DisplayWidth,
            Config.DisplayHeight
        );
    }
    
    public override void _Draw()
    {
        if (Config?.FontAtlas == null) return;
        
        // Draw character 'A' at position (0, 0)
        char ch = 'A';
        Rect2 sourceRect = Config.GetCharacterRect(ch);
        Rect2 destRect = new Rect2(
            0, 0,
            Config.CharacterWidth * Config.DisplayScale,
            Config.CharacterHeight * Config.DisplayScale
        );
        
        DrawTextureRectRegion(
            Config.FontAtlas,
            destRect,
            sourceRect
        );
    }
}
```

### Font Selection Guide

Choose font based on target aesthetic:

| Font | Height | Best For | Readability | Authentic |
|------|--------|----------|-------------|-----------|
| OEM437_8.png | 8px | CGA, C64, retro | ⭐⭐ | ⭐⭐⭐⭐⭐ |
| OEM437_12.png | 12px | EGA, VT100 | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| OEM437_16.png | 16px | VGA, modern | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |

### Aspect Ratio Notes

Real CRT text modes stretched characters vertically:
- **CGA 80×25**: 640×200 → 16:5 (3.2:1) - very wide
- **EGA 80×25**: 640×350 → ~1.83:1
- **VGA 80×25**: 720×400 → 1.8:1

With our square pixels:
- **8×8 font** at 80×25 = 640×200 (appears letterboxed on 4:3)
- **8×12 font** at 80×25 = 640×300 (better 4:3 approximation)
- **8×16 font** at 80×25 = 640×400 (closest to true 4:3)

Use `DisplayScale` and window stretch mode to fill modern displays.

## Future Components

- `Terminal.cs` - Core terminal state (character buffer, cursor)
- `TerminalDisplay.cs` - Godot rendering node
- `TerminalInput.cs` - Keyboard input handler
- `TerminalTheme.cs` - Extended theme with color palettes
