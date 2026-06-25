using Godot;
using RetroTerm.Terminal;
using RetroTerm.UI;

namespace RetroTerm.Scenes;

/// <summary>
/// Test scene demonstrating toolbar and settings menu integration.
/// </summary>
public partial class TestToolbar : Control
{
	private RetroToolbar? _toolbar;
	private SettingsMenuUI? _settingsMenu;
	private TerminalDisplay? _display;

	public override void _Ready()
	{
		// Get node references
		_toolbar = GetNode<RetroToolbar>("%RetroToolbar");
		_settingsMenu = GetNode<SettingsMenuUI>("%SettingsMenu");
		_display = GetNode<TerminalDisplay>("%TerminalDisplay");

		if (_toolbar == null || _settingsMenu == null || _display == null)
		{
			GD.PrintErr("TestToolbar: Failed to get required nodes");
			return;
		}

		// Load font for toolbar and settings menu
		var font = GD.Load<Font>("res://assets/fonts/OEM437_12.png");
		if (font != null)
		{
			_toolbar.Font = font;
			_settingsMenu.Font = font;
		}

		// Connect signals
		_toolbar.MenuSelected += OnToolbarMenuSelected;
		_settingsMenu.SettingsApplied += OnSettingsApplied;
		_settingsMenu.SettingsCancelled += OnSettingsCancelled;

		// Write demo content to terminal
		if (_display.Terminal != null)
		{
			_display.Terminal.WriteLine("=== TOOLBAR AND SETTINGS DEMO ===");
			_display.Terminal.WriteLine("");
			_display.Terminal.WriteLine("Press F3 or Alt+S to open Settings menu");
			_display.Terminal.WriteLine("Press Alt+F for File menu (not yet implemented)");
			_display.Terminal.WriteLine("Press Alt+E for Edit menu (not yet implemented)");
			_display.Terminal.WriteLine("Press Alt+H or F1 for Help menu (not yet implemented)");
			_display.Terminal.WriteLine("");
			_display.Terminal.WriteLine("Try changing settings like:");
			_display.Terminal.WriteLine("- Command history size");
			_display.Terminal.WriteLine("- Tab width");
			_display.Terminal.WriteLine("- Cursor style");
			_display.Terminal.WriteLine("- Clipboard behavior");
			_display.Terminal.WriteLine("");
			_display.Terminal.Write("Ready> ");

			_display.UpdateDisplay();
		}

		GD.Print("TestToolbar: Initialized");
	}

	private void OnToolbarMenuSelected(string menuName)
	{
		GD.Print($"TestToolbar: Menu selected: {menuName}");

		switch (menuName)
		{
			case "File":
				ShowNotImplemented("File menu");
				break;

			case "Edit":
				ShowNotImplemented("Edit menu");
				break;

			case "Settings":
				OpenSettingsMenu();
				break;

			case "Help":
				ShowNotImplemented("Help menu");
				break;
		}
	}

	private void OpenSettingsMenu()
	{
		if (_settingsMenu == null || _display == null || _display.Settings == null)
		{
			return;
		}

		GD.Print("TestToolbar: Opening settings menu");

		// Load current settings into menu
		_settingsMenu.LoadSettings(_display.Settings);

		// Show menu (pauses terminal input)
		_settingsMenu.Show();
		_display.SetProcessInput(false);
	}

	private void OnSettingsApplied(TerminalSettings settings)
	{
		GD.Print("TestToolbar: Settings applied");

		if (_display == null)
		{
			return;
		}

		// Settings object was modified in-place, just resume terminal
		_display.SetProcessInput(true);

		// Write confirmation to terminal
		if (_display.Terminal != null)
		{
			_display.Terminal.WriteLine("");
			_display.Terminal.WriteLine("Settings saved!");
			_display.Terminal.Write("Ready> ");
			_display.UpdateDisplay();
		}
	}

	private void OnSettingsCancelled()
	{
		GD.Print("TestToolbar: Settings cancelled");

		if (_display == null)
		{
			return;
		}

		// Resume terminal input
		_display.SetProcessInput(true);

		// Write confirmation to terminal
		if (_display.Terminal != null)
		{
			_display.Terminal.WriteLine("");
			_display.Terminal.WriteLine("Settings cancelled.");
			_display.Terminal.Write("Ready> ");
			_display.UpdateDisplay();
		}
	}

	private void ShowNotImplemented(string feature)
	{
		if (_display == null || _display.Terminal == null)
		{
			return;
		}

		_display.Terminal.WriteLine("");
		_display.Terminal.WriteLine($"{feature} - Not yet implemented");
		_display.Terminal.Write("Ready> ");
		_display.UpdateDisplay();

		GD.Print($"TestToolbar: {feature} not yet implemented");
	}
}
