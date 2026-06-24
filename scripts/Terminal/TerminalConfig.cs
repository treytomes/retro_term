using Godot;

namespace RetroTerm.Terminal;

/// <summary>
/// Configuration resource for terminal display settings.
/// </summary>
/// <remarks>
/// This resource defines the visual and behavioral properties of a terminal emulator,
/// including dimensions, character rendering, and font selection. Can be saved as a
/// .tres file for theme presets (IBM PC, C64, VT100, etc.).
/// </remarks>
[GlobalClass]
public partial class TerminalConfig : Resource
{
	#region Properties

	/// <summary>
	/// Number of character columns in the terminal (horizontal).
	/// </summary>
	/// <remarks>
	/// Common values:
	/// - 80: Standard DOS/VT100 terminal
	/// - 40: Commodore 64, Apple II
	/// - 132: Wide terminal mode
	/// </remarks>
	[Export(PropertyHint.Range, "20,132,1")]
	public int Columns { get; set; } = 80;

	/// <summary>
	/// Number of character rows in the terminal (vertical).
	/// </summary>
	/// <remarks>
	/// Common values:
	/// - 25: Standard DOS terminal (80×25)
	/// - 24: VT100 terminal (80×24)
	/// - 43: EGA 43-line mode
	/// - 50: VGA 50-line mode
	/// </remarks>
	[Export(PropertyHint.Range, "20,60,1")]
	public int Rows { get; set; } = 25;

	/// <summary>
	/// Width of each character cell in pixels.
	/// </summary>
	/// <remarks>
	/// Should match the character width in the selected font atlas.
	/// Common values: 8 (CGA/EGA), 9 (VGA text mode).
	/// </remarks>
	[Export(PropertyHint.Range, "4,16,1")]
	public int CharacterWidth { get; set; } = 8;

	/// <summary>
	/// Height of each character cell in pixels.
	/// </summary>
	/// <remarks>
	/// Should match the character height in the selected font atlas.
	/// Common values: 8 (CGA), 12, 14 (EGA), 16 (VGA).
	/// </remarks>
	[Export(PropertyHint.Range, "8,16,1")]
	public int CharacterHeight { get; set; } = 8;

	/// <summary>
	/// Integer scale multiplier for the display.
	/// </summary>
	/// <remarks>
	/// Use integer scaling to maintain pixel-perfect rendering.
	/// 1 = native resolution, 2 = 2x size, 3 = 3x size, etc.
	///
	/// Example: 80×25 terminal with 8×8 characters at 2× scale
	/// = 1280×400 pixels display area.
	/// </remarks>
	[Export(PropertyHint.Range, "1,4,1")]
	public int DisplayScale { get; set; } = 2;

	/// <summary>
	/// Font atlas texture containing the character set.
	/// </summary>
	/// <remarks>
	/// Expected format: 16×16 grid of characters (256 total).
	/// Character size should match CharacterWidth × CharacterHeight.
	///
	/// Available fonts:
	/// - OEM437_8.png: 8×8 pixels per character (128×128 total)
	/// - OEM437_12.png: 8×12 pixels per character (128×192 total)
	/// - OEM437_16.png: 8×16 pixels per character (128×256 total)
	/// </remarks>
	[Export]
	public Texture2D? FontAtlas { get; set; }

	/// <summary>
	/// Number of columns in the font atlas grid.
	/// </summary>
	/// <remarks>
	/// Standard CP437 fonts use 16 columns (16×16 = 256 characters).
	/// </remarks>
	[Export(PropertyHint.Range, "8,32,1")]
	public int FontAtlasColumns { get; set; } = 16;

	/// <summary>
	/// Number of rows in the font atlas grid.
	/// </summary>
	/// <remarks>
	/// Standard CP437 fonts use 16 rows (16×16 = 256 characters).
	/// </remarks>
	[Export(PropertyHint.Range, "8,32,1")]
	public int FontAtlasRows { get; set; } = 16;

	/// <summary>
	/// Foreground (text) color.
	/// </summary>
	[Export]
	public Color ForegroundColor { get; set; } = Colors.LightGray;

	/// <summary>
	/// Background color.
	/// </summary>
	[Export]
	public Color BackgroundColor { get; set; } = Colors.Black;

	/// <summary>
	/// Cursor color.
	/// </summary>
	[Export]
	public Color CursorColor { get; set; } = Colors.White;

	/// <summary>
	/// Cursor blink rate in seconds (0 = no blinking).
	/// </summary>
	[Export(PropertyHint.Range, "0.0,2.0,0.1")]
	public float CursorBlinkRate { get; set; } = 0.5f;

	/// <summary>
	/// Enable CRT scanline effect.
	/// </summary>
	[Export]
	public bool EnableScanlines { get; set; } = true;

	/// <summary>
	/// Scanline intensity (0.0 = invisible, 1.0 = fully opaque).
	/// </summary>
	[Export(PropertyHint.Range, "0.0,1.0,0.01")]
	public float ScanlineIntensity { get; set; } = 0.15f;

	/// <summary>
	/// Enable CRT screen curvature effect.
	/// </summary>
	[Export]
	public bool EnableCurvature { get; set; } = false;

	/// <summary>
	/// Screen curvature amount (0.0 = flat, 1.0 = maximum curve).
	/// </summary>
	[Export(PropertyHint.Range, "0.0,1.0,0.01")]
	public float CurvatureAmount { get; set; } = 0.15f;

	/// <summary>
	/// Calculates the total display width in pixels.
	/// </summary>
	public int DisplayWidth => Columns * CharacterWidth * DisplayScale;

	/// <summary>
	/// Calculates the total display height in pixels.
	/// </summary>
	public int DisplayHeight => Rows * CharacterHeight * DisplayScale;

	/// <summary>
	/// Calculates the font atlas total width in pixels.
	/// </summary>
	public int FontAtlasWidth => FontAtlasColumns * CharacterWidth;

	/// <summary>
	/// Calculates the font atlas total height in pixels.
	/// </summary>
	public int FontAtlasHeight => FontAtlasRows * CharacterHeight;

	#endregion

	#region Methods

	/// <summary>
	/// Gets the source rectangle for a character in the font atlas.
	/// </summary>
	/// <param name="charCode">Character code (0-255 for CP437).</param>
	/// <returns>Rectangle defining the character's position in the atlas.</returns>
	public Rect2 GetCharacterRect(int charCode)
	{
		int col = charCode % FontAtlasColumns;
		int row = charCode / FontAtlasColumns;
		return new Rect2(col * CharacterWidth, row * CharacterHeight, CharacterWidth, CharacterHeight);
	}

	/// <summary>
	/// Loads a preset theme from resources.
	/// </summary>
	/// <param name="themeName">Theme name: "ibm_pc", "commodore64", "vt100", or "vga".</param>
	/// <returns>Loaded TerminalConfig resource, or null if not found.</returns>
	public static TerminalConfig? LoadTheme(string themeName)
	{
		string path = $"res://assets/themes/{themeName}.tres";
		return GD.Load<TerminalConfig>(path);
	}

	/// <summary>
	/// Loads the default IBM PC configuration (80×25, 8×12 font).
	/// </summary>
	/// <remarks>
	/// Loads from res://assets/themes/ibm_pc.tres
	/// </remarks>
	public static TerminalConfig? LoadIBMPC() => LoadTheme("ibm_pc");

	/// <summary>
	/// Loads the Commodore 64 style configuration (40×25, 8×8 font, blue background).
	/// </summary>
	/// <remarks>
	/// Loads from res://assets/themes/commodore64.tres
	/// </remarks>
	public static TerminalConfig? LoadCommodore64() => LoadTheme("commodore64");

	/// <summary>
	/// Loads the VT100 style configuration (80×24, 8×12 font, green text on black).
	/// </summary>
	/// <remarks>
	/// Loads from res://assets/themes/vt100.tres
	/// </remarks>
	public static TerminalConfig? LoadVT100() => LoadTheme("vt100");

	/// <summary>
	/// Loads the VGA text mode configuration (80×25, 8×16 font).
	/// </summary>
	/// <remarks>
	/// Loads from res://assets/themes/vga.tres
	/// </remarks>
	public static TerminalConfig? LoadVGA() => LoadTheme("vga");

	#endregion
}
