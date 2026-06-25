using Godot;
using System.Text;

namespace RetroTerm.Terminal;

/// <summary>
/// Helper script to generate palette resource files.
/// Run this once to create .tres files from palette factory methods.
/// </summary>
[Tool]
[GlobalClass]
public partial class GeneratePaletteResources : Node
{
	public override void _Ready()
	{
		// Only run in editor
		if (!Engine.IsEditorHint())
		{
			return;
		}

		GD.Print("Generating palette resource files...");

		// Generate default ANSI palette
		GenerateDefaultPalette();

		// Generate C64 palette
		GenerateC64Palette();

		GD.Print("Palette generation complete!");
	}

	private void GenerateDefaultPalette()
	{
		var palette = new TerminalPalette();
		palette.Colors = TerminalPalette.CreateDefaultPalette();

		string path = "res://assets/palettes/ansi_default.tres";
		ResourceSaver.Save(palette, path);
		GD.Print($"Saved: {path}");
	}

	private void GenerateC64Palette()
	{
		var palette = TerminalPalette.CreateC64Palette();

		string path = "res://assets/palettes/commodore64.tres";
		ResourceSaver.Save(palette, path);
		GD.Print($"Saved: {path}");
	}
}
