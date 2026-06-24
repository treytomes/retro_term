# Scenes

Godot scene files for the retro terminal project.

## Structure

```
scenes/
├── components/              # Reusable UI components
│   └── terminal_display.tscn    # TerminalDisplay Control node
├── test_terminal.tscn      # Test scene for TerminalDisplay
└── TestTerminal.cs         # Test script
```

## Testing TerminalDisplay (Issue #2)

### Manual Testing Steps

1. **Open Project in Godot**:
   ```bash
   godot project.godot
   ```

2. **Open Test Scene**:
   - In Godot editor: Scene → Open Scene
   - Navigate to `res://scenes/test_terminal.tscn`
   - Click "Open"

3. **Run Test Scene**:
   - Press F6 (or click "Run Current Scene")
   - Terminal should display with test text

### Expected Results

**Visual Verification**:
- ✅ Terminal displays at configured size (default: IBM PC 80×25)
- ✅ Background is black
- ✅ Text is light gray
- ✅ Font is pixel-perfect (no anti-aliasing blur)
- ✅ Characters are aligned in a grid
- ✅ Cursor is visible and blinking (white outline)

**Test Text Output**:
```
=== RETRO TERMINAL TEST ===

Terminal initialized successfully!

Testing character rendering:
ABCDEFGHIJKLMNOPQRSTUVWXYZ
abcdefghijklmnopqrstuvwxyz
0123456789 !@#$%^&*()

Testing wrapping and scrolling...
The quick brown fox jumps over the lazy dog.

Current position: (X, Y)
```

**Character Rendering**:
- All uppercase letters visible
- All lowercase letters visible
- All digits visible
- Special characters visible
- Text wraps correctly at line end

**Cursor**:
- Blinking white outline at cursor position
- Blink rate ~0.5 seconds (configurable)
- Positioned after last character written

### Testing Different Themes

Edit `test_terminal.tscn` or modify TestTerminal.cs to load different themes:

```csharp
// In TestTerminal.cs _Ready():
var display = GetNode<TerminalDisplay>("%TerminalDisplay");

// Set theme before Terminal is created
display.Config = TerminalConfig.LoadCommodore64();  // C64 theme
// or
display.Config = TerminalConfig.LoadVT100();        // VT100 green
// or
display.Config = TerminalConfig.LoadVGA();          // VGA 16px font
```

Or set in the inspector:
1. Select TerminalDisplay node
2. In Inspector → Script Variables → Config
3. Click "Load" → Navigate to `res://assets/themes/`
4. Choose a `.tres` file

### Testing Different Scales

Modify the theme `.tres` file:
- `DisplayScale = 1` - Native resolution (small)
- `DisplayScale = 2` - 2× scale (default, good for 1080p)
- `DisplayScale = 3` - 3× scale (large, good for 4K)
- `DisplayScale = 4` - 4× scale (very large)

### Verifying Pixel-Perfect Rendering

**Zoom In** (in Godot editor while running):
- Characters should have sharp, crisp edges
- No blurring or anti-aliasing
- Each pixel clearly defined
- Grid alignment perfect

**Common Issues**:
- ❌ Blurry text → Texture filter not set to NEAREST
- ❌ Misaligned characters → Font atlas dimensions incorrect
- ❌ Wrong colors → Color modulation not applied
- ❌ No cursor → CursorVisible false or blink state issue

### Performance Check

While test scene is running:
- Press F8 (Show Debug) in Godot
- Check FPS counter
- Should maintain 60 FPS easily
- CPU usage should be minimal

### Console Output

Check Godot console for:
```
TestTerminal: Test content written
Terminal size: 80x25
```

If errors appear:
- "FontAtlas is null" → Config not loaded or theme missing font
- "Terminal is null" → TerminalDisplay._Ready() failed

### Interactive Testing

Modify `TestTerminal.cs` to test interactive features:

```csharp
public override void _Ready()
{
    var display = GetNode<TerminalDisplay>("%TerminalDisplay");
    
    // Clear and write custom text
    display.Terminal.Clear();
    display.Terminal.WriteLine("Type something!");
    display.UpdateDisplay();
}

public override void _Input(InputEvent @event)
{
    if (@event is InputEventKey keyEvent && keyEvent.Pressed)
    {
        var display = GetNode<TerminalDisplay>("%TerminalDisplay");
        
        if (keyEvent.Keycode == Key.Enter)
        {
            display.Terminal.WriteLine("");
        }
        else
        {
            string text = keyEvent.AsTextKeyLabel();
            display.Terminal.Write(text);
        }
        
        display.UpdateDisplay();
    }
}
```

## Acceptance Criteria Checklist

Use this checklist when manually testing Issue #2:

### Node Setup
- [x] Inherits from Control node
- [x] [Export] property for TerminalConfig
- [x] Reference to Terminal instance
- [x] Initialize display size from config

### Font Rendering
- [ ] Load font atlas from TerminalConfig (verify in scene)
- [ ] Render character grid using _Draw() (verify visual)
- [ ] Use DrawTextureRectRegion() for each character (check code)
- [ ] Apply integer scaling from config (test 1x, 2x, 3x)
- [ ] Use NEAREST texture filtering (verify pixel-perfect)

### Cursor Rendering
- [ ] Draw cursor block at current position (verify visual)
- [ ] Implement cursor blinking using timer (verify blink)
- [ ] Use CursorBlinkRate from config (modify and test)
- [ ] Toggle cursor visibility on/off (test CursorVisible property)

### Colors
- [ ] Apply foreground color to characters (verify gray text)
- [ ] Apply background color to terminal (verify black background)
- [ ] Support cursor color from config (verify white cursor)
- [ ] Modulate texture with colors (test different theme colors)

### Performance
- [ ] Only call QueueRedraw() when buffer changes (check FPS)
- [ ] Cache node references in _Ready() (code review)
- [ ] Avoid allocations in _Draw() (code review)

### Public API
- [x] UpdateDisplay() - Request redraw (test in script)
- [x] Property for Terminal instance (access in test)
- [x] Property for TerminalConfig (set in inspector)

## Troubleshooting

### Scene Won't Run
- Check Godot console for errors
- Verify all scripts compiled (Build → Build Solution)
- Ensure theme files exist in `assets/themes/`

### No Text Visible
- Check if background color == foreground color
- Verify font atlas loaded (not null)
- Check terminal buffer has content
- Verify UpdateDisplay() was called

### Characters Misaligned
- Check CharacterWidth/Height match font atlas
- Verify FontAtlasColumns/Rows = 16
- Check DisplayScale is integer (1, 2, 3, 4)

### Poor Performance
- Check terminal size (80×25 recommended, not 200×100)
- Verify not redrawing every frame
- Check for allocations in _Draw()

## Next Steps

After verifying TerminalDisplay works:
1. Close Issue #2 on GitHub
2. Merge feature branch to main
3. Begin Issue #3 (GodotTerminalEnvironment)
