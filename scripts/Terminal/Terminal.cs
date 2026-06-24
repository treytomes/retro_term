namespace RetroTerm.Terminal;

/// <summary>
/// Core terminal emulator managing character buffer, cursor, and scrolling.
/// </summary>
/// <remarks>
/// This class handles all terminal state and logic without any Godot dependencies,
/// making it fully unit testable. Rendering is handled separately by TerminalDisplay.
///
/// The terminal maintains a 2D character buffer (columns × rows) with cursor tracking.
/// Text wraps to the next line when reaching the end of a row, and the terminal
/// scrolls up when writing past the bottom row.
/// </remarks>
public class Terminal
{
	private readonly char[,] _buffer;
	private int _cursorX;
	private int _cursorY;

	/// <summary>
	/// Gets the number of character columns in the terminal.
	/// </summary>
	public int Columns { get; }

	/// <summary>
	/// Gets the number of character rows in the terminal.
	/// </summary>
	public int Rows { get; }

	/// <summary>
	/// Gets or sets the cursor X position (column).
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">Value is out of bounds (0 to Columns-1).</exception>
	public int CursorX
	{
		get => _cursorX;
		set
		{
			if (value < 0 || value >= Columns)
			{
				throw new ArgumentOutOfRangeException(nameof(value),
					$"CursorX must be between 0 and {Columns - 1}, got {value}");
			}
			_cursorX = value;
		}
	}

	/// <summary>
	/// Gets or sets the cursor Y position (row).
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">Value is out of bounds (0 to Rows-1).</exception>
	public int CursorY
	{
		get => _cursorY;
		set
		{
			if (value < 0 || value >= Rows)
			{
				throw new ArgumentOutOfRangeException(nameof(value),
					$"CursorY must be between 0 and {Rows - 1}, got {value}");
			}
			_cursorY = value;
		}
	}

	/// <summary>
	/// Gets or sets whether the cursor is visible.
	/// </summary>
	public bool CursorVisible { get; set; }

	/// <summary>
	/// Initializes a new terminal with the specified dimensions.
	/// </summary>
	/// <param name="columns">Number of character columns (must be > 0).</param>
	/// <param name="rows">Number of character rows (must be > 0).</param>
	/// <exception cref="ArgumentException">Columns or rows is less than or equal to zero.</exception>
	public Terminal(int columns, int rows)
	{
		if (columns <= 0)
		{
			throw new ArgumentException("Columns must be greater than zero", nameof(columns));
		}
		if (rows <= 0)
		{
			throw new ArgumentException("Rows must be greater than zero", nameof(rows));
		}

		Columns = columns;
		Rows = rows;
		_buffer = new char[columns, rows];
		_cursorX = 0;
		_cursorY = 0;
		CursorVisible = true;

		// Initialize buffer with spaces
		Clear();
	}

	/// <summary>
	/// Sets a character at the specified position.
	/// </summary>
	/// <param name="x">Column position (0 to Columns-1).</param>
	/// <param name="y">Row position (0 to Rows-1).</param>
	/// <param name="c">Character to set.</param>
	/// <exception cref="ArgumentOutOfRangeException">Position is out of bounds.</exception>
	public void SetChar(int x, int y, char c)
	{
		ValidatePosition(x, y);
		_buffer[x, y] = c;
	}

	/// <summary>
	/// Gets the character at the specified position.
	/// </summary>
	/// <param name="x">Column position (0 to Columns-1).</param>
	/// <param name="y">Row position (0 to Rows-1).</param>
	/// <returns>Character at the specified position.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Position is out of bounds.</exception>
	public char GetChar(int x, int y)
	{
		ValidatePosition(x, y);
		return _buffer[x, y];
	}

	/// <summary>
	/// Writes text at the current cursor position.
	/// </summary>
	/// <param name="text">Text to write.</param>
	/// <remarks>
	/// Advances the cursor as characters are written.
	/// Text wraps to the next line when reaching the end of a row.
	/// If writing past the last row, the terminal scrolls up.
	/// </remarks>
	public void Write(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}

		foreach (char c in text)
		{
			// Write character at cursor
			_buffer[_cursorX, _cursorY] = c;

			// Advance cursor
			_cursorX++;

			// Wrap to next line if needed
			if (_cursorX >= Columns)
			{
				_cursorX = 0;
				_cursorY++;

				// Scroll if past last row
				if (_cursorY >= Rows)
				{
					Scroll();
					_cursorY = Rows - 1;
				}
			}
		}
	}

	/// <summary>
	/// Writes text at the current cursor position and advances to the next line.
	/// </summary>
	/// <param name="text">Text to write.</param>
	/// <remarks>
	/// After writing, the cursor moves to the beginning of the next line.
	/// If already on the last row, the cursor stays on the last row.
	/// </remarks>
	public void WriteLine(string text)
	{
		Write(text);

		// Move to beginning of next line
		_cursorX = 0;

		// If not on last row, advance to next row
		// If on last row, stay on last row (don't scroll automatically)
		if (_cursorY < Rows - 1)
		{
			_cursorY++;
		}
	}

	/// <summary>
	/// Moves the cursor to the specified position.
	/// </summary>
	/// <param name="x">Column position (0 to Columns-1).</param>
	/// <param name="y">Row position (0 to Rows-1).</param>
	/// <exception cref="ArgumentOutOfRangeException">Position is out of bounds.</exception>
	public void MoveCursor(int x, int y)
	{
		ValidatePosition(x, y);
		_cursorX = x;
		_cursorY = y;
	}

	/// <summary>
	/// Scrolls the terminal content up by one line.
	/// </summary>
	/// <remarks>
	/// The top line is discarded, all other lines move up one row,
	/// and the bottom line is cleared (filled with spaces).
	/// </remarks>
	public void Scroll()
	{
		// Move all rows up by one
		for (int y = 0; y < Rows - 1; y++)
		{
			for (int x = 0; x < Columns; x++)
			{
				_buffer[x, y] = _buffer[x, y + 1];
			}
		}

		// Clear the bottom row
		for (int x = 0; x < Columns; x++)
		{
			_buffer[x, Rows - 1] = ' ';
		}
	}

	/// <summary>
	/// Clears the entire terminal buffer and resets the cursor to the origin.
	/// </summary>
	/// <remarks>
	/// All characters are set to spaces, and the cursor moves to position (0, 0).
	/// </remarks>
	public void Clear()
	{
		// Fill buffer with spaces
		for (int y = 0; y < Rows; y++)
		{
			for (int x = 0; x < Columns; x++)
			{
				_buffer[x, y] = ' ';
			}
		}

		// Reset cursor to origin
		_cursorX = 0;
		_cursorY = 0;
	}

	/// <summary>
	/// Validates that a position is within terminal bounds.
	/// </summary>
	/// <param name="x">Column position.</param>
	/// <param name="y">Row position.</param>
	/// <exception cref="ArgumentOutOfRangeException">Position is out of bounds.</exception>
	private void ValidatePosition(int x, int y)
	{
		if (x < 0 || x >= Columns)
		{
			throw new ArgumentOutOfRangeException(nameof(x),
				$"X must be between 0 and {Columns - 1}, got {x}");
		}
		if (y < 0 || y >= Rows)
		{
			throw new ArgumentOutOfRangeException(nameof(y),
				$"Y must be between 0 and {Rows - 1}, got {y}");
		}
	}
}
