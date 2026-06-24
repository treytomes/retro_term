# Asset Directory

This directory contains all visual and audio assets for the retro terminal project.

## Directory Structure

```
assets/
├── fonts/           # Bitmap fonts for terminal display
│   ├── OEM437_8.png     # IBM PC Code Page 437 (8×8 pixels)
│   └── README.md        # Font usage documentation
├── shaders/         # CRT effects and visual shaders (to be added)
└── themes/          # Theme definitions and resources (to be added)
```

## Current Assets

### Fonts

**OEM437_8.png** (3.7 KB)
- IBM PC OEM Code Page 437 character set
- 8×8 pixels per character
- 256 characters (16×16 grid)
- Import configured for pixel-perfect rendering (no filtering)
- See [fonts/README.md](fonts/README.md) for usage examples

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
