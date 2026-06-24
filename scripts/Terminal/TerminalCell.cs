namespace RetroTerm.Terminal;

/// <summary>
/// Represents a single character cell in the terminal with attributes and colors.
/// </summary>
/// <remarks>
/// Each cell contains:
/// - Character to display
/// - Foreground color index (into palette)
/// - Background color index (into palette)
/// - Text attributes (bold, underline, etc.)
///
/// This structure enables rich terminal formatting similar to ANSI terminals.
/// </remarks>
public struct TerminalCell
{
	/// <summary>
	/// The character to display in this cell.
	/// </summary>
	public char Character { get; set; }

	/// <summary>
	/// Foreground color index into the terminal's color palette.
	/// </summary>
	/// <remarks>
	/// Default is 7 (usually light gray/white in standard palettes).
	/// Range: 0-255 for full ANSI color support.
	/// </remarks>
	public byte ForegroundColor { get; set; }

	/// <summary>
	/// Background color index into the terminal's color palette.
	/// </summary>
	/// <remarks>
	/// Default is 0 (usually black in standard palettes).
	/// Range: 0-255 for full ANSI color support.
	/// </remarks>
	public byte BackgroundColor { get; set; }

	/// <summary>
	/// Text attributes applied to this cell (bold, underline, etc.).
	/// </summary>
	public TerminalAttributes Attributes { get; set; }

	/// <summary>
	/// Creates a new terminal cell with the specified properties.
	/// </summary>
	/// <param name="character">Character to display.</param>
	/// <param name="foregroundColor">Foreground color index (default: 7 = light gray).</param>
	/// <param name="backgroundColor">Background color index (default: 0 = black).</param>
	/// <param name="attributes">Text attributes (default: None).</param>
	public TerminalCell(
		char character,
		byte foregroundColor = 7,
		byte backgroundColor = 0,
		TerminalAttributes attributes = TerminalAttributes.None)
	{
		Character = character;
		ForegroundColor = foregroundColor;
		BackgroundColor = backgroundColor;
		Attributes = attributes;
	}

	/// <summary>
	/// Creates a blank cell (space character with default colors).
	/// </summary>
	public static TerminalCell Blank => new(' ', 7, 0, TerminalAttributes.None);

	/// <summary>
	/// Checks if this cell has the specified attribute.
	/// </summary>
	/// <param name="attribute">Attribute to check.</param>
	/// <returns>True if the attribute is set.</returns>
	public readonly bool HasAttribute(TerminalAttributes attribute)
	{
		return (Attributes & attribute) == attribute;
	}

	/// <summary>
	/// Returns a string representation of this cell for debugging.
	/// </summary>
	public override readonly string ToString()
	{
		return $"'{Character}' FG:{ForegroundColor} BG:{BackgroundColor} Attr:{Attributes}";
	}
}
