---
name: testing
description: Testing standards and code coverage requirements for retro_term
paths: ["**/*Test*.cs", "**/Tests/**/*.cs"]
---

# Testing Standards

## Code Coverage Requirements

**Minimum 80% code coverage** is required for all C# business logic code.

### Measuring Coverage

```bash
# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate coverage report (install reportgenerator first)
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:Html

# View report
start coverage-report/index.html
```

### Coverage Guidelines
- Focus on meaningful tests, not just hitting coverage metrics
- Prioritize testing:
  - Terminal emulation logic
  - BASIC interpreter integration
  - Input parsing and state management
  - Configuration and settings
  - Business logic and algorithms
- Lower priority for coverage:
  - Simple getters/setters
  - Godot scene initialization code
  - Visual-only code (test manually)
  - Trivial forwarding methods

## Test Structure

### Organization
- Test classes mirror production class structure
- Separate test projects for different concerns
- Keep test resources in `tests/Resources/` directory
- Use descriptive test names

### Naming Conventions
- Test classes: `{ClassName}Tests`
- Test methods: `MethodName_Scenario_ExpectedResult`
- Integration tests: `Integration_{Feature}_{Scenario}`

### Test Patterns

#### Arrange-Act-Assert
```csharp
[Fact]
public void Terminal_WriteLine_AddsTextToBuffer()
{
    // Arrange
    var terminal = new Terminal(80, 24);
    var expectedText = "Hello, World!";
    
    // Act
    terminal.WriteLine(expectedText);
    
    // Assert
    Assert.Equal(expectedText, terminal.GetLine(0));
}
```

#### Testing with Mocks (for Godot dependencies)
```csharp
[Fact]
public void TerminalDisplay_UpdateDisplay_CallsQueueRedraw()
{
    // Arrange
    var mockControl = new Mock<IControl>();
    var display = new TerminalDisplay(mockControl.Object);
    
    // Act
    display.UpdateDisplay();
    
    // Assert
    mockControl.Verify(c => c.QueueRedraw(), Times.Once);
}
```

#### Integration Tests
```csharp
[Fact]
public void BasicInterpreter_ExecutePrintStatement_OutputsToTerminal()
{
    // Arrange
    var terminal = new Terminal(80, 24);
    var env = new GodotTerminalEnvironment(terminal);
    var interpreter = new Interpreter(env);
    
    // Act
    interpreter.Execute("10 PRINT \"HELLO\"\n20 END");
    
    // Assert
    Assert.Contains("HELLO", terminal.GetAllText());
}
```

## Test Categories

### Unit Tests
- Test individual components in isolation
- Mock external dependencies (Godot nodes, file system)
- Fast execution (milliseconds)
- No I/O, no external dependencies

### Integration Tests
- Test component interactions
- Use real implementations where practical
- Test terminal + BASIC interpreter integration
- Validate signal routing

### Manual Testing (for UI/Visual)
- CRT shader effects
- Font rendering quality
- Color accuracy
- Input responsiveness
- Animation smoothness
- Theme switching

**Note**: Visual tests require running in Godot editor or exported build.

## xUnit Best Practices

### Attributes
- `[Fact]`: Simple test with no parameters
- `[Theory]`: Parameterized test with `[InlineData]`
- `[Skip("reason")]`: Temporarily disable tests (use sparingly)

### Assertions
- Use specific assertions: `Assert.Equal`, `Assert.True`, `Assert.Throws`
- Provide meaningful failure messages
- Assert one concept per test

### Test Isolation
- Each test should be independent
- Don't rely on test execution order
- Clean up resources in Dispose if needed

## Continuous Testing

### During Development
```bash
# Run tests in watch mode
dotnet watch test --project tests/RetroTerm.Tests
```

### Pre-Commit
- All tests must pass before committing
- Coverage should not decrease
- No skipped tests without documented reason

## Testing Godot Code

### Strategy for Godot Nodes

Since Godot nodes are difficult to unit test, separate concerns:

**Testable (C# logic)**:
- Business logic in pure C# classes
- Terminal state management
- BASIC interpreter integration
- Data structures and algorithms

**Manual testing (Godot-specific)**:
- Scene initialization
- Node tree structure
- Visual rendering
- Input handling
- Signals and event routing

### Example: Separation of Concerns

```csharp
// ✅ Testable - Pure C# logic
public class Terminal
{
    private readonly char[,] _buffer;
    private int _cursorX, _cursorY;
    
    public void WriteLine(string text)
    {
        // Logic here - easily testable
    }
}

// ❌ Hard to test - Godot-specific
public partial class TerminalDisplay : Control
{
    private Terminal _terminal;
    
    public override void _Ready()
    {
        _terminal = new Terminal(80, 24);
    }
    
    public override void _Draw()
    {
        // Render _terminal to screen
        // Test manually in Godot
    }
}
```

## Test-Driven Development

Follow the Red-Green-Refactor cycle:
1. **Red**: Write a failing test that defines desired behavior
2. **Green**: Implement minimum code to make the test pass
3. **Refactor**: Improve code quality while keeping tests passing

## Coverage Targets

| Component | Target | Reason |
|-----------|--------|--------|
| Terminal logic | 90%+ | Core business logic |
| BASIC integration | 85%+ | Critical functionality |
| UI components | 60%+ | Mixed logic/visual |
| Godot scene code | 20%+ | Mostly visual, manual testing |
| Utility classes | 95%+ | Shared code, high value |

**Overall project target: 80% minimum**
