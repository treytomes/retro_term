using Godot;

namespace RetroTerm.Terminal;

/// <summary>
/// Color palette for terminal display.
/// </summary>
/// <remarks>
/// Stores up to 256 colors for full ANSI color support.
/// Default palette provides standard 16 ANSI colors.
/// </remarks>
[GlobalClass]
public partial class TerminalPalette : Resource
{
	private Color[] _colors;

	/// <summary>
	/// Array of colors in this palette (up to 256).
	/// </summary>
	[Export]
	public Color[] Colors
	{
		get => _colors;
		set => _colors = value ?? CreateDefaultPalette();
	}

	/// <summary>
	/// Creates a new palette with default ANSI colors.
	/// </summary>
	public TerminalPalette()
	{
		_colors = CreateDefaultPalette();
	}

	/// <summary>
	/// Gets a color from the palette by index.
	/// </summary>
	/// <param name="index">Color index (0-255).</param>
	/// <returns>Color at the specified index, or white if out of range.</returns>
	public Color GetColor(byte index)
	{
		if (index < _colors.Length)
		{
			return _colors[index];
		}
		return new Color(1, 1, 1); // Fallback to white
	}

	/// <summary>
	/// Sets a color in the palette.
	/// </summary>
	/// <param name="index">Color index (0-255).</param>
	/// <param name="color">Color to set.</param>
	public void SetColor(byte index, Color color)
	{
		if (index < _colors.Length)
		{
			_colors[index] = color;
		}
	}

	/// <summary>
	/// Creates the standard 16-color ANSI palette.
	/// </summary>
	/// <remarks>
	/// Colors 0-7: Normal intensity
	/// Colors 8-15: Bright intensity
	/// Based on standard VGA text mode colors.
	/// </remarks>
	public static Color[] CreateDefaultPalette()
	{
		var palette = new Color[256];

		// Standard 16 ANSI colors (0-15)
		// Normal intensity (0-7)
		palette[0] = new Color(0.0f, 0.0f, 0.0f);       // Black
		palette[1] = new Color(0.667f, 0.0f, 0.0f);     // Red
		palette[2] = new Color(0.0f, 0.667f, 0.0f);     // Green
		palette[3] = new Color(0.667f, 0.667f, 0.0f);   // Yellow
		palette[4] = new Color(0.0f, 0.0f, 0.667f);     // Blue
		palette[5] = new Color(0.667f, 0.0f, 0.667f);   // Magenta
		palette[6] = new Color(0.0f, 0.667f, 0.667f);   // Cyan
		palette[7] = new Color(0.667f, 0.667f, 0.667f); // White (light gray)

		// Bright intensity (8-15)
		palette[8] = new Color(0.333f, 0.333f, 0.333f);   // Bright Black (dark gray)
		palette[9] = new Color(1.0f, 0.333f, 0.333f);     // Bright Red
		palette[10] = new Color(0.333f, 1.0f, 0.333f);    // Bright Green
		palette[11] = new Color(1.0f, 1.0f, 0.333f);      // Bright Yellow
		palette[12] = new Color(0.333f, 0.333f, 1.0f);    // Bright Blue
		palette[13] = new Color(1.0f, 0.333f, 1.0f);      // Bright Magenta
		palette[14] = new Color(0.333f, 1.0f, 1.0f);      // Bright Cyan
		palette[15] = new Color(1.0f, 1.0f, 1.0f);        // Bright White

		// Extended colors (16-255) - grayscale ramp for now
		// Can be customized for 256-color mode
		for (int i = 16; i < 256; i++)
		{
			float gray = (i - 16) / 239.0f;
			palette[i] = new Color(gray, gray, gray);
		}

		return palette;
	}

	/// <summary>
	/// Creates a Commodore 64 inspired palette.
	/// </summary>
	public static TerminalPalette CreateC64Palette()
	{
		var palette = new TerminalPalette();
		palette._colors[0] = new Color(0.0f, 0.0f, 0.0f);        // Black
		palette._colors[1] = new Color(1.0f, 1.0f, 1.0f);        // White
		palette._colors[2] = new Color(0.533f, 0.0f, 0.0f);      // Red
		palette._colors[3] = new Color(0.667f, 1.0f, 0.933f);    // Cyan
		palette._colors[4] = new Color(0.8f, 0.267f, 0.8f);      // Purple
		palette._colors[5] = new Color(0.0f, 0.8f, 0.333f);      // Green
		palette._colors[6] = new Color(0.0f, 0.0f, 0.667f);      // Blue
		palette._colors[7] = new Color(0.933f, 0.933f, 0.467f);  // Yellow
		palette._colors[8] = new Color(0.733f, 0.467f, 0.0f);    // Orange
		palette._colors[9] = new Color(0.467f, 0.267f, 0.0f);    // Brown
		palette._colors[10] = new Color(1.0f, 0.467f, 0.467f);   // Light Red
		palette._colors[11] = new Color(0.333f, 0.333f, 0.333f); // Dark Gray
		palette._colors[12] = new Color(0.467f, 0.467f, 0.467f); // Gray
		palette._colors[13] = new Color(0.667f, 1.0f, 0.4f);     // Light Green
		palette._colors[14] = new Color(0.4f, 0.4f, 1.0f);       // Light Blue
		palette._colors[15] = new Color(0.667f, 0.667f, 0.667f); // Light Gray
		return palette;
	}
}
