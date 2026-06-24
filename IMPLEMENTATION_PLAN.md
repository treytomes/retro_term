# Implementation Plan

## Current Status

✅ **Phase 1: Project Setup & Configuration** (Complete)
- Project structure initialized
- Claude Code configuration complete
- Development standards documented
- OEM437 fonts imported (8, 12, 16 pixel heights)
- TerminalConfig resource implemented
- Repository created: https://github.com/treytomes/retro_term

## Next Steps

### Phase 2: Core Terminal Implementation

Follow spec-driven development: **Write tests first, then implement.**

#### Issue #1: Terminal Class
**File**: `scripts/Terminal/Terminal.cs`  
**Status**: 🔴 Not Started  
**GitHub**: https://github.com/treytomes/retro_term/issues/1

Pure C# business logic for character buffer management.

**Key Features:**
- 2D character buffer (columns × rows)
- Cursor position tracking and movement
- Character read/write operations
- Scrolling when cursor reaches bottom
- Clear screen functionality

**Testing:**
- Create `tests/RetroTerm.Tests/Terminal/TerminalTests.cs`
- Write failing tests for each feature
- Implement to make tests pass
- Target: 80%+ coverage

**Dependencies:** None (pure C# logic)

---

#### Issue #2: TerminalDisplay Node
**File**: `scripts/Terminal/TerminalDisplay.cs`  
**Scene**: `scenes/components/terminal_display.tscn`  
**Status**: 🔴 Not Started  
**GitHub**: https://github.com/treytomes/retro_term/issues/2

Godot Control node that renders terminal to screen.

**Key Features:**
- Render character grid using font atlas
- Pixel-perfect rendering (NEAREST filtering)
- Cursor rendering with blink animation
- Color support (foreground, background, cursor)
- Integer scaling from TerminalConfig

**Testing:**
- Manual visual testing in Godot editor
- Verify character alignment
- Test cursor blinking
- Verify color accuracy
- Test different scales (1x, 2x, 3x, 4x)

**Dependencies:** Issue #1 (Terminal.cs)

---

#### Issue #3: GodotTerminalEnvironment
**File**: `scripts/BasicInterop/GodotTerminalEnvironment.cs`  
**Status**: 🔴 Not Started  
**GitHub**: https://github.com/treytomes/retro_term/issues/3

Bridge between ECMA BASIC interpreter and terminal.

**Key Features:**
- Implement `IEnvironment` from ECMABasic.Application
- Redirect BASIC output to terminal
- Handle BASIC input (blocking reads)
- Support PRINT, INPUT, CLS statements

**Testing:**
- Unit tests with mock Terminal
- Integration tests with simple BASIC programs
- Test PRINT statement output
- Test INPUT statement behavior
- Target: 80%+ coverage

**Dependencies:**
- Issue #1 (Terminal.cs)
- ECMABasic.Application library (add as reference)

---

#### Issue #4: CRT Shader Effect
**File**: `assets/shaders/crt_effect.gdshader`  
**Status**: 🔴 Not Started  
**GitHub**: https://github.com/treytomes/retro_term/issues/4

Visual shader for authentic CRT appearance.

**Key Features:**
- Scanline effect (horizontal lines)
- Optional screen curvature (subtle barrel distortion)
- Configurable intensity
- Maintains text readability
- 60 FPS performance target

**Testing:**
- Manual visual testing
- Test different intensities
- Verify readability at all settings
- Performance profiling

**Dependencies:** Issue #2 (TerminalDisplay.cs)

---

## Implementation Order

Follow this sequence to maintain dependency chain:

### Week 1: Core Terminal
1. **Issue #1** - Terminal.cs
   - Write tests first
   - Implement character buffer
   - Implement cursor logic
   - Implement scrolling

2. **Issue #2** - TerminalDisplay.cs
   - Create Godot scene
   - Implement rendering
   - Test visual output
   - Verify pixel-perfect display

### Week 2: BASIC Integration & Visual Polish
3. **Issue #3** - GodotTerminalEnvironment.cs
   - Add ECMABasic.Application reference
   - Implement IEnvironment
   - Test with simple BASIC programs
   - Verify I/O redirection

4. **Issue #4** - CRT Shader
   - Create shader file
   - Implement scanline effect
   - Add curvature (optional)
   - Tune for readability

---

## Development Workflow

For each issue:

### 1. Specification Phase
- ✅ GitHub issue created with acceptance criteria
- Read and understand requirements
- Clarify any ambiguities

### 2. Test Phase (Red)
- Create test file structure
- Write failing tests for each acceptance criterion
- Run tests to verify they fail
- Commit tests with message: `test: add failing tests for X`

### 3. Implementation Phase (Green)
- Implement minimum code to pass tests
- Run tests frequently
- Commit when all tests pass: `feat: implement X`

### 4. Refactor Phase
- Improve code quality
- Maintain test coverage
- Run tests to ensure no regression
- Commit improvements: `refactor: improve X`

### 5. Documentation Phase
- Add XML documentation comments
- Update README files
- Create usage examples
- Commit docs: `docs: document X`

### 6. Review Phase
- Use code-reviewer agent: "code-reviewer: review scripts/Terminal/Terminal.cs"
- Address any issues found
- Ensure 80%+ test coverage
- Verify zero warnings

### 7. Close Issue
- Verify all acceptance criteria met
- Reference issue in final commit
- Close issue with summary comment

---

## Branch Strategy

Use feature branches for each issue:

```bash
# Issue #1
git checkout -b feature/1-terminal-class
# ... implement and test ...
git commit -m "feat(terminal): implement Terminal class

Closes #1"
git push -u origin feature/1-terminal-class
# Create PR and merge

# Issue #2
git checkout main
git pull
git checkout -b feature/2-terminal-display
# ... implement and test ...
```

---

## Testing Standards

### Unit Tests (xUnit)
- **Coverage**: 80% minimum for C# logic
- **Pattern**: Arrange-Act-Assert
- **Naming**: `MethodName_Scenario_ExpectedResult`
- **Location**: `tests/RetroTerm.Tests/`

### Manual Tests (Godot)
- **Visual**: Rendering, colors, effects
- **Input**: Keyboard, mouse interactions
- **Performance**: 60 FPS target

### Integration Tests
- **BASIC Programs**: Test complete workflow
- **I/O**: Verify input/output routing
- **Scenarios**: End-to-end user workflows

---

## Success Criteria

Phase 2 is complete when:

- ✅ All 4 issues closed
- ✅ All tests passing
- ✅ 80%+ code coverage maintained
- ✅ Zero build warnings
- ✅ Can run simple BASIC programs in terminal
- ✅ Terminal displays with CRT effect
- ✅ Documentation complete
- ✅ Code reviewed by code-reviewer agent

---

## Future Phases

### Phase 3: Input & Interaction
- Keyboard input handling
- Copy/paste support
- Text selection
- Command history

### Phase 4: Theme System
- Save/load theme presets
- Theme editor UI
- Multiple retro system themes
- Custom color palettes

### Phase 5: Advanced Features
- File browser UI
- Settings menu
- Program management
- Save states

---

## Resources

- **Repository**: https://github.com/treytomes/retro_term
- **Issues**: https://github.com/treytomes/retro_term/issues
- **ECMA BASIC**: https://github.com/treytomes/ecma_basic
- **Godot Docs**: https://docs.godotengine.org/

---

## Getting Help

Use Claude Code agents:
```
"code-reviewer: review scripts/Terminal/Terminal.cs"
"test-writer: create tests for Terminal class"
"ui-designer: plan the main terminal scene"
```

Read development guides:
- `CLAUDE.md` - Complete development guide
- `.claude/rules/spec-first.md` - Spec-driven development
- `.claude/rules/testing.md` - Testing standards
- `.claude/rules/godot-conventions.md` - Godot best practices

---

**Ready to start Phase 2! 🚀**

Begin with Issue #1: Terminal class implementation.
