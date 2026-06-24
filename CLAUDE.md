# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

A Godot 4.7 + C# retro terminal environment frontend for the ECMA BASIC interpreter. This project provides an authentic 80s-style terminal UI for running BASIC programs with period-accurate aesthetics (CRT effects, retro fonts, authentic color palettes).

**Related Project**: [ecma_basic](https://github.com/treytomes/ecma_basic) - The ECMA-55/116 BASIC interpreter  
**Repository**: https://github.com/treytomes/retro_term (TBD)  
**Wiki**: https://github.com/treytomes/retro_term/wiki (TBD)  
**Issues**: https://github.com/treytomes/retro_term/issues (TBD)

## Development Philosophy

### Spec-Driven Development

**CRITICAL: Never modify source code without a specification.**

Every code change requires ONE of:
1. **Failing test** that defines expected behavior (written first)
2. **GitHub issue** with acceptance criteria
3. **Design document** for UI/visual features
4. **Pure refactoring** (no behavior change, all tests pass)

#### Process
- **Specifications are GitHub Issues**: Feature requests and bugs are tracked as issues with acceptance criteria
- **Tests as Living Specs**: Every feature has tests that serve as executable specifications
- **Red-Green-Refactor**: Write failing tests first, implement to pass, then refactor
- **Documentation in Wiki**: Project notes, architecture decisions, and guides live in the GitHub wiki

If asked to modify code without a spec, STOP and request either a GitHub issue or write the test first.

### Security and Secrets Management

**CRITICAL: Never commit secrets, tokens, or API keys to git.**

- ✅ **Safe to commit**: `.claude/settings.json` (no secrets)
- 🔒 **Never commit**: `.claude/settings.local.json` (gitignored, contains tokens)

**Token Storage Options**:
1. `.claude/settings.local.json` (recommended for development)
2. System environment variable `GITHUB_TOKEN`
3. GitHub CLI credential store: `gh auth token`

See [`.claude/SECRETS_MANAGEMENT.md`](.claude/SECRETS_MANAGEMENT.md) for complete security guide.

### Clean Architecture
- **Godot Scene Structure**: Organized by feature/screen with clear parent-child relationships
- **C# Scripts**: One class per file, organized by responsibility
- **Signals**: Use Godot signals for decoupled communication between components
- **Dependency Rule**: UI depends on business logic, business logic has no UI dependencies
- **Testable Design**: Separate terminal logic from Godot-specific rendering code

### Quality Standards
- **80% Code Coverage Minimum**: All business logic must maintain 80%+ test coverage
- **Nullable Reference Types**: Explicit null handling throughout C# codebase
- **Modern C# Patterns**: File-scoped namespaces, target-typed new, pattern matching, var keyword
- **Zero Warnings**: Build treats ALL warnings as errors
  - The build WILL FAIL if any warnings exist
  - This includes nullable warnings, code style warnings, and XML documentation warnings
  - New C# files automatically follow this standard

## Project Structure

```
retro_term/
├── .claude/                    # Claude Code configuration
│   ├── README.md
│   ├── SECRETS_MANAGEMENT.md
│   ├── settings.json          # ✅ Committed (no secrets)
│   ├── settings.local.json    # 🔒 Gitignored (contains tokens)
│   ├── rules/                 # Development standards
│   ├── skills/                # User-invocable commands
│   ├── agents/                # Specialized subagents
│   └── workflows/             # Multi-agent orchestration
├── scripts/                   # C# source code
│   ├── Terminal/              # Terminal emulation logic
│   ├── BasicInterop/          # ECMA BASIC integration
│   ├── UI/                    # UI components
│   └── Utils/                 # Utility classes
├── scenes/                    # Godot scenes
│   ├── main.tscn             # Main terminal scene
│   ├── components/           # Reusable UI components
│   └── screens/              # Different screens/modes
├── assets/                    # Art, fonts, shaders
│   ├── fonts/                # Retro terminal fonts
│   ├── shaders/              # CRT effects, scanlines
│   └── themes/               # Visual themes (C64, Apple II, etc.)
├── tests/                     # Test projects
│   └── RetroTerm.Tests/      # xUnit test project
├── project.godot              # Godot project configuration
├── CLAUDE.md                  # This file
└── README.md                  # Project README
```

## Development Commands

### Godot Commands

```bash
# Run the project in Godot editor (requires Godot 4.7+ installed)
godot -e project.godot

# Run the project directly
godot project.godot

# Export for Windows
godot --export "Windows Desktop" builds/retro_term.exe

# Run with console output (for debugging)
godot --verbose project.godot
```

### Building and Testing C# Code

```bash
# Build C# code
dotnet build

# Run tests with coverage (minimum 80% required)
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/RetroTerm.Tests/RetroTerm.Tests.csproj

# Watch mode for continuous testing
dotnet watch test --project tests/RetroTerm.Tests
```

### Quick Start Scripts

Convenience scripts for common tasks:

```bash
# Build and run (create these as needed)
./run.sh        # or run.bat on Windows
./test.sh       # or test.bat on Windows
./export.sh     # or export.bat on Windows
```

## Architecture

### Godot + C# Integration

The project uses Godot 4.7 with C# scripting:

- **Godot Engine**: Handles rendering, input, audio, and scene management
- **C# Scripts**: Business logic, terminal emulation, BASIC interpreter integration
- **Signals**: Godot's event system for component communication
- **Resources**: Data structures using Godot's Resource system

### Key Components

**Terminal Emulator** (`scripts/Terminal/`):
- Character grid rendering with retro fonts
- Cursor management and blinking
- Input handling (keyboard, copy/paste)
- ANSI/VT100 control sequences (optional)

**BASIC Integration** (`scripts/BasicInterop/`):
- Wrapper around ECMABasic.Application
- Redirects BASIC I/O to terminal display
- Handles RUN, LIST, LOAD, SAVE commands
- Program storage and management

**Visual Effects** (`assets/shaders/`):
- CRT shader with scanlines and curvature
- Phosphor glow and bloom effects
- Screen flicker and noise
- Authentic color palettes (C64, Apple II, IBM CGA, etc.)

**UI Components** (`scripts/UI/`):
- Menu system for settings
- File browser for LOAD/SAVE
- Theme selector
- Configuration panels

### Testing Strategy

**Unit Tests** (C# Logic):
- Terminal state management
- BASIC interpreter integration
- Input parsing and validation
- Configuration and settings

**Integration Tests** (C# + Godot):
- Scene loading and initialization
- Signal routing
- Resource loading
- Multi-component interactions

**Manual Testing** (UI/Visual):
- CRT effects and visual quality
- Font rendering and readability
- Input responsiveness
- Theme switching
- BASIC program execution

## Coding Standards

### C# Code Style

**File Organization**:
- One public type per file
- File name matches type name
- Use file-scoped namespaces
- Group using statements

**Naming Conventions**:
- PascalCase for types, methods, properties, constants
- camelCase for local variables, parameters, private fields
- _camelCase for private fields (optional, follow Godot conventions)
- Descriptive names over abbreviations

**Godot-Specific**:
- Override `_Ready()` for initialization
- Override `_Process(double delta)` for per-frame updates
- Use `[Export]` attribute for inspector-editable properties
- Use `GetNode<T>()` for node references
- Call base class methods when overriding

### Godot Scene Organization

- Keep scenes focused on single responsibilities
- Use scene instancing for reusability
- Organize nodes hierarchically by feature
- Use groups for batch operations
- Name nodes descriptively (PascalCase)

### XML Documentation Required

All public APIs must have complete XML documentation:

```csharp
/// <summary>
/// Writes a line of text to the terminal at the cursor position.
/// </summary>
/// <param name="text">The text to write.</param>
/// <remarks>
/// Advances the cursor to the next line after writing.
/// Scrolls the terminal if the cursor reaches the bottom.
/// </remarks>
public void WriteLine(string text)
{
    // Implementation
}
```

## Git Conventions

### Commit Message Format

All commits MUST follow [Conventional Commits](https://www.conventionalcommits.org/) format:

```
<type>[optional scope]: <description>

[optional body]

Co-Authored-By: Claude Sonnet 4.5 <noreply@anthropic.com>
```

**Types**: `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `revert`

**Examples**:
```
feat(terminal): add CRT scanline shader effect
fix(basic): handle null reference in program execution
docs: update README with installation instructions
style(ui): convert to file-scoped namespaces
refactor(terminal): extract cursor logic to separate class
```

**Rules**:
- Use imperative mood: "add" not "added"
- Lowercase description, no period at end
- Subject line ≤ 72 characters
- Body explains WHY, not WHAT
- Always include Co-Authored-By footer for Claude commits

See `.claude/rules/git-conventions.md` for complete specification.

### Branch Naming

```
<type>/<issue-number>-<short-description>

Examples:
feature/1-terminal-emulator
fix/42-cursor-position-bug
refactor/3-extract-shader-system
docs/update-readme
```

## Testing Patterns

### Test Organization
Tests follow xUnit conventions organized by component with **80% minimum code coverage requirement**:

- Test methods named `MethodName_Scenario_ExpectedResult`
- Focus on business logic and integration points
- Mock Godot dependencies when needed
- Test resources in `tests/Resources/` directory

### Common Test Pattern
```csharp
[Fact]
public void Terminal_WriteLine_AddsTextToBuffer()
{
    // Arrange
    var terminal = new Terminal(80, 24);
    
    // Act
    terminal.WriteLine("Hello, World!");
    
    // Assert
    Assert.Equal("Hello, World!", terminal.GetLine(0));
}
```

### Test-Driven Development
Follow the Red-Green-Refactor cycle:
1. **Red**: Write a failing test that defines desired behavior
2. **Green**: Implement minimum code to make the test pass
3. **Refactor**: Improve code quality while keeping tests passing

## Common Patterns

### Godot Node Initialization

```csharp
using Godot;

namespace RetroTerm.UI;

public partial class TerminalDisplay : Control
{
    [Export]
    public Font? TerminalFont { get; set; }
    
    private Terminal? _terminal;
    
    public override void _Ready()
    {
        _terminal = new Terminal(80, 24);
        // Initialize
    }
    
    public override void _Process(double delta)
    {
        // Per-frame updates
    }
}
```

### Signal Usage

```csharp
[Signal]
public delegate void TextInputEventHandler(string text);

public void HandleInput(string text)
{
    EmitSignal(SignalName.TextInput, text);
}
```

### Resource Loading

```csharp
var font = GD.Load<Font>("res://assets/fonts/retro_terminal.tres");
var shader = GD.Load<Shader>("res://assets/shaders/crt_effect.gdshader");
```

## Godot + ECMA BASIC Integration

### Approach

The retro terminal will:
1. **Embed** the ECMABasic.Application library
2. **Wrap** the interpreter with a custom `IEnvironment` implementation
3. **Redirect** I/O through the terminal display
4. **Provide** authentic retro UI for BASIC programming

### IEnvironment Implementation

```csharp
public class GodotTerminalEnvironment : EnvironmentBase
{
    private readonly TerminalDisplay _display;
    
    public override void Write(string text)
    {
        _display.WriteText(text);
    }
    
    public override string? ReadLine()
    {
        return _display.ReadLineFromUser();
    }
    
    // ... other IEnvironment methods
}
```

## Visual Design Goals

### Authentic Retro Aesthetic
- **CRT Effects**: Scanlines, curvature, phosphor glow, bloom
- **Period Fonts**: C64 PETSCII, Apple II, IBM PC, VT100
- **Color Palettes**: Authentic system colors (C64, Apple II, CGA, EGA, VGA)
- **Behavior**: Cursor blinking, screen flicker (subtle), boot sequence

### Configurable Themes
- Commodore 64
- Apple II
- IBM PC (CGA, EGA, VGA)
- VT100/VT220 terminal
- Custom user themes

### Modern Conveniences (Optional Toggles)
- Copy/paste support
- File browser UI (instead of typed paths)
- Syntax highlighting (subtle)
- Command history
- Resizable window with proper scaling

## Future Considerations

- Network multiplayer (BBS-style)
- Save/load terminal state
- Tape/disk drive emulation with audio
- Multiple terminal sessions
- Screenshot/GIF recording
- ASCII art editor integration
- Integration with other ECMA BASIC variants (ECMA-116)

## References

- [Godot 4.7 Documentation](https://docs.godotengine.org/en/stable/)
- [Godot C# Documentation](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/)
- [ECMA BASIC Project](https://github.com/treytomes/ecma_basic)
- [ECMA-55 Specification](https://www.ecma-international.org/publications-and-standards/standards/ecma-55/)
