# Terminal Color Palettes

This directory contains color palette resources for the terminal display.

## Available Palettes

### ansi_default.tres
**Standard ANSI/VGA 256-color palette**

- **Colors 0-7**: Normal intensity ANSI colors
  - 0: Black
  - 1: Red
  - 2: Green
  - 3: Yellow
  - 4: Blue
  - 5: Magenta
  - 6: Cyan
  - 7: White (light gray)

- **Colors 8-15**: Bright intensity ANSI colors
  - 8: Bright Black (dark gray)
  - 9: Bright Red
  - 10: Bright Green
  - 11: Bright Yellow
  - 12: Bright Blue
  - 13: Bright Magenta
  - 14: Bright Cyan
  - 15: Bright White

- **Colors 16-255**: Grayscale ramp (for extended 256-color support)

This is the default palette used by most terminal configurations.

### commodore64.tres
**Commodore 64 inspired color palette**

Authentic colors from the legendary Commodore 64 home computer:
- 0: Black
- 1: White
- 2: Red
- 3: Cyan
- 4: Purple
- 5: Green
- 6: Blue
- 7: Yellow
- 8: Orange
- 9: Brown
- 10: Light Red
- 11: Dark Gray
- 12: Gray
- 13: Light Green
- 14: Light Blue
- 15: Light Gray

Colors 16-255 are filled with a grayscale ramp for compatibility.

## Creating Custom Palettes

To create your own palette:

1. Create a new `.tres` file in this directory
2. Copy the structure from `ansi_default.tres`
3. Modify the `Colors = PackedColorArray(...)` values
4. Each color is 4 floats: `R, G, B, A` (0.0 to 1.0)
5. Must contain exactly 256 colors (1024 float values total)

**Example custom color**:
```
// Bright orange: RGB(255, 128, 0)
1.0, 0.5, 0.0, 1.0
```

## Usage in Themes

Reference a palette in a `TerminalConfig` resource:

```
[ext_resource type="Resource" uid="uid://ansi_default" path="res://assets/palettes/ansi_default.tres" id="palette_resource"]

[resource]
script = ExtResource("1_script")
Palette = ExtResource("palette_resource")
```

If no palette is specified, `TerminalDisplay` will create a default ANSI palette at runtime.

## Color Index Reference

When setting colors in code:

```csharp
terminal.CurrentForegroundColor = 10;  // Bright green (ANSI)
terminal.CurrentBackgroundColor = 4;   // Blue (ANSI)
```

See `TerminalPalette.cs` for palette generation code.
