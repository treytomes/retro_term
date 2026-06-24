# Asset Directory

This directory contains all visual and audio assets for the retro terminal project.

## Directory Structure

```
assets/
├── fonts/           # Bitmap fonts for terminal display
│   ├── OEM437_8.png     # IBM PC Code Page 437 (8×8 pixels)
│   ├── OEM437_12.png    # IBM PC Code Page 437 (8×12 pixels)
│   ├── OEM437_16.png    # IBM PC Code Page 437 (8×16 pixels)
│   └── README.md        # Font usage documentation
├── themes/          # Pre-configured terminal themes
│   ├── ibm_pc.tres      # IBM PC DOS (80×25, 8×12)
│   ├── commodore64.tres # Commodore 64 (40×25, 8×8)
│   ├── vt100.tres       # VT100 Unix (80×24, 8×12)
│   ├── vga.tres         # VGA text mode (80×25, 8×16)
│   └── README.md        # Theme usage guide
└── shaders/         # CRT effects and visual shaders (to be added)
```

## Current Assets

### Fonts

**OEM437_8.png** (3.7 KB)
- IBM PC OEM Code Page 437 character set
- 8×8 pixels per character
- 256 characters (16×16 grid)
- Import configured for pixel-perfect rendering (no filtering)

**OEM437_12.png** (4.1 KB)
- 8×12 pixels per character
- Better readability than 8×8

**OEM437_16.png** (4.4 KB)
- 8×16 pixels per character
- Maximum readability

See [fonts/README.md](fonts/README.md) for detailed usage.

### Themes

Pre-configured `TerminalConfig` resource files:

**ibm_pc.tres** - Classic DOS terminal (80×25, 8×12 font)
**commodore64.tres** - C64 PETSCII style (40×25, 8×8 font, blue colors)
**vt100.tres** - Unix terminal (80×24, 8×12 font, green on black)
**vga.tres** - VGA text mode (80×25, 8×16 font)

Load in code:
```csharp
var config = TerminalConfig.LoadIBMPC();
// or
var config = TerminalConfig.LoadTheme("vt100");
// or
var config = GD.Load<TerminalConfig>("res://assets/themes/ibm_pc.tres");
```

See [themes/README.md](themes/README.md) for customization guide.

## Planned Assets

### Shaders (shaders/)
- `crt_effect.gdshader` - Main CRT screen effect
- `scanlines.gdshader` - Scanline overlay
- `phosphor_glow.gdshader` - Phosphor persistence effect
- `screen_curve.gdshader` - Screen curvature distortion
- `color_bleed.gdshader` - Color bleeding effect

### Themes (themes/)
- `commodore64.tres` - C64 light blue on blue
- `apple2.tres` - Apple II green on black
- `ibm_cga.tres` - IBM CGA colors
- `ibm_ega.tres` - IBM EGA colors  
- `ibm_vga.tres` - IBM VGA colors
- `vt100_amber.tres` - Amber terminal
- `vt100_green.tres` - Green terminal
- `custom.tres` - User-customizable template

## Import Settings

All pixel art assets should use these Godot import settings:

```ini
texture_filter=0          # TEXTURE_FILTER_NEAREST
mipmaps/generate=false    # No mipmaps
compress/mode=0           # Lossless
detect_3d/compress_to=0   # No auto-compression
```

This ensures crisp, pixelated rendering without anti-aliasing.

## Asset Guidelines

### Creating New Fonts
- Use 8×8 or 8×16 pixel grids for authentic retro look
- Organize as character atlases (16×16 grid recommended)
- Export as PNG with no compression artifacts
- Test at 1x, 2x, 3x, 4x integer scales only

### Creating Shaders
- Follow Godot shader language conventions
- Provide configurable parameters via shader uniforms
- Test performance on target hardware
- Document all parameters and effects

### Creating Themes
- Use TerminalTheme resource class (to be implemented)
- Include font, colors, and shader parameters
- Test readability at different resolutions
- Provide preview screenshot

## References

- [Godot Shading Language](https://docs.godotengine.org/en/stable/tutorials/shaders/shader_reference/index.html)
- [Code Page 437](https://en.wikipedia.org/wiki/Code_page_437)
- [CRT Shader Tutorial](https://www.shadertoy.com/view/XsjSzR)
- [Retro Terminal Aesthetics](https://int10h.org/oldschool-pc-fonts/)
