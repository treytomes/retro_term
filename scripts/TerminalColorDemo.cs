using Godot;

namespace RetroTerm.Terminal;

/// <summary>
/// Demonstrates terminal color and attribute features.
/// </summary>
[GlobalClass]
public partial class TerminalColorDemo : Node
{
	private TerminalDisplay? _display;

	public override void _Ready()
	{
		// Get the TerminalDisplay node
		_display = GetNode<TerminalDisplay>("%TerminalDisplay");

		if (_display?.Terminal == null)
		{
			GD.PrintErr("TerminalColorDemo: TerminalDisplay or Terminal is null");
			return;
		}

		Terminal terminal = _display.Terminal;

		// Title
		terminal.CurrentForegroundColor = 15; // Bright white
		terminal.CurrentBackgroundColor = 4;  // Blue
		terminal.WriteLine("  Terminal Color & Attribute Demo  ");
		terminal.WriteLine("");

		// Reset colors
		terminal.CurrentForegroundColor = 7;  // Light gray
		terminal.CurrentBackgroundColor = 0;  // Black

		// Show standard 16 ANSI colors
		terminal.WriteLine("Standard ANSI Colors (0-15):");
		terminal.WriteLine("");

		for (byte i = 0; i < 16; i++)
		{
			terminal.CurrentForegroundColor = i;
			terminal.Write($"{i,2} ");
		}
		terminal.WriteLine("");
		terminal.WriteLine("");

		// Reset
		terminal.CurrentForegroundColor = 7;

		// Show background colors
		terminal.WriteLine("Background Colors:");
		for (byte i = 0; i < 8; i++)
		{
			terminal.CurrentForegroundColor = 15; // Bright white text
			terminal.CurrentBackgroundColor = i;
			terminal.Write($" {i} ");
			terminal.CurrentBackgroundColor = 0;  // Reset
			terminal.Write(" ");
		}
		terminal.WriteLine("");
		terminal.WriteLine("");

		// Show text attributes
		terminal.WriteLine("Text Attributes:");
		terminal.WriteLine("");

		// Bold
		terminal.CurrentAttributes = TerminalAttributes.Bold;
		terminal.WriteLine("Bold text (rendered as bright color)");

		// Underline
		terminal.CurrentAttributes = TerminalAttributes.Underline;
		terminal.WriteLine("Underlined text");

		// Inverse
		terminal.CurrentAttributes = TerminalAttributes.Inverse;
		terminal.WriteLine("Inverse video text");

		// Dim
		terminal.CurrentAttributes = TerminalAttributes.Dim;
		terminal.WriteLine("Dimmed text (50% opacity)");

		// Strikethrough
		terminal.CurrentAttributes = TerminalAttributes.Strikethrough;
		terminal.WriteLine("Strikethrough text");

		// Combined attributes
		terminal.CurrentAttributes = TerminalAttributes.Bold | TerminalAttributes.Underline;
		terminal.CurrentForegroundColor = 10; // Bright green
		terminal.WriteLine("Bold + Underline + Green");

		// Reset
		terminal.CurrentAttributes = TerminalAttributes.None;
		terminal.CurrentForegroundColor = 7;
		terminal.WriteLine("");

		// Colorful pattern using CP437 block character (0xDB = 219)
		terminal.WriteLine("Rainbow Pattern:");
		byte[] colors = { 1, 3, 2, 6, 4, 5, 9, 11, 10, 14, 12, 13 };
		foreach (byte color in colors)
		{
			terminal.CurrentForegroundColor = color;
			terminal.Write("Û"); // CP437 full block character
		}
		terminal.WriteLine("");

		// Request display update
		_display.UpdateDisplay();
	}
}
