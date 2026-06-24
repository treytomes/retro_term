using Godot;

namespace RetroTerm.Terminal;

/// <summary>
/// Godot Control node that renders a Terminal to the screen.
/// </summary>
/// <remarks>
/// This node handles all rendering of the terminal character buffer using a font atlas.
/// It delegates all terminal logic (cursor movement, character storage, scrolling) to
/// the Terminal class, focusing solely on visual presentation.
///
/// The display uses pixel-perfect rendering with integer scaling and NEAREST texture
/// filtering to maintain the authentic retro aesthetic.
/// </remarks>
[GlobalClass]
public partial class TerminalDisplay : Control
{
	#region Fields

	private Terminal? _terminal;
	private double _cursorBlinkTimer;
	private bool _cursorBlinkState;

	#endregion

	#region Properties

	/// <summary>
	/// Terminal configuration including dimensions, font, and visual settings.
	/// </summary>
	/// <remarks>
	/// Can be set in the Godot inspector or assigned programmatically.
	/// If not set, defaults to IBM PC configuration on _Ready().
	/// </remarks>
	[Export]
	public TerminalConfig? Config { get; set; }

	/// <summary>
	/// Gets the Terminal instance managing the character buffer.
	/// </summary>
	/// <remarks>
	/// Initialized in _Ready() using dimensions from Config.
	/// All text operations should be performed on this Terminal instance,
	/// followed by a call to UpdateDisplay().
	/// </remarks>
	public Terminal? Terminal => _terminal;

	#endregion

	#region Godot Lifecycle

	/// <summary>
	/// Called when the node enters the scene tree.
	/// </summary>
	public override void _Ready()
	{
		// Load default config if not set
		Config ??= TerminalConfig.LoadIBMPC();

		if (Config == null)
		{
			GD.PrintErr("TerminalDisplay: Failed to load terminal configuration");
			return;
		}

		// Validate font atlas
		if (Config.FontAtlas == null)
		{
			GD.PrintErr("TerminalDisplay: FontAtlas is null in TerminalConfig");
			return;
		}

		// Create terminal with configured dimensions
		_terminal = new Terminal(Config.Columns, Config.Rows);

		// Set control size to match terminal dimensions
		CustomMinimumSize = new Vector2(Config.DisplayWidth, Config.DisplayHeight);
		Size = CustomMinimumSize;

		// Initialize cursor blink state
		_cursorBlinkTimer = 0.0;
		_cursorBlinkState = true;

		// Initial draw
		QueueRedraw();
	}

	/// <summary>
	/// Called every frame for processing.
	/// </summary>
	/// <param name="delta">Time elapsed since last frame in seconds.</param>
	public override void _Process(double delta)
	{
		if (Config == null || _terminal == null)
		{
			return;
		}

		// Handle cursor blinking
		if (Config.CursorBlinkRate > 0 && _terminal.CursorVisible)
		{
			_cursorBlinkTimer += delta;

			if (_cursorBlinkTimer >= Config.CursorBlinkRate)
			{
				_cursorBlinkTimer = 0.0;
				_cursorBlinkState = !_cursorBlinkState;
				QueueRedraw(); // Redraw to show cursor blink
			}
		}
		else
		{
			// No blinking or cursor hidden
			_cursorBlinkState = true;
		}
	}

	/// <summary>
	/// Called when the node needs to be redrawn.
	/// </summary>
	public override void _Draw()
	{
		if (Config == null || _terminal == null || Config.FontAtlas == null)
		{
			return;
		}

		// Draw background
		DrawRect(new Rect2(Vector2.Zero, Size), Config.BackgroundColor, true);

		// Calculate scaled character size
		int scaledCharWidth = Config.CharacterWidth * Config.DisplayScale;
		int scaledCharHeight = Config.CharacterHeight * Config.DisplayScale;

		// Draw each character in the terminal buffer
		for (int y = 0; y < _terminal.Rows; y++)
		{
			for (int x = 0; x < _terminal.Columns; x++)
			{
				char ch = _terminal.GetChar(x, y);

				// Skip spaces (already drawn by background)
				if (ch == ' ')
				{
					continue;
				}

				// Get character rectangle from font atlas
				Rect2 sourceRect = Config.GetCharacterRect(ch);

				// Calculate destination position (scaled)
				Vector2 destPos = new Vector2(
					x * scaledCharWidth,
					y * scaledCharHeight
				);

				Rect2 destRect = new Rect2(destPos, new Vector2(scaledCharWidth, scaledCharHeight));

				// Draw character with foreground color modulation
				DrawTextureRectRegion(
					Config.FontAtlas,
					destRect,
					sourceRect,
					Config.ForegroundColor
				);
			}
		}

		// Draw cursor if visible and blinking state is on
		if (_terminal.CursorVisible && _cursorBlinkState)
		{
			Vector2 cursorPos = new Vector2(
				_terminal.CursorX * scaledCharWidth,
				_terminal.CursorY * scaledCharHeight
			);

			Rect2 cursorRect = new Rect2(
				cursorPos,
				new Vector2(scaledCharWidth, scaledCharHeight)
			);

			// Draw cursor as a filled rectangle
			DrawRect(cursorRect, Config.CursorColor, false, 2.0f * Config.DisplayScale);
		}
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Requests a redraw of the terminal display.
	/// </summary>
	/// <remarks>
	/// Call this after making changes to the Terminal buffer
	/// (e.g., after Write, WriteLine, Clear, etc.).
	/// </remarks>
	public void UpdateDisplay()
	{
		QueueRedraw();
	}

	#endregion
}
