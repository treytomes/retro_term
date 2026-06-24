using Godot;
using RetroTerm.Terminal;

namespace RetroTerm.Scenes;

/// <summary>
/// Test scene for demonstrating TerminalDisplay functionality.
/// </summary>
/// <remarks>
/// This scene writes sample text to the terminal for visual verification.
/// Used for manual testing of Issue #2 (TerminalDisplay).
/// </remarks>
public partial class TestTerminal : Node2D
{
	public override void _Ready()
	{
		var display = GetNode<TerminalDisplay>("%TerminalDisplay");

		if (display.Terminal == null)
		{
			GD.PrintErr("TestTerminal: Terminal is null");
			return;
		}

		var term = display.Terminal;

		// Title with colored background
		term.CurrentForegroundColor = 15; // Bright white
		term.CurrentBackgroundColor = 4;  // Blue
		term.WriteLine("  Terminal Color & Attribute Demo  ");
		term.WriteLine("");

		// Reset colors
		term.CurrentForegroundColor = 7;  // Light gray
		term.CurrentBackgroundColor = 0;  // Black

		// Show standard 16 ANSI colors
		term.WriteLine("Standard ANSI Colors (0-15):");
		term.WriteLine("");

		for (byte i = 0; i < 16; i++)
		{
			term.CurrentForegroundColor = i;
			term.Write($"{i,2} ");
		}
		term.WriteLine("");
		term.WriteLine("");

		// Reset
		term.CurrentForegroundColor = 7;

		// Show background colors
		term.WriteLine("Background Colors:");
		for (byte i = 0; i < 8; i++)
		{
			term.CurrentForegroundColor = 15; // Bright white text
			term.CurrentBackgroundColor = i;
			term.Write($" {i} ");
			term.CurrentBackgroundColor = 0;  // Reset
			term.Write(" ");
		}
		term.WriteLine("");
		term.WriteLine("");

		// Show text attributes
		term.WriteLine("Text Attributes:");
		term.WriteLine("");

		// Underline
		term.CurrentAttributes = TerminalAttributes.Underline;
		term.WriteLine("Underlined text");

		// Inverse
		term.CurrentAttributes = TerminalAttributes.Inverse;
		term.WriteLine("Inverse video text");

		// Dim
		term.CurrentAttributes = TerminalAttributes.Dim;
		term.WriteLine("Dimmed text (50% opacity)");

		// Strikethrough
		term.CurrentAttributes = TerminalAttributes.Strikethrough;
		term.WriteLine("Strikethrough text");

		// Combined attributes
		term.CurrentAttributes = TerminalAttributes.Underline;
		term.CurrentForegroundColor = 10; // Bright green
		term.WriteLine("Green + Underline");

		// Reset
		term.CurrentAttributes = TerminalAttributes.None;
		term.CurrentForegroundColor = 7;
		term.WriteLine("");

		// Colorful pattern
		term.WriteLine("Rainbow Pattern:");
		byte[] colors = { 1, 3, 2, 6, 4, 5, 9, 11, 10, 14, 12, 13 };
		foreach (byte color in colors)
		{
			term.CurrentForegroundColor = color;
			term.Write("█");
		}
		term.WriteLine("");

		// Request display update
		display.UpdateDisplay();

		GD.Print("TestTerminal: Color demo content written");
		GD.Print($"Terminal size: {term.Columns}x{term.Rows}");
	}
}
