# Retro Terminal

A Godot 4.7 + C# retro terminal environment frontend for the ECMA BASIC interpreter. Experience authentic 80s-style computing with period-accurate aesthetics, including CRT effects, retro fonts, and classic color palettes.

## Features

- **Authentic Terminal Emulation**: Character-based display with retro fonts
- **CRT Visual Effects**: Scanlines, phosphor glow, curvature, and screen flicker
- **ECMA BASIC Integration**: Full integration with the [ecma_basic](https://github.com/treytomes/ecma_basic) interpreter
- **Multiple Themes**: Commodore 64, Apple II, IBM PC, VT100, and custom themes
- **Modern Conveniences**: Optional features like copy/paste, file browser, and command history

## Related Projects

- [ecma_basic](https://github.com/treytomes/ecma_basic) - ECMA-55/116 BASIC interpreter

## Requirements

- Godot 4.7 or higher
- .NET 9.0 SDK or higher
- Windows, macOS, or Linux

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/treytomes/retro_term.git
cd retro_term
```

### Open in Godot

1. Launch Godot 4.7+
2. Click "Import"
3. Navigate to the `retro_term` directory
4. Select `project.godot`
5. Click "Import & Edit"

### Run the Project

In Godot editor:
- Press F5 or click the Play button

Or from command line:
```bash
godot project.godot
```

## Development

See [CLAUDE.md](CLAUDE.md) for comprehensive development guidelines, including:
- Spec-driven development workflow
- Git conventions (Conventional Commits)
- Testing standards (80% coverage minimum)
- Godot + C# best practices
- Architecture and project structure

### Building and Testing

```bash
# Build C# code
dotnet build

# Run tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Project Structure

```
retro_term/
├── scripts/         # C# source code
├── scenes/          # Godot scenes
├── assets/          # Fonts, shaders, themes
├── tests/           # xUnit test projects
├── .claude/         # Claude Code configuration
├── CLAUDE.md        # Development guide
└── README.md        # This file
```

## Contributing

1. Follow the spec-driven development approach (see [CLAUDE.md](CLAUDE.md))
2. Write tests before implementation (Red-Green-Refactor)
3. Use Conventional Commits format for commit messages
4. Maintain 80% minimum code coverage
5. Ensure all warnings are resolved (zero warnings policy)

## License

[License TBD]

## Credits

Built with [Godot Engine](https://godotengine.org/) and the [ecma_basic](https://github.com/treytomes/ecma_basic) interpreter.