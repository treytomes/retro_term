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
	private Texture2D? _fontAtlas;
	private int _charWidth = 8;
	private int _charHeight = 12;
	private int _atlasColumns = 16;

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
	/// Font atlas texture for rendering text.
	/// </summary>
	[Export]
	public Texture2D? FontAtlas
	{
		get => _fontAtlas;
		set
		{
			_fontAtlas = value;
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
		if (_fontAtlas == null)
		{
			return;
		}

		// Draw background bar
		DrawRect(new Rect2(0, 0, Size.X, _charHeight), BackgroundColor, true);

		// Draw separator lines (CP437 box characters)
		// ║ (186) at start and end
		DrawText(0, 0, "║", NormalColor);

		// Draw menu items
		float x = _charWidth * 2; // Start after left border
		for (int i = 0; i < _menuItems.Length; i++)
		{
			Color color = i == _selectedIndex ? HighlightColor : NormalColor;

			// Add highlight background for selected item
			if (i == _selectedIndex)
			{
				float itemWidth = _menuItems[i].Length * _charWidth;
				DrawRect(new Rect2(x - 2, 0, itemWidth + 4, _charHeight),
					new Color(0.667f, 0.667f, 0.0f), true); // Yellow background
			}

			DrawText(x, 0, _menuItems[i], color);

			x += _menuItems[i].Length * _charWidth + _charWidth;

			// Draw separator between items
			if (i < _menuItems.Length - 1)
			{
				DrawText(x, 0, "│", NormalColor);
				x += _charWidth * 2;
			}
		}

		// Draw right border and hint
		string hint = "F10=Menu";
		float hintX = Size.X - (hint.Length * _charWidth) - _charWidth * 2;
		DrawText(hintX, 0, hint, NormalColor);
		DrawText(Size.X - _charWidth, 0, "║", NormalColor);
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Draws text at the specified position using the font atlas.
	/// </summary>
	/// <param name="x">X position in pixels.</param>
	/// <param name="y">Y position in pixels.</param>
	/// <param name="text">Text to draw.</param>
	/// <param name="color">Color to modulate the text.</param>
	private void DrawText(float x, float y, string text, Color color)
	{
		if (_fontAtlas == null || string.IsNullOrEmpty(text))
		{
			return;
		}

		float currentX = x;
		foreach (char c in text)
		{
			// Get character rectangle from atlas
			Rect2 sourceRect = GetCharacterRect(c);

			// Calculate destination
			Rect2 destRect = new Rect2(currentX, y, _charWidth, _charHeight);

			// Draw character
			DrawTextureRectRegion(_fontAtlas, destRect, sourceRect, color);

			currentX += _charWidth;
		}
	}

	/// <summary>
	/// Gets the source rectangle for a character in the font atlas.
	/// </summary>
	/// <param name="c">Character to get rectangle for.</param>
	/// <returns>Rectangle in the font atlas texture.</returns>
	private Rect2 GetCharacterRect(char c)
	{
		int charCode = c;
		int col = charCode % _atlasColumns;
		int row = charCode / _atlasColumns;

		return new Rect2(
			col * _charWidth,
			row * _charHeight,
			_charWidth,
			_charHeight
		);
	}

	/// <summary>
	/// Gets the menu item index at the specified mouse position.
	/// </summary>
	/// <param name="position">Mouse position.</param>
	/// <returns>Menu item index, or -1 if no item at position.</returns>
	private int GetMenuItemAtPosition(Vector2 position)
	{
		if (_fontAtlas == null || position.Y < 0 || position.Y > _charHeight)
		{
			return -1;
		}

		float x = _charWidth * 2; // Start after left border
		for (int i = 0; i < _menuItems.Length; i++)
		{
			float itemWidth = _menuItems[i].Length * _charWidth;

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
