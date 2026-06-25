using Godot;

namespace RetroTerm.Terminal;

/// <summary>
/// User-configurable terminal behavior settings.
/// </summary>
/// <remarks>
/// This resource defines runtime preferences and behavior settings for terminal input/output,
/// separate from display configuration (TerminalConfig). Can be saved as a .tres file for
/// user preference presets.
///
/// Settings include:
/// - Input behavior (history size, tab width, cursor mode)
/// - Editing behavior (selection, copy/paste)
/// - Output behavior (auto-scroll, bell)
/// </remarks>
[GlobalClass]
public partial class TerminalSettings : Resource
{
	#region Input Settings

	/// <summary>
	/// Maximum number of commands to store in history.
	/// </summary>
	/// <remarks>
	/// Commands are stored in memory for recall with Up/Down arrow keys.
	/// Oldest commands are removed when limit is reached.
	/// Default: 100 commands.
	/// </remarks>
	[Export(PropertyHint.Range, "10,1000,1")]
	public int CommandHistorySize { get; set; } = 100;

	/// <summary>
	/// Number of spaces to insert when Tab key is pressed.
	/// </summary>
	/// <remarks>
	/// Common values:
	/// - 4: Modern editors, Python
	/// - 8: Traditional terminals, C
	/// Default: 4 spaces.
	/// </remarks>
	[Export(PropertyHint.Range, "2,8,1")]
	public int TabWidth { get; set; } = 4;

	/// <summary>
	/// Default cursor style on startup.
	/// </summary>
	/// <remarks>
	/// Block cursor typically indicates Insert mode.
	/// Underline cursor typically indicates Overwrite mode.
	/// User can toggle with Insert key at runtime.
	/// Default: Block (Insert mode).
	/// </remarks>
	[Export]
	public CursorStyle DefaultCursorStyle { get; set; } = CursorStyle.Block;

	#endregion

	#region Selection and Clipboard Settings

	/// <summary>
	/// Whether to clear text selection after copying to clipboard.
	/// </summary>
	/// <remarks>
	/// True: Traditional behavior, selection clears after Ctrl+C
	/// False: Modern behavior, selection remains after copy
	/// Default: false (keep selection).
	/// </remarks>
	[Export]
	public bool ClearSelectionAfterCopy { get; set; } = false;

	/// <summary>
	/// Whether to strip newlines when pasting multi-line text.
	/// </summary>
	/// <remarks>
	/// True: Convert newlines to spaces (single-line input mode)
	/// False: Preserve newlines (multi-line input mode - Phase 2)
	/// Default: true (single-line mode).
	/// </remarks>
	[Export]
	public bool PasteStripNewlines { get; set; } = true;

	/// <summary>
	/// Whether to convert tabs to spaces when pasting.
	/// </summary>
	/// <remarks>
	/// True: Replace \t with spaces (using TabWidth setting)
	/// False: Preserve tab characters
	/// Default: true (convert to spaces).
	/// </remarks>
	[Export]
	public bool PasteConvertTabs { get; set; } = true;

	/// <summary>
	/// Maximum length of pasted text in characters.
	/// </summary>
	/// <remarks>
	/// Prevents accidental paste of very large clipboard content.
	/// 0 = no limit (not recommended)
	/// Default: 1024 characters.
	/// </remarks>
	[Export(PropertyHint.Range, "0,10000,1")]
	public int MaxPasteLength { get; set; } = 1024;

	#endregion

	#region Output Settings

	/// <summary>
	/// Whether terminal auto-scrolls when new output is written.
	/// </summary>
	/// <remarks>
	/// True: Automatically scroll to bottom when output arrives
	/// False: Stay at current scroll position (manual scrollback)
	/// Default: true (auto-scroll).
	/// </remarks>
	[Export]
	public bool AutoScrollOnOutput { get; set; } = true;

	/// <summary>
	/// Whether to play a bell sound on beep character (ASCII 7).
	/// </summary>
	/// <remarks>
	/// True: Play audio feedback for bell character
	/// False: Ignore bell character (silent)
	/// Default: false (silent).
	/// </remarks>
	[Export]
	public bool BellEnabled { get; set; } = false;

	#endregion

	#region Factory Methods

	/// <summary>
	/// Creates default terminal settings.
	/// </summary>
	/// <returns>Settings with default values.</returns>
	public static TerminalSettings CreateDefault()
	{
		return new TerminalSettings
		{
			CommandHistorySize = 100,
			TabWidth = 4,
			DefaultCursorStyle = CursorStyle.Block,
			ClearSelectionAfterCopy = false,
			PasteStripNewlines = true,
			PasteConvertTabs = true,
			MaxPasteLength = 1024,
			AutoScrollOnOutput = true,
			BellEnabled = false
		};
	}

	/// <summary>
	/// Creates settings optimized for programming/scripting.
	/// </summary>
	/// <returns>Settings for coding use case.</returns>
	public static TerminalSettings CreateProgramming()
	{
		return new TerminalSettings
		{
			CommandHistorySize = 500,  // More history for coding
			TabWidth = 4,              // Standard for modern code
			DefaultCursorStyle = CursorStyle.Block,
			ClearSelectionAfterCopy = false,
			PasteStripNewlines = false, // Allow multi-line paste (Phase 2)
			PasteConvertTabs = false,   // Preserve tabs in code
			MaxPasteLength = 10000,     // Allow larger code pastes
			AutoScrollOnOutput = true,
			BellEnabled = false
		};
	}

	/// <summary>
	/// Creates settings optimized for classic terminal experience.
	/// </summary>
	/// <returns>Settings for retro/traditional terminal behavior.</returns>
	public static TerminalSettings CreateClassic()
	{
		return new TerminalSettings
		{
			CommandHistorySize = 50,    // Smaller history like old systems
			TabWidth = 8,               // Traditional terminal tab width
			DefaultCursorStyle = CursorStyle.Block,
			ClearSelectionAfterCopy = true,  // Traditional behavior
			PasteStripNewlines = true,
			PasteConvertTabs = true,
			MaxPasteLength = 512,       // Conservative limit
			AutoScrollOnOutput = true,
			BellEnabled = true          // Classic terminal bell
		};
	}

	#endregion
}
