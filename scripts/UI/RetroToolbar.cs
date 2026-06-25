using Godot;

namespace RetroTerm.UI;

/// <summary>
/// Retro-style text-based toolbar for terminal menu navigation.
/// </summary>
/// <remarks>
/// DOS/Norton Commander inspired menu bar with keyboard and mouse navigation.
/// Displays menu items as text with highlighting for the current selection.
/// </remarks>
[GlobalClass]
public partial class RetroToolbar : Control
{
	#region Fields

	private int _selectedIndex = -1; // -1 = no selection
	private readonly string[] _menuItems = { "File", "Edit", "Settings", "Help" };
	private Font? _font;
	private int _charWidth = 8;
	private int _charHeight = 12;

	#endregion

	#region Signals

	/// <summary>
	/// Emitted when a menu item is selected via keyboard or mouse.
	/// </summary>
	[Signal]
	public delegate void MenuSelectedEventHandler(string menuName);

	#endregion

	#region Properties

	/// <summary>
	/// Background color for the toolbar.
	/// </summary>
	[Export]
	public Color BackgroundColor { get; set; } = new Color(0.0f, 0.0f, 0.667f); // Blue

	/// <summary>
	/// Text color for normal menu items.
	/// </summary>
	[Export]
	public Color NormalColor { get; set; } = new Color(0.667f, 0.667f, 0.667f); // Light gray

	/// <summary>
	/// Text color for highlighted menu item.
	/// </summary>
	[Export]
	public Color HighlightColor { get; set; } = new Color(1.0f, 1.0f, 0.0f); // Yellow

	/// <summary>
	/// Font to use for menu text.
	/// </summary>
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

	#region Godot Lifecycle

	public override void _Ready()
	{
		// Set default size
		CustomMinimumSize = new Vector2(0, _charHeight);
		Size = new Vector2(GetViewportRect().Size.X, _charHeight);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
		{
			// Alt+Letter shortcuts
			if (keyEvent.AltPressed)
			{
				switch (keyEvent.Keycode)
				{
					case Key.F:
						EmitSignal("MenuSelected", "File");
						AcceptEvent();
						break;
					case Key.E:
						EmitSignal("MenuSelected", "Edit");
						AcceptEvent();
						break;
					case Key.S:
						EmitSignal("MenuSelected", "Settings");
						AcceptEvent();
						break;
					case Key.H:
						EmitSignal("MenuSelected", "Help");
						AcceptEvent();
						break;
				}
			}
			// Function key shortcuts
			else if (keyEvent.Keycode == Key.F3)
			{
				EmitSignal("MenuSelected", "Settings");
				AcceptEvent();
			}
			else if (keyEvent.Keycode == Key.F1)
			{
				EmitSignal("MenuSelected", "Help");
				AcceptEvent();
			}
		}
		else if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left)
			{
				// Check which menu item was clicked
				int clickedIndex = GetMenuItemAtPosition(mouseButton.Position);
				if (clickedIndex >= 0 && clickedIndex < _menuItems.Length)
				{
					EmitSignal("MenuSelected", _menuItems[clickedIndex]);
					AcceptEvent();
				}
			}
		}
		else if (@event is InputEventMouseMotion mouseMotion)
		{
			// Update highlight on hover
			int hoveredIndex = GetMenuItemAtPosition(mouseMotion.Position);
			if (hoveredIndex != _selectedIndex)
			{
				_selectedIndex = hoveredIndex;
				QueueRedraw();
			}
		}
	}

	public override void _Draw()
	{
		if (_font == null)
		{
			return;
		}

		// Draw background bar
		DrawRect(new Rect2(0, 0, Size.X, _charHeight), BackgroundColor, true);

		// Draw separator lines (CP437 box characters)
		// ║ (186) at start and end
		DrawString(_font, new Vector2(0, _charHeight - 2), "║", modulate: NormalColor);

		// Draw menu items
		float x = _charWidth * 2; // Start after left border
		for (int i = 0; i < _menuItems.Length; i++)
		{
			Color color = i == _selectedIndex ? HighlightColor : NormalColor;

			// Add highlight background for selected item
			if (i == _selectedIndex)
			{
				float itemWidth = _font.GetStringSize(_menuItems[i]).X;
				DrawRect(new Rect2(x - 2, 0, itemWidth + 4, _charHeight),
					new Color(0.667f, 0.667f, 0.0f), true); // Yellow background
			}

			DrawString(_font, new Vector2(x, _charHeight - 2), _menuItems[i], modulate: color);

			x += _font.GetStringSize(_menuItems[i]).X + _charWidth;

			// Draw separator between items
			if (i < _menuItems.Length - 1)
			{
				DrawString(_font, new Vector2(x, _charHeight - 2), "│", modulate: NormalColor);
				x += _charWidth * 2;
			}
		}

		// Draw right border and hint
		string hint = "F10=Menu";
		float hintX = Size.X - _font.GetStringSize(hint).X - _charWidth * 2;
		DrawString(_font, new Vector2(hintX, _charHeight - 2), hint, modulate: NormalColor);
		DrawString(_font, new Vector2(Size.X - _charWidth, _charHeight - 2), "║", modulate: NormalColor);
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Gets the menu item index at the specified mouse position.
	/// </summary>
	/// <param name="position">Mouse position.</param>
	/// <returns>Menu item index, or -1 if no item at position.</returns>
	private int GetMenuItemAtPosition(Vector2 position)
	{
		if (_font == null || position.Y < 0 || position.Y > _charHeight)
		{
			return -1;
		}

		float x = _charWidth * 2; // Start after left border
		for (int i = 0; i < _menuItems.Length; i++)
		{
			float itemWidth = _font.GetStringSize(_menuItems[i]).X;

			if (position.X >= x && position.X < x + itemWidth)
			{
				return i;
			}

			x += itemWidth + _charWidth * 3; // Item width + separator
		}

		return -1;
	}

	#endregion
}
