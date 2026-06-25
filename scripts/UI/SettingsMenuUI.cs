using Godot;
using RetroTerm.Terminal;
using System;

namespace RetroTerm.UI;

/// <summary>
/// Retro-style settings editor UI with text-based form.
/// </summary>
/// <remarks>
/// Fullscreen modal dialog for editing TerminalSettings.
/// Uses CP437 box-drawing characters and keyboard navigation.
/// </remarks>
[GlobalClass]
public partial class SettingsMenuUI : Control
{
	#region Fields

	private TerminalSettings? _settings;
	private TerminalSettings? _originalSettings;
	private int _selectedIndex = 0;
	private int _settingCount = 9; // Total number of settings
	private bool _isEditingNumeric = false;
	private string _numericInput = "";

	private Font? _font;
	private int _charWidth = 8;
	private int _charHeight = 12;

	#endregion

	#region Signals

	[Signal]
	public delegate void SettingsAppliedEventHandler(TerminalSettings settings);

	[Signal]
	public delegate void SettingsCancelledEventHandler();

	#endregion

	#region Properties

	[Export]
	public Color BackgroundColor { get; set; } = new Color(0.0f, 0.0f, 0.0f); // Black

	[Export]
	public Color BorderColor { get; set; } = new Color(0.667f, 0.667f, 0.667f); // Light gray

	[Export]
	public Color TextColor { get; set; } = new Color(0.667f, 0.667f, 0.667f); // Light gray

	[Export]
	public Color HighlightColor { get; set; } = new Color(1.0f, 1.0f, 0.0f); // Yellow

	[Export]
	public Color ValueColor { get; set; } = new Color(0.0f, 1.0f, 1.0f); // Cyan

	[Export]
	public Font? Font
	{
		get => _font;
		set
		{
			_font = value;
			QueueRedraw();
		}
	}

	#endregion

	#region Public Methods

	public void LoadSettings(TerminalSettings? settings)
	{
		if (settings == null)
		{
			return;
		}

		_settings = settings;
		// Create a copy for cancel functionality
		_originalSettings = new TerminalSettings
		{
			CommandHistorySize = settings.CommandHistorySize,
			TabWidth = settings.TabWidth,
			DefaultCursorStyle = settings.DefaultCursorStyle,
			ClearSelectionAfterCopy = settings.ClearSelectionAfterCopy,
			PasteStripNewlines = settings.PasteStripNewlines,
			PasteConvertTabs = settings.PasteConvertTabs,
			MaxPasteLength = settings.MaxPasteLength,
			AutoScrollOnOutput = settings.AutoScrollOnOutput,
			BellEnabled = settings.BellEnabled
		};

		_selectedIndex = 0;
		_isEditingNumeric = false;
		QueueRedraw();
	}

	#endregion

	#region Godot Lifecycle

	public override void _Ready()
	{
		// Start hidden
		Hide();

		// Make fullscreen
		SetAnchorsPreset(LayoutPreset.FullRect);
		Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		if (!Visible || _settings == null)
		{
			return;
		}

		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
		{
			if (_isEditingNumeric)
			{
				HandleNumericInput(keyEvent);
			}
			else
			{
				HandleNavigationInput(keyEvent);
			}

			AcceptEvent();
		}
	}

	public override void _Draw()
	{
		if (_font == null || _settings == null)
		{
			return;
		}

		// Draw semi-transparent background
		DrawRect(new Rect2(Vector2.Zero, Size), new Color(0, 0, 0, 0.85f), true);

		// Calculate box dimensions (centered)
		int boxWidth = 62; // Characters
		int boxHeight = 22; // Lines
		float pixelWidth = boxWidth * _charWidth;
		float pixelHeight = boxHeight * _charHeight;
		float startX = (Size.X - pixelWidth) / 2;
		float startY = (Size.Y - pixelHeight) / 2;

		// Draw background box
		DrawRect(new Rect2(startX, startY, pixelWidth, pixelHeight), BackgroundColor, true);

		// Draw border using CP437 box-drawing characters
		DrawBoxBorder(startX, startY, boxWidth, boxHeight);

		// Draw title
		string title = " TERMINAL SETTINGS ";
		float titleX = startX + (pixelWidth - _font.GetStringSize(title).X) / 2;
		DrawString(_font, new Vector2(titleX, startY + _charHeight), title, modulate: HighlightColor);

		// Draw settings
		float contentY = startY + _charHeight * 3;
		DrawSettings(startX, contentY);

		// Draw buttons
		DrawButtons(startX, startY + pixelHeight - _charHeight * 3);

		// Draw footer hints
		string footer = "F1=Help  F10=Exit";
		float footerX = startX + _charWidth * 2;
		DrawString(_font, new Vector2(footerX, startY + pixelHeight - _charHeight), footer, modulate: TextColor);
	}

	#endregion

	#region Private Methods - Input Handling

	private void HandleNavigationInput(InputEventKey keyEvent)
	{
		switch (keyEvent.Keycode)
		{
			case Key.Up:
				_selectedIndex = Math.Max(0, _selectedIndex - 1);
				QueueRedraw();
				break;

			case Key.Down:
				_selectedIndex = Math.Min(_settingCount + 2, _selectedIndex + 1); // +2 for buttons
				QueueRedraw();
				break;

			case Key.Left:
			case Key.Right:
				ToggleCurrentSetting(keyEvent.Keycode == Key.Right);
				break;

			case Key.Enter:
				HandleEnterKey();
				break;

			case Key.Escape:
			case Key.F10:
				Cancel();
				break;

			case Key.F1:
				// TODO: Show help for current setting
				break;
		}
	}

	private void HandleNumericInput(InputEventKey keyEvent)
	{
		if (keyEvent.Keycode == Key.Enter)
		{
			// Apply numeric input
			if (int.TryParse(_numericInput, out int value))
			{
				SetNumericValue(_selectedIndex, value);
			}
			_isEditingNumeric = false;
			_numericInput = "";
			QueueRedraw();
		}
		else if (keyEvent.Keycode == Key.Escape)
		{
			// Cancel numeric input
			_isEditingNumeric = false;
			_numericInput = "";
			QueueRedraw();
		}
		else if (keyEvent.Keycode == Key.Backspace)
		{
			if (_numericInput.Length > 0)
			{
				_numericInput = _numericInput[..^1];
				QueueRedraw();
			}
		}
		else if (keyEvent.Unicode != 0)
		{
			char c = (char)keyEvent.Unicode;
			if (char.IsDigit(c) && _numericInput.Length < 5)
			{
				_numericInput += c;
				QueueRedraw();
			}
		}
	}

	private void HandleEnterKey()
	{
		if (_settings == null)
		{
			return;
		}

		// Check if on a button
		if (_selectedIndex == _settingCount) // Apply button
		{
			Apply();
		}
		else if (_selectedIndex == _settingCount + 1) // Cancel button
		{
			Cancel();
		}
		else if (_selectedIndex == _settingCount + 2) // Reset button
		{
			ResetToDefaults();
		}
		// Check if on a numeric setting
		else if (IsNumericSetting(_selectedIndex))
		{
			_isEditingNumeric = true;
			_numericInput = GetNumericValue(_selectedIndex).ToString();
			QueueRedraw();
		}
	}

	private void ToggleCurrentSetting(bool increment)
	{
		if (_settings == null || _isEditingNumeric)
		{
			return;
		}

		switch (_selectedIndex)
		{
			case 2: // DefaultCursorStyle (enum)
				if (increment)
				{
					_settings.DefaultCursorStyle = _settings.DefaultCursorStyle == CursorStyle.Block
						? CursorStyle.Underline
						: (_settings.DefaultCursorStyle == CursorStyle.Underline
							? CursorStyle.VerticalBar
							: CursorStyle.Block);
				}
				else
				{
					_settings.DefaultCursorStyle = _settings.DefaultCursorStyle == CursorStyle.Block
						? CursorStyle.VerticalBar
						: (_settings.DefaultCursorStyle == CursorStyle.VerticalBar
							? CursorStyle.Underline
							: CursorStyle.Block);
				}
				break;

			case 3: // ClearSelectionAfterCopy (bool)
				_settings.ClearSelectionAfterCopy = !_settings.ClearSelectionAfterCopy;
				break;

			case 4: // PasteStripNewlines (bool)
				_settings.PasteStripNewlines = !_settings.PasteStripNewlines;
				break;

			case 5: // PasteConvertTabs (bool)
				_settings.PasteConvertTabs = !_settings.PasteConvertTabs;
				break;

			case 7: // AutoScrollOnOutput (bool)
				_settings.AutoScrollOnOutput = !_settings.AutoScrollOnOutput;
				break;

			case 8: // BellEnabled (bool)
				_settings.BellEnabled = !_settings.BellEnabled;
				break;
		}

		QueueRedraw();
	}

	private bool IsNumericSetting(int index)
	{
		return index == 0 || index == 1 || index == 6; // HistorySize, TabWidth, MaxPasteLength
	}

	private int GetNumericValue(int index)
	{
		if (_settings == null)
		{
			return 0;
		}

		return index switch
		{
			0 => _settings.CommandHistorySize,
			1 => _settings.TabWidth,
			6 => _settings.MaxPasteLength,
			_ => 0
		};
	}

	private void SetNumericValue(int index, int value)
	{
		if (_settings == null)
		{
			return;
		}

		switch (index)
		{
			case 0: // CommandHistorySize
				_settings.CommandHistorySize = Math.Clamp(value, 10, 1000);
				break;
			case 1: // TabWidth
				_settings.TabWidth = Math.Clamp(value, 2, 8);
				break;
			case 6: // MaxPasteLength
				_settings.MaxPasteLength = Math.Clamp(value, 0, 10000);
				break;
		}
	}

	#endregion

	#region Private Methods - Drawing

	private void DrawBoxBorder(float x, float y, int width, int height)
	{
		if (_font == null)
		{
			return;
		}

		// Top border: ╔═══...═══╗
		DrawString(_font, new Vector2(x, y + _charHeight), "╔", modulate: BorderColor);
		for (int i = 1; i < width - 1; i++)
		{
			DrawString(_font, new Vector2(x + i * _charWidth, y + _charHeight), "═", modulate: BorderColor);
		}
		DrawString(_font, new Vector2(x + (width - 1) * _charWidth, y + _charHeight), "╗", modulate: BorderColor);

		// Side borders: ║
		for (int i = 1; i < height - 1; i++)
		{
			DrawString(_font, new Vector2(x, y + i * _charHeight + _charHeight), "║", modulate: BorderColor);
			DrawString(_font, new Vector2(x + (width - 1) * _charWidth, y + i * _charHeight + _charHeight), "║", modulate: BorderColor);
		}

		// Bottom border: ╚═══...═══╝
		DrawString(_font, new Vector2(x, y + (height - 1) * _charHeight + _charHeight), "╚", modulate: BorderColor);
		for (int i = 1; i < width - 1; i++)
		{
			DrawString(_font, new Vector2(x + i * _charWidth, y + (height - 1) * _charHeight + _charHeight), "═", modulate: BorderColor);
		}
		DrawString(_font, new Vector2(x + (width - 1) * _charWidth, y + (height - 1) * _charHeight + _charHeight), "╝", modulate: BorderColor);
	}

	private void DrawSettings(float x, float y)
	{
		if (_font == null || _settings == null)
		{
			return;
		}

		float lineY = y;
		int index = 0;

		// INPUT SETTINGS
		DrawString(_font, new Vector2(x + _charWidth * 2, lineY), "INPUT SETTINGS", modulate: HighlightColor);
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Command History Size", _settings.CommandHistorySize.ToString(), "(10-1000)");
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Tab Width", _settings.TabWidth.ToString(), "(2-8 spaces)");
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Default Cursor Style", _settings.DefaultCursorStyle.ToString(), "");
		lineY += _charHeight * 2;

		// CLIPBOARD SETTINGS
		DrawString(_font, new Vector2(x + _charWidth * 2, lineY), "CLIPBOARD SETTINGS", modulate: HighlightColor);
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Clear Selection After Copy", _settings.ClearSelectionAfterCopy ? "Yes" : "No", "(Yes/No)");
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Paste Strip Newlines", _settings.PasteStripNewlines ? "Yes" : "No", "(Yes/No)");
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Paste Convert Tabs", _settings.PasteConvertTabs ? "Yes" : "No", "(Yes/No)");
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Max Paste Length", _settings.MaxPasteLength.ToString(), "(0-10000)");
		lineY += _charHeight * 2;

		// OUTPUT SETTINGS
		DrawString(_font, new Vector2(x + _charWidth * 2, lineY), "OUTPUT SETTINGS", modulate: HighlightColor);
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Auto-Scroll On Output", _settings.AutoScrollOnOutput ? "Yes" : "No", "(Yes/No)");
		lineY += _charHeight;

		DrawSetting(x, lineY, index++, "Bell Enabled", _settings.BellEnabled ? "Yes" : "No", "(Yes/No)");
	}

	private void DrawSetting(float x, float y, int index, string label, string value, string hint)
	{
		if (_font == null)
		{
			return;
		}

		// Draw selection indicator
		if (index == _selectedIndex)
		{
			DrawString(_font, new Vector2(x + _charWidth * 2, y), "►", modulate: HighlightColor);
		}

		// Draw label
		Color labelColor = index == _selectedIndex ? HighlightColor : TextColor;
		DrawString(_font, new Vector2(x + _charWidth * 4, y), label + ":", modulate: labelColor);

		// Draw value
		float valueX = x + _charWidth * 30;
		if (_isEditingNumeric && index == _selectedIndex)
		{
			// Show input box
			DrawString(_font, new Vector2(valueX, y), "[" + _numericInput + "_]", modulate: ValueColor);
		}
		else
		{
			DrawString(_font, new Vector2(valueX, y), "[" + value + "]", modulate: ValueColor);
		}

		// Draw hint
		DrawString(_font, new Vector2(valueX + _charWidth * 15, y), hint, modulate: TextColor);
	}

	private void DrawButtons(float x, float y)
	{
		if (_font == null)
		{
			return;
		}

		int buttonIndex = _settingCount;
		float buttonX = x + _charWidth * 4;

		// Apply button
		Color applyColor = _selectedIndex == buttonIndex ? HighlightColor : TextColor;
		DrawString(_font, new Vector2(buttonX, y), "[Apply]", modulate: applyColor);
		buttonX += _charWidth * 10;
		buttonIndex++;

		// Cancel button
		Color cancelColor = _selectedIndex == buttonIndex ? HighlightColor : TextColor;
		DrawString(_font, new Vector2(buttonX, y), "[Cancel]", modulate: cancelColor);
		buttonX += _charWidth * 12;
		buttonIndex++;

		// Reset button
		Color resetColor = _selectedIndex == buttonIndex ? HighlightColor : TextColor;
		DrawString(_font, new Vector2(buttonX, y), "[Reset to Defaults]", modulate: resetColor);
	}

	#endregion

	#region Private Methods - Actions

	private void Apply()
	{
		if (_settings != null)
		{
			EmitSignal("SettingsApplied", _settings);
		}
		Hide();
	}

	private void Cancel()
	{
		// Restore original settings
		if (_settings != null && _originalSettings != null)
		{
			_settings.CommandHistorySize = _originalSettings.CommandHistorySize;
			_settings.TabWidth = _originalSettings.TabWidth;
			_settings.DefaultCursorStyle = _originalSettings.DefaultCursorStyle;
			_settings.ClearSelectionAfterCopy = _originalSettings.ClearSelectionAfterCopy;
			_settings.PasteStripNewlines = _originalSettings.PasteStripNewlines;
			_settings.PasteConvertTabs = _originalSettings.PasteConvertTabs;
			_settings.MaxPasteLength = _originalSettings.MaxPasteLength;
			_settings.AutoScrollOnOutput = _originalSettings.AutoScrollOnOutput;
			_settings.BellEnabled = _originalSettings.BellEnabled;
		}

		EmitSignal("SettingsCancelled");
		Hide();
	}

	private void ResetToDefaults()
	{
		if (_settings != null)
		{
			var defaults = TerminalSettings.CreateDefault();
			_settings.CommandHistorySize = defaults.CommandHistorySize;
			_settings.TabWidth = defaults.TabWidth;
			_settings.DefaultCursorStyle = defaults.DefaultCursorStyle;
			_settings.ClearSelectionAfterCopy = defaults.ClearSelectionAfterCopy;
			_settings.PasteStripNewlines = defaults.PasteStripNewlines;
			_settings.PasteConvertTabs = defaults.PasteConvertTabs;
			_settings.MaxPasteLength = defaults.MaxPasteLength;
			_settings.AutoScrollOnOutput = defaults.AutoScrollOnOutput;
			_settings.BellEnabled = defaults.BellEnabled;
			QueueRedraw();
		}
	}

	#endregion
}
