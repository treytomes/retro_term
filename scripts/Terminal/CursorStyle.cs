namespace RetroTerm.Terminal;

/// <summary>
/// Terminal cursor display styles.
/// </summary>
/// <remarks>
/// Defines the visual appearance of the terminal cursor.
/// Block (CP437 character 219) is typically used for insert mode,
/// while underline is used for overwrite mode.
/// </remarks>
public enum CursorStyle
{
	/// <summary>
	/// Full block cursor (CP437 character 219 'Û').
	/// </summary>
	/// <remarks>
	/// Traditional insert mode cursor. Fills the entire character cell.
	/// </remarks>
	Block,

	/// <summary>
	/// Underline cursor (line at bottom of cell).
	/// </summary>
	/// <remarks>
	/// Traditional overwrite mode cursor. Shows a line at the bottom
	/// of the character cell.
	/// </remarks>
	Underline,

	/// <summary>
	/// Vertical bar cursor (line at left edge of cell).
	/// </summary>
	/// <remarks>
	/// Modern editor-style cursor. Shows a thin vertical line
	/// at the left edge of the character cell.
	/// </remarks>
	VerticalBar
}
