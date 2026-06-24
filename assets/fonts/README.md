# Font Assets

## Font Files

All fonts use IBM PC OEM Code Page 437 character set in a 16×16 grid (256 characters).

### OEM437_8.png (3.7 KB)

**Source**: [wpf-console OEM437_8.png](https://github.com/treytomes/wpf-console/blob/cd93129549ad4ddc11ed29ff5e3e1037574762da/Terminal/Resources/OEM437_8.png)

- **Character Size**: 8×8 pixels per character
- **Image Size**: 128×128 pixels total
- **Best For**: CGA text mode, low-res displays, authentic 8-bit look
- **Usage**: `CharacterWidth=8, CharacterHeight=8`

### OEM437_12.png (4.1 KB)

**Source**: [wpf-console OEM437_12.png](https://github.com/treytomes/wpf-console/blob/cd93129549ad4ddc11ed29ff5e3e1037574762da/Terminal/Resources/OEM437_12.png)

- **Character Size**: 8×12 pixels per character
- **Image Size**: 128×192 pixels total
- **Best For**: EGA text mode, improved readability, VT100 terminals
- **Usage**: `CharacterWidth=8, CharacterHeight=12`

### OEM437_16.png (4.4 KB)

**Source**: [wpf-console OEM437_16.png](https://github.com/treytomes/wpf-console/blob/cd93129549ad4ddc11ed29ff5e3e1037574762da/Terminal/Resources/OEM437_16.png)

- **Character Size**: 8×16 pixels per character
- **Image Size**: 128×256 pixels total
- **Best For**: VGA text mode, maximum readability, modern displays
- **Usage**: `CharacterWidth=8, CharacterHeight=16`

### Character Set

- **Code Page**: CP437 (IBM PC Extended ASCII)
- **Characters**: 256 characters (0x00 - 0xFF)
- **Grid Layout**: 16 columns × 16 rows (all fonts)

### Godot Import Settings

The `.import` file is configured to preserve the pixelated aesthetic:

```ini
# Critical settings for retro look
texture_filter=0          # TEXTURE_FILTER_NEAREST (no anti-aliasing)
mipmaps/generate=false    # Disable mipmaps (keeps pixels sharp)
compress/mode=0           # Lossless compression
detect_3d/compress_to=0   # No automatic 3D compression
```

### Usage in Godot

#### Recommended: Using TerminalConfig Resource

The project includes a `TerminalConfig` resource class that manages font selection and terminal settings:

```csharp
using RetroTerm.Terminal;

// Load or create a config
var config = TerminalConfig.CreateIBMPC();  // 80×25, 8×8 font
// or
var config = TerminalConfig.CreateVGA();    // 80×25, 8×16 font
// or
var config = TerminalConfig.CreateVT100();  // 80×24, 8×12 font

// Access font properties
Texture2D fontTexture = config.FontAtlas;
int charWidth = config.CharacterWidth;
int charHeight = config.CharacterHeight;

// Get character source rectangle
Rect2 charRect = config.GetCharacterRect('A');

// Calculate display size
int displayWidth = config.DisplayWidth;   // Columns × CharWidth × Scale
int displayHeight = config.DisplayHeight; // Rows × CharHeight × Scale
```

See [scripts/Terminal/TerminalConfig.cs](../../scripts/Terminal/TerminalConfig.cs) for complete API.

#### Method 1: Using BitmapFont Resource

Create a BitmapFont resource that references this texture:

```csharp
// C# example
var bitmapFont = new BitmapFont();
var texture = GD.Load<Texture2D>("res://assets/fonts/OEM437_8.png");

// Configure character grid
const int charWidth = 8;
const int charHeight = 8;
const int columns = 16;

// Add each character to the font
for (int i = 0; i < 256; i++)
{
    int col = i % columns;
    int row = i / columns;
    
    bitmapFont.AddCharacter(
        (char)i,
        texture,
        new Rect2(col * charWidth, row * charHeight, charWidth, charHeight)
    );
}
```

#### Method 2: Manual Rendering with AtlasTexture

For more control over character rendering:

```csharp
public partial class TerminalRenderer : Node2D
{
    private Texture2D _fontTexture;
    private const int CharWidth = 8;
    private const int CharHeight = 8;
    private const int Columns = 16;
    
    public override void _Ready()
    {
        _fontTexture = GD.Load<Texture2D>("res://assets/fonts/OEM437_8.png");
    }
    
    private Rect2 GetCharRect(char c)
    {
        int charCode = (int)c;
        int col = charCode % Columns;
        int row = charCode / Columns;
        return new Rect2(col * CharWidth, row * CharWidth, CharWidth, CharHeight);
    }
    
    public override void _Draw()
    {
        // Draw character at position
        char ch = 'A';
        var sourceRect = GetCharRect(ch);
        DrawTextureRectRegion(
            _fontTexture,
            new Rect2(0, 0, CharWidth, CharHeight),
            sourceRect
        );
    }
}
```

#### Method 3: Using Sprite2D Array

For a terminal display, create a grid of Sprite2D nodes:

```csharp
public partial class TerminalDisplay : Node2D
{
    private Texture2D _fontTexture;
    private Sprite2D[,] _characterGrid;
    
    public void SetCharacter(int x, int y, char c)
    {
        var sprite = _characterGrid[x, y];
        int charCode = (int)c;
        
        // Set the region to display
        sprite.RegionEnabled = true;
        sprite.RegionRect = GetCharRect(c);
        sprite.Texture = _fontTexture;
    }
}
```

### CP437 Character Map Reference

Notable characters in the OEM437 charset:

- **0x00-0x1F**: Box-drawing, card suits, musical notes, arrows
- **0x20-0x7E**: Standard ASCII printable characters
- **0x7F**: House symbol
- **0x80-0x9F**: Accented letters (Latin)
- **0xA0-0xDF**: More box-drawing and block characters
- **0xE0-0xFF**: Greek letters and mathematical symbols

### Performance Considerations

For a terminal with many characters (e.g., 80×25 = 2000 characters):

1. **Use Sprite2D with shared texture** - Reuses the same texture atlas
2. **Enable TextureFilter.Nearest** - Essential for crisp pixels
3. **Object pooling** - Reuse Sprite2D nodes instead of creating/destroying
4. **Custom drawing** - Use `_Draw()` for best performance with many characters

### Creating Additional Font Sizes

To scale the font while maintaining pixel-perfect appearance:

```csharp
// Integer scaling only (2x, 3x, 4x) to avoid blurring
var sprite = new Sprite2D();
sprite.Texture = fontTexture;
sprite.RegionEnabled = true;
sprite.RegionRect = GetCharRect('A');
sprite.Scale = new Vector2(2, 2);  // 2x scale = 16×16 pixels
sprite.TextureFilter = TextureFilterEnum.Nearest;  // Critical!
```

### Visual Theme Integration

This font works perfectly with these retro themes:

- **IBM PC** (CGA/EGA/VGA) - Native font
- **DOS** - Standard DOS font
- **C64** - Can substitute for PETSCII (with remapping)
- **Custom retro** - Any 8×8 character-based terminal

### References

- [Code Page 437](https://en.wikipedia.org/wiki/Code_page_437) - Wikipedia
- [IBM PC Character Set](https://www.ascii-codes.com/) - ASCII reference
- [Godot BitmapFont](https://docs.godotengine.org/en/stable/classes/class_bitmapfont.html) - Documentation
