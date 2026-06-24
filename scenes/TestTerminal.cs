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

		// Write test content to terminal
		display.Terminal.WriteLine("=== RETRO TERMINAL TEST ===");
		display.Terminal.WriteLine("");
		display.Terminal.WriteLine("Terminal initialized successfully!");
		display.Terminal.WriteLine("");
		display.Terminal.WriteLine("Testing character rendering:");
		display.Terminal.WriteLine("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
		display.Terminal.WriteLine("abcdefghijklmnopqrstuvwxyz");
		display.Terminal.WriteLine("0123456789 !@#$%^&*()");
		display.Terminal.WriteLine("");
		display.Terminal.WriteLine("Testing wrapping and scrolling...");
		display.Terminal.WriteLine("The quick brown fox jumps over the lazy dog.");
		display.Terminal.WriteLine("");
		display.Terminal.Write("Current position: ");
		display.Terminal.Write($"({display.Terminal.CursorX}, {display.Terminal.CursorY})");

		// Request display update
		display.UpdateDisplay();

		GD.Print("TestTerminal: Test content written");
		GD.Print($"Terminal size: {display.Terminal.Columns}x{display.Terminal.Rows}");
	}
}
