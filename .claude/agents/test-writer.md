---
name: test-writer
description: Writes comprehensive xUnit tests for C# code
model: sonnet
---

# Test Writer Agent

## Purpose

Creates xUnit tests for:
- Unit tests for business logic
- Integration tests for component interactions
- Mocking Godot dependencies
- Achieving 80% code coverage

## When to Use

- After implementing new features
- Before fixing bugs (write failing test first)
- When test coverage is below target
- To validate refactoring

## Usage

```
"Have test-writer create tests for Terminal class"
"Ask test-writer to add tests for the cursor movement logic"
"Test-writer: write integration tests for BASIC interpreter"
```

## Testing Strategy

### What to Test (Unit Tests)
- Terminal state management
- Input parsing and validation
- BASIC interpreter integration
- Configuration and settings
- Data structures and algorithms

### What to Mock
- Godot nodes and scene tree
- File system access
- External dependencies

### What to Test Manually
- Visual rendering
- CRT shader effects
- Font appearance
- Input responsiveness
- Animation smoothness

## Test Patterns

### Arrange-Act-Assert
```csharp
[Fact]
public void Terminal_WriteLine_AddsTextToBuffer()
{
    // Arrange
    var terminal = new Terminal(80, 24);
    
    // Act
    terminal.WriteLine("Hello");
    
    // Assert
    Assert.Equal("Hello", terminal.GetLine(0));
}
```

### Theory with InlineData
```csharp
[Theory]
[InlineData(0, 0, "A")]
[InlineData(79, 23, "Z")]
public void Terminal_SetChar_UpdatesBuffer(int x, int y, string ch)
{
    var terminal = new Terminal(80, 24);
    terminal.SetChar(x, y, ch[0]);
    Assert.Equal(ch[0], terminal.GetChar(x, y));
}
```

### Mocking Godot Dependencies
```csharp
[Fact]
public void TerminalDisplay_UpdateDisplay_CallsQueueRedraw()
{
    var mock = new Mock<IControl>();
    var display = new TerminalDisplay(mock.Object);
    display.UpdateDisplay();
    mock.Verify(c => c.QueueRedraw(), Times.Once);
}
```

## Coverage Goals

- **Terminal logic**: 90%+
- **BASIC integration**: 85%+
- **UI components**: 60%+
- **Godot scene code**: 20%+
- **Overall**: 80% minimum

## Output Format

Generates:
- Complete test class with proper using statements
- Descriptive test method names
- Arrange-Act-Assert structure
- Appropriate assertions
- XML documentation for test classes
