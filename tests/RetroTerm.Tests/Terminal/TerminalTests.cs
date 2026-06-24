using RetroTerm.Terminal;

namespace RetroTerm.Tests.Terminal;

/// <summary>
/// Tests for the Terminal class character buffer management.
/// </summary>
/// <remarks>
/// Following TDD approach: these tests are written BEFORE implementation.
/// All tests should fail initially (RED phase).
/// </remarks>
public class TerminalTests
{
	#region Constructor and Initialization Tests

	[Fact]
	public void Constructor_ValidDimensions_CreatesTerminal()
	{
		// Arrange & Act
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Assert
		Assert.NotNull(terminal);
		Assert.Equal(80, terminal.Columns);
		Assert.Equal(25, terminal.Rows);
	}

	[Fact]
	public void Constructor_ValidDimensions_InitializesBufferWithSpaces()
	{
		// Arrange & Act
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Assert - all cells should be spaces
		for (int y = 0; y < 25; y++)
		{
			for (int x = 0; x < 80; x++)
			{
				Assert.Equal(' ', terminal.GetChar(x, y));
			}
		}
	}

	[Fact]
	public void Constructor_ValidDimensions_InitializesCursorAtOrigin()
	{
		// Arrange & Act
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Assert
		Assert.Equal(0, terminal.CursorX);
		Assert.Equal(0, terminal.CursorY);
	}

	[Fact]
	public void Constructor_ValidDimensions_SetsCursorVisible()
	{
		// Arrange & Act
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Assert
		Assert.True(terminal.CursorVisible);
	}

	[Theory]
	[InlineData(0, 25)]
	[InlineData(80, 0)]
	[InlineData(-1, 25)]
	[InlineData(80, -1)]
	public void Constructor_InvalidDimensions_ThrowsArgumentException(int columns, int rows)
	{
		// Arrange, Act & Assert
		Assert.Throws<ArgumentException>(() => new RetroTerm.Terminal.Terminal(columns, rows));
	}

	#endregion

	#region Character Access Tests

	[Fact]
	public void SetChar_ValidPosition_SetsCharacter()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.SetChar(10, 5, 'A');

		// Assert
		Assert.Equal('A', terminal.GetChar(10, 5));
	}

	[Fact]
	public void SetChar_MultiplePositions_SetsAllCharacters()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.SetChar(0, 0, 'H');
		terminal.SetChar(1, 0, 'E');
		terminal.SetChar(2, 0, 'L');
		terminal.SetChar(3, 0, 'L');
		terminal.SetChar(4, 0, 'O');

		// Assert
		Assert.Equal('H', terminal.GetChar(0, 0));
		Assert.Equal('E', terminal.GetChar(1, 0));
		Assert.Equal('L', terminal.GetChar(2, 0));
		Assert.Equal('L', terminal.GetChar(3, 0));
		Assert.Equal('O', terminal.GetChar(4, 0));
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(0, -1)]
	[InlineData(80, 0)]
	[InlineData(0, 25)]
	public void SetChar_OutOfBounds_ThrowsArgumentOutOfRangeException(int x, int y)
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act & Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => terminal.SetChar(x, y, 'A'));
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(0, -1)]
	[InlineData(80, 0)]
	[InlineData(0, 25)]
	public void GetChar_OutOfBounds_ThrowsArgumentOutOfRangeException(int x, int y)
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act & Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => terminal.GetChar(x, y));
	}

	#endregion

	#region Cursor Movement Tests

	[Fact]
	public void MoveCursor_ValidPosition_MovesCursor()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.MoveCursor(40, 12);

		// Assert
		Assert.Equal(40, terminal.CursorX);
		Assert.Equal(12, terminal.CursorY);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(0, -1)]
	[InlineData(80, 0)]
	[InlineData(0, 25)]
	public void MoveCursor_OutOfBounds_ThrowsArgumentOutOfRangeException(int x, int y)
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act & Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => terminal.MoveCursor(x, y));
	}

	[Fact]
	public void CursorX_SetValid_UpdatesCursor()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.CursorX = 50;

		// Assert
		Assert.Equal(50, terminal.CursorX);
	}

	[Fact]
	public void CursorY_SetValid_UpdatesCursor()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.CursorY = 10;

		// Assert
		Assert.Equal(10, terminal.CursorY);
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(80)]
	public void CursorX_SetOutOfBounds_ThrowsArgumentOutOfRangeException(int x)
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act & Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => terminal.CursorX = x);
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(25)]
	public void CursorY_SetOutOfBounds_ThrowsArgumentOutOfRangeException(int y)
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act & Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => terminal.CursorY = y);
	}

	#endregion

	#region Write Tests

	[Fact]
	public void Write_SingleCharacter_WritesAtCursor()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.Write("A");

		// Assert
		Assert.Equal('A', terminal.GetChar(0, 0));
		Assert.Equal(1, terminal.CursorX); // Cursor advanced
	}

	[Fact]
	public void Write_String_WritesAtCursorAndAdvances()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.Write("HELLO");

		// Assert
		Assert.Equal('H', terminal.GetChar(0, 0));
		Assert.Equal('E', terminal.GetChar(1, 0));
		Assert.Equal('L', terminal.GetChar(2, 0));
		Assert.Equal('L', terminal.GetChar(3, 0));
		Assert.Equal('O', terminal.GetChar(4, 0));
		Assert.Equal(5, terminal.CursorX);
	}

	[Fact]
	public void Write_EmptyString_DoesNothing()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.Write("");

		// Assert
		Assert.Equal(0, terminal.CursorX);
		Assert.Equal(0, terminal.CursorY);
	}

	[Fact]
	public void Write_StringWrapsToNextLine_WhenReachingEndOfLine()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);
		terminal.MoveCursor(78, 0);

		// Act
		terminal.Write("ABC");

		// Assert
		Assert.Equal('A', terminal.GetChar(78, 0));
		Assert.Equal('B', terminal.GetChar(79, 0));
		Assert.Equal('C', terminal.GetChar(0, 1)); // Wrapped to next line
		Assert.Equal(1, terminal.CursorX);
		Assert.Equal(1, terminal.CursorY);
	}

	#endregion

	#region WriteLine Tests

	[Fact]
	public void WriteLine_String_WritesAndAdvancesToNextLine()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.WriteLine("HELLO");

		// Assert
		Assert.Equal('H', terminal.GetChar(0, 0));
		Assert.Equal('E', terminal.GetChar(1, 0));
		Assert.Equal('L', terminal.GetChar(2, 0));
		Assert.Equal('L', terminal.GetChar(3, 0));
		Assert.Equal('O', terminal.GetChar(4, 0));
		Assert.Equal(0, terminal.CursorX); // Reset to column 0
		Assert.Equal(1, terminal.CursorY); // Next line
	}

	[Fact]
	public void WriteLine_EmptyString_AdvancesToNextLine()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.WriteLine("");

		// Assert
		Assert.Equal(0, terminal.CursorX);
		Assert.Equal(1, terminal.CursorY);
	}

	[Fact]
	public void WriteLine_MultipleLines_WritesSequentially()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.WriteLine("LINE 1");
		terminal.WriteLine("LINE 2");
		terminal.WriteLine("LINE 3");

		// Assert
		Assert.Equal('L', terminal.GetChar(0, 0));
		Assert.Equal('L', terminal.GetChar(0, 1));
		Assert.Equal('L', terminal.GetChar(0, 2));
		Assert.Equal(0, terminal.CursorX);
		Assert.Equal(3, terminal.CursorY);
	}

	#endregion

	#region Scrolling Tests

	[Fact]
	public void WriteLine_AtBottomRow_ScrollsTerminal()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);
		terminal.MoveCursor(0, 24); // Last row

		// Act
		terminal.WriteLine("BOTTOM LINE");

		// Assert
		Assert.Equal(0, terminal.CursorX);
		Assert.Equal(24, terminal.CursorY); // Still on last row
		Assert.Equal('B', terminal.GetChar(0, 24));
	}

	[Fact]
	public void Scroll_ScrollsContentUp()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);
		terminal.WriteLine("LINE 0");
		terminal.WriteLine("LINE 1");
		terminal.WriteLine("LINE 2");

		// Act
		terminal.Scroll();

		// Assert - first line should be gone, others moved up
		Assert.Equal('L', terminal.GetChar(0, 0)); // "LINE 1" moved to row 0
		Assert.Equal('L', terminal.GetChar(0, 1)); // "LINE 2" moved to row 1
		Assert.Equal(' ', terminal.GetChar(0, 2)); // New line is blank
	}

	[Fact]
	public void Scroll_ClearsBottomRow()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);
		for (int i = 0; i < 25; i++)
		{
			terminal.WriteLine($"LINE {i}");
		}

		// Act
		terminal.Scroll();

		// Assert - bottom row should be spaces
		for (int x = 0; x < 80; x++)
		{
			Assert.Equal(' ', terminal.GetChar(x, 24));
		}
	}

	#endregion

	#region Clear Tests

	[Fact]
	public void Clear_ResetsAllCharactersToSpaces()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);
		terminal.Write("HELLO WORLD");
		terminal.WriteLine("TEST");

		// Act
		terminal.Clear();

		// Assert - all cells should be spaces
		for (int y = 0; y < 25; y++)
		{
			for (int x = 0; x < 80; x++)
			{
				Assert.Equal(' ', terminal.GetChar(x, y));
			}
		}
	}

	[Fact]
	public void Clear_ResetsCursorToOrigin()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);
		terminal.MoveCursor(40, 12);

		// Act
		terminal.Clear();

		// Assert
		Assert.Equal(0, terminal.CursorX);
		Assert.Equal(0, terminal.CursorY);
	}

	#endregion

	#region CursorVisible Tests

	[Fact]
	public void CursorVisible_SetToFalse_HidesCursor()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);

		// Act
		terminal.CursorVisible = false;

		// Assert
		Assert.False(terminal.CursorVisible);
	}

	[Fact]
	public void CursorVisible_SetToTrue_ShowsCursor()
	{
		// Arrange
		var terminal = new RetroTerm.Terminal.Terminal(80, 25);
		terminal.CursorVisible = false;

		// Act
		terminal.CursorVisible = true;

		// Assert
		Assert.True(terminal.CursorVisible);
	}

	#endregion
}
