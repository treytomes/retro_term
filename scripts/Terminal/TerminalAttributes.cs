namespace RetroTerm.Terminal;

/// <summary>
/// Text attributes that can be applied to terminal characters.
/// </summary>
/// <remarks>
/// These flags can be combined using bitwise OR to apply multiple attributes.
/// Matches common terminal escape sequence capabilities.
/// </remarks>
[Flags]
public enum TerminalAttributes : byte
{
	/// <summary>
	/// No attributes applied.
	/// </summary>
	None = 0,

	/// <summary>
	/// Bold or bright text (increased intensity).
	/// </summary>
	Bold = 1 << 0,

	/// <summary>
	/// Dim or faint text (decreased intensity).
	/// </summary>
	Dim = 1 << 1,

	/// <summary>
	/// Italic text.
	/// </summary>
	Italic = 1 << 2,

	/// <summary>
	/// Underlined text.
	/// </summary>
	Underline = 1 << 3,

	/// <summary>
	/// Blinking text (slow blink).
	/// </summary>
	Blink = 1 << 4,

	/// <summary>
	/// Inverse or reverse video (swap foreground and background colors).
	/// </summary>
	Inverse = 1 << 5,

	/// <summary>
	/// Hidden or invisible text.
	/// </summary>
	Hidden = 1 << 6,

	/// <summary>
	/// Strikethrough text.
	/// </summary>
	Strikethrough = 1 << 7
}
