---
name: godot-conventions
description: Godot 4.7 + C# coding conventions and best practices
paths: ["scripts/**/*.cs", "scenes/**/*.tscn"]
---

# Godot Conventions

## Godot + C# Best Practices

### File Organization

**C# Scripts**:
- One public type per file
- File name matches type name (PascalCase)
- Place in `scripts/` directory organized by feature
- Use file-scoped namespaces

**Scenes**:
- One scene per `.tscn` file
- Place in `scenes/` directory organized by purpose
- Use descriptive names (PascalCase)
- Keep scenes focused and composable

### Naming Conventions

**C# Code**:
- Types, methods, properties: `PascalCase`
- Local variables, parameters: `camelCase`
- Private fields: `_camelCase` (leading underscore)
- Constants: `UPPER_SNAKE_CASE` or `PascalCase`

**Godot Nodes**:
- Node names in scene tree: `PascalCase`
- Node paths: Use `%` syntax for unique names when possible
- Groups: `snake_case`
- Signals: `PascalCase` (matches C# delegates)

### Node Lifecycle Methods

Override these methods in your C# scripts attached to nodes:

```csharp
public partial class MyNode : Node
{
    // Called when node enters the scene tree
    public override void _Ready()
    {
        base._Ready();  // Call if parent has logic
        // Initialize here
    }
    
    // Called every frame (delta = time since last frame)
    public override void _Process(double delta)
    {
        base._Process(delta);
        // Per-frame updates
    }
    
    // Called every physics frame (fixed timestep)
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        // Physics updates
    }
    
    // Called when node is about to be removed
    public override void _ExitTree()
    {
        // Cleanup here
        base._ExitTree();
    }
}
```

### Exported Properties

Use `[Export]` for inspector-editable properties:

```csharp
public partial class Terminal : Control
{
    [Export]
    public Font? TerminalFont { get; set; }
    
    [Export(PropertyHint.Range, "8,24,1")]
    public int FontSize { get; set; } = 16;
    
    [Export]
    public Color BackgroundColor { get; set; } = Colors.Black;
    
    [Export]
    public PackedScene? ComponentScene { get; set; }
}
```

### Node References

**Getting Node References**:

```csharp
// Type-safe node access
var label = GetNode<Label>("UI/StatusLabel");
var timer = GetNode<Timer>("../SharedTimer");

// Using % for unique names (recommended)
var uniqueLabel = GetNode<Label>("%StatusLabel");

// Optional node (may not exist)
var optional = GetNodeOrNull<Button>("OptionalButton");

// Cache references in _Ready()
private Label? _statusLabel;

public override void _Ready()
{
    _statusLabel = GetNode<Label>("%StatusLabel");
}
```

**Avoid**:
- Don't call `GetNode()` in `_Process()` (cache in `_Ready()`)
- Don't use string paths without type checking
- Don't assume nodes exist without null checks

### Signals

**Defining Signals**:

```csharp
public partial class Terminal : Control
{
    // Define signal with delegate
    [Signal]
    public delegate void TextInputEventHandler(string text);
    
    [Signal]
    public delegate void CommandExecutedEventHandler(string command, bool success);
    
    // Emit signals
    private void OnUserInput(string text)
    {
        EmitSignal(SignalName.TextInput, text);
    }
}
```

**Connecting Signals**:

```csharp
// Connect in code (C# method)
terminal.TextInput += OnTerminalTextInput;

// Or use Godot's Connect method
terminal.Connect(Terminal.SignalName.TextInput, 
    Callable.From<string>(OnTerminalTextInput));

// Handler method
private void OnTerminalTextInput(string text)
{
    GD.Print($"Received: {text}");
}
```

**Signal Naming**:
- Use present tense verbs: `Pressed`, `TextChanged`, `CommandExecuted`
- Name describes what happened, not what to do
- Use descriptive EventHandler suffix for delegate

### Resource Loading

```csharp
// Load resources
var font = GD.Load<Font>("res://assets/fonts/terminal.tres");
var shader = GD.Load<Shader>("res://assets/shaders/crt.gdshader");
var scene = GD.Load<PackedScene>("res://scenes/menu.tscn");

// Preload (resolved at compile time, faster)
private static readonly Font DefaultFont = 
    GD.Load<Font>("res://assets/fonts/default.tres");

// Resource paths always use forward slashes and "res://" prefix
// No "\" or Windows paths
```

### Scene Instancing

```csharp
// Load and instance a scene
var menuScene = GD.Load<PackedScene>("res://scenes/menu.tscn");
var menu = menuScene.Instantiate<Control>();

// Add to scene tree
AddChild(menu);

// Or with specific parent
GetNode("%Container").AddChild(menu);

// Remove and queue free
menu.QueueFree();  // Deferred deletion (safe in _Process)
// or
RemoveChild(menu);
menu.Free();  // Immediate deletion (not safe in _Process)
```

### Input Handling

```csharp
public partial class Terminal : Control
{
    public override void _Input(InputEvent @event)
    {
        // Handle unhandled input
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            HandleKeyPress(keyEvent);
            // Mark as handled
            AcceptEvent();
        }
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        // Only receives events not handled by UI
        if (@event.IsActionPressed("ui_cancel"))
        {
            // Handle
            AcceptEvent();
        }
    }
    
    // Check input in _Process
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("jump"))
        {
            // Handle jump
        }
    }
}
```

### Custom Resources

Create reusable data structures:

```csharp
[GlobalClass]
public partial class TerminalTheme : Resource
{
    [Export]
    public Color ForegroundColor { get; set; } = Colors.Green;
    
    [Export]
    public Color BackgroundColor { get; set; } = Colors.Black;
    
    [Export]
    public Font? Font { get; set; }
    
    [Export]
    public int FontSize { get; set; } = 16;
}

// Use in scenes
public partial class Terminal : Control
{
    [Export]
    public TerminalTheme? Theme { get; set; }
    
    public override void _Ready()
    {
        if (Theme != null)
        {
            // Apply theme
        }
    }
}
```

### Debugging

```csharp
// Console output
GD.Print("Debug message");
GD.PrintErr("Error message");
GD.PrintRaw("No newline");

// String formatting
GD.Print($"Value: {myVar}");

// Assertions (removed in release builds)
GD.Assert(condition, "Assertion message");

// Conditional compilation
#if DEBUG
    GD.Print("Debug only");
#endif
```

### Performance Tips

**Do**:
- ✅ Cache node references in `_Ready()`
- ✅ Use `_PhysicsProcess()` for consistent timing
- ✅ Pool frequently created objects
- ✅ Use `QueueFree()` instead of `Free()` in loops
- ✅ Disable `_Process()` when not needed: `SetProcess(false)`

**Avoid**:
- ❌ `GetNode()` calls in `_Process()` or `_PhysicsProcess()`
- ❌ String allocations in hot paths
- ❌ LINQ in `_Process()` (use for loops)
- ❌ Creating new objects every frame
- ❌ Expensive operations without caching

### Scene Structure Best Practices

**Organize by Feature**:
```
scenes/
├── main.tscn                  # Main entry point
├── components/                # Reusable UI components
│   ├── terminal_display.tscn
│   ├── menu_button.tscn
│   └── file_dialog.tscn
├── screens/                   # Full screen scenes
│   ├── main_terminal.tscn
│   ├── settings.tscn
│   └── about.tscn
└── ui/                        # UI-specific scenes
    ├── hud.tscn
    └── status_bar.tscn
```

**Scene Composition**:
- Keep scenes small and focused
- Use scene instancing for reusability
- Prefer composition over deep hierarchies
- Use signals for communication between scenes

### Godot-Specific C# Attributes

```csharp
// Make class available in Godot editor
[GlobalClass]
public partial class MyClass : Resource { }

// Export property to inspector
[Export]
public int Value { get; set; }

// Export with hint
[Export(PropertyHint.Range, "0,100,1")]
public float Percentage { get; set; }

[Export(PropertyHint.File, "*.json,*.txt")]
public string FilePath { get; set; } = "";

[Export(PropertyHint.Enum, "Option1,Option2,Option3")]
public int Choice { get; set; }

// Define signal
[Signal]
public delegate void MySignalEventHandler(int value);

// Tool script (runs in editor)
[Tool]
public partial class EditorTool : Node { }

// RPC attribute (multiplayer)
[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
public void NetworkMethod() { }
```

### Null Safety with Godot

```csharp
// Godot nodes may be null
public partial class MyNode : Node
{
    private Label? _label;  // Nullable
    
    public override void _Ready()
    {
        // Safe access with null check
        _label = GetNodeOrNull<Label>("Label");
        
        if (_label != null)
        {
            _label.Text = "Hello";
        }
        
        // Or with null-coalescing
        _label?.SetText("Hello");
        
        // Or with null-forgiving (if you're certain)
        _label = GetNode<Label>("Label");  // Throws if not found
    }
}
```

### Godot C# vs GDScript Differences

**No Dynamic Types**:
```csharp
// ❌ No GDScript-style dynamic typing
// var node = get_node("Path")  # GDScript

// ✅ Use generic GetNode<T>
var node = GetNode<Label>("Path");
```

**Property Access**:
```csharp
// GDScript: label.text = "Hello"
// C#: Same, but type-safe
var label = GetNode<Label>("Label");
label.Text = "Hello";  // Strongly typed
```

**Built-in Types**:
```csharp
// GDScript: Vector2, Color, etc. are built-in
// C#: Use Godot.Vector2, Godot.Color

using Godot;

var position = new Vector2(10, 20);
var color = new Color(1, 0, 0);  // Red
```

### Common Patterns

**Singleton Pattern (Autoload)**:
```csharp
// Create as AutoLoad in project settings
public partial class GameManager : Node
{
    private static GameManager? _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new InvalidOperationException(
                    "GameManager not initialized");
            }
            return _instance;
        }
    }
    
    public override void _Ready()
    {
        _instance = this;
    }
}

// Usage from anywhere
GameManager.Instance.DoSomething();
```

**State Machine**:
```csharp
public partial class Player : Node
{
    private enum State
    {
        Idle,
        Moving,
        Jumping
    }
    
    private State _currentState = State.Idle;
    
    public override void _Process(double delta)
    {
        switch (_currentState)
        {
            case State.Idle:
                ProcessIdle(delta);
                break;
            case State.Moving:
                ProcessMoving(delta);
                break;
            case State.Jumping:
                ProcessJumping(delta);
                break;
        }
    }
    
    private void TransitionTo(State newState)
    {
        _currentState = newState;
    }
}
```

## Testing Considerations

**Separate Godot from Logic**:
```csharp
// ✅ Testable - Pure C# logic
public class Terminal
{
    public void WriteLine(string text) { }
}

// ❌ Hard to test - Godot-dependent
public partial class TerminalControl : Control
{
    private Terminal _terminal = new();
    
    public override void _Draw()
    {
        // Godot rendering
    }
}
```

**Mock Godot Dependencies**:
```csharp
// Use interfaces for testability
public interface ITerminalDisplay
{
    void UpdateDisplay();
}

public partial class TerminalControl : Control, ITerminalDisplay
{
    public void UpdateDisplay()
    {
        QueueRedraw();
    }
}

// Test with mock
var mock = new Mock<ITerminalDisplay>();
```

## References

- [Godot C# Documentation](https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/)
- [C# API Reference](https://docs.godotengine.org/en/stable/classes/)
- [Godot Best Practices](https://docs.godotengine.org/en/stable/tutorials/best_practices/)
