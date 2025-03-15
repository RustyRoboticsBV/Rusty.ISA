# Rusty.ISA
A generic *instruction set architecture* module (ISA for short), written for the Godot game engine. It allows you to:
- Define an instruction set.
- Write programs using this instruction set.
- Execute these programs from within a Godot game.

The module is mostly implemented in C#, and partially in GDScript.

Related repos:
- [Editor](https://github.com/RustyRoboticsBV/Rusty.ISA.Editor): A graph-based editor interface. While you can write programs by hand or in the inspector, this is not encouraged.
- [Importer](https://github.com/RustyRoboticsBV/Rusty.ISA.Importer): A set of plugins that can import some of the resources in this module from various file types.

## Terminology
In this module, we define a *process* as a node that can run a *program*, which consist of *instruction instances*. In order to execute instructions, the process uses an *instruction set*, which contains *instruction definitions*. Each definition contains an opcode, parameters and an implementation. The instances contain an opcode and arguments. The opcode is used to match an instance with its definition.

You can essentially think of a program as an Assembly program. A process can start executing a program at explicitly-defined start points, and stops when it reaches an end point. Programs may have multiple start points. Flow control is done via jump and label instructions.

## Built-in Instructions
The module only comes with a few built-in instructions that are necessary for its core functioning. They are:
- `BEG(name)`: marks a start point, from which a process can start running the program.
- `END()`: terminates the current program.
- `LAB(name)`: marks a jump target, to which goto statements can jump.
- `GTO(target)`: a goto statement. It moves execution to the label with the specified name.
- `WRN(message)`: prints a warning message.
- `ERR(message)`: prints an error message and terminates the program.

## Instruction Definitions
Instruction definitions contain various properties. We group them into three categories:
- Runtime data: the opcode, parameters and implementation.
- Metadata: a display name, description, category and icon.
- Editor data: contain data only relevant to the editor module: editor node info, preview data and compile rule data.

Opcodes should be unique within the instruction set, and should preferably be as short as possible.

## Parameters
Each parameter contains an ID, display name and description. Some parameter types also contain additional data, such as a default value, a minimum value or a maximum value.

Like with opcodes, each parameter ID should be unique within an instruction definition.

Various types of parameters are supported. These have no in-game meaning, but are used to define how the parameter will be drawn in the editor. The following types of parameters exist:
- bool
- int
- int slider
- float
- float slider
- character
- single-line text
- multi-line text
- color
- output

## Implementations
Each instruction definition has an implementation property. The ISA module uses them to generate GDScript execution handler classes for each instruction definition at runtime.

The implementation is a resource with three text values: `Initialize`, `Execute` and `Members`. Each is expected to be in _**GDScript**_.

When an instruction set is first referenced by the module, it generates execution handler classes for each instruction definition in the set. Every process maintains its own instances of these classes.
They come with two methods that can be implemented by the user:
- `_initialize(process : Process)`: is called by a process when it enters the scene tree. The `Initialize` code is inserted into this method.
- `_execute(arguments : Array[Variant], delta_time : float)`: is called by the process when it encounters an instruction of the matching type. The `Execute` code is inserted into this method.

Lastly, the `Members` property can be used to insert various user-defined class members into the generated class.

### Execution Handler Contents
In addition to the above, the generated classes also come with a few built-in members that can be used:
- Constants:
  - `OPCODE : String`: the opcode of the instruction definition.
  - `PARAMETER_IDS : Array[String]`: the parameter IDs of the instruction definition.
  - `PARAMETER_COUNT : int`: the number of parameters in the instruction definition.
- Variables:
  - `process : Process`: the process that maintains the execution handler.
- Functions:
  - `end()`: notifies the process that it must stop executing its current program. Equivalent to an `END` instruction.
  - `goto(target_label : String)`: notifies the process that it must jump to the target label. Equivalent to a `GTO` instruction.
  - `warning(message : String)`: prints a warning message. Equivalent to an `WRN` instruction.
  - `error(message : String)`: prints an error message, and notifies the process that it must stop executing its curren program. Equivalent to an `ERR` instruction.
  - `get_parameter_id(index : int) -> String`: get the ID of a parameter.
  - `get_parameter_index(id : String) -> int`: get the index of a parameter.
  - `get_register(name : String) -> Register`: get a register with some name (see below). If the register didn't exist yet, it will be created automatically.

### Registers
Execution handlers can write to and read from registers. Each register is a double-ended queue of `Variant` objects, and is accessible from every execution handler. Each register contains the following functions:
  - `push(value : Variant)`: add a value to the register.
  - `pop() -> Variant`: remove the newest value from the register and return it.
  - `dequeue() -> Variant`: remove the oldest value from the register and return it.
  - `top() -> Variant`: return the newest value of the register, without removing it.
  - `back() -> Variant`: return the oldest value of the register, without removing it.

### Special Syntaxes
#### Arguments
Arguments can be accessed with their parameter id, using the syntax `%<parameter_id>%`. For example: `%foo%` is equivalent to `arguments[get_parameter_index("foo")]`.

Make sure to not do this in the `_initialize` function (or any user-defined function that doesn't have access to the arguments)!

#### Registers
As a shorthand for the `get_register` function, you can use the syntax `$<register_name>$`. For example: `$foo$.push("bar")` is equivalent to `get_register("foo").push("bar")`.

The special syntax `$OPCODE$` accesses a register that uses the instruction's opcode as its name.

### Editor-only Instructions
You don't have to define an implementation for an instruction instruction definition, and can instead leave its implementation at the value `null`. If you do so, then the instruction becomes an *editor-only instruction*.

Editor-onlies have no in-game meaning, and are stripped from imported programs by default. You can use them to group various secondary instructions into one editor node.
