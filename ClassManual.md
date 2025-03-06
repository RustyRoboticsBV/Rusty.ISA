# Class Manual
This document is intended as an explanation for the various classes in this module.

## Cutscene Players
Cutscene players form the core of the module. They are nodes that can execute cutscene programs.

Cutscene players can be set up to start playing automatically, or only in response to being manually activated.

## Cutscene Actors
Cutscene actors are nodes that represend objects in the scene that are known to the cutscene players.

Actors contain an identifier string that players use to reference the. This string should be unique!

## Cutscene Triggers
A cutscene trigger is a node that can activate a cutscene player.

There are three types of triggers:
- Triggers that activate if an actor enters some area.
- Triggers that activate if the player presses some button.
- Triggers that activate if an actor is inside some area, and the player presses some button.

You can create child classes of the cutscene trigger, in case a more game-specific activation method is needed.

## Cutscene Programs
Cutscene programs represent the actual cutscenes, which can be played by the `CutscenePlayer` node.

They are stored as CSV files with an assembly-like structure: each line represents a single instruction, where the left-most cell contains the instruction's opcode and the other cells contain the arguments (which correspond to the instruction definition's parameters).

Cutscene players start from a `BEG` instruction, and step through the program top-to-bottom, executing each instruction as they encounter them. When an `END` instruction is reached, the player stops.
`GTO` and `LAB` instructions are used to jump to skip parts of the program, or to implement loops.

## Instruction Definitions
You can think of instruction definitions as function declarations for the various instructions that can appear in cutscene programs.

They contain an opcode, parameters and implementation, as well as a bunch of editor-relevant metadata that has no in-game meaning.

## Instruction Sets
[TODO]

## Parameter Definitions
[TODO]

## Instruction Implementations
Each instruction definition has an implementation property. The cutscene module uses them to generate GDScript execution handler classes for each instruction definition at runtime.

The implementation is a resource with three text values: `Initialize`, `Execute` and `Members`. Each is expected to be in _**GDScript**_.

When an instruction set is first referenced by the module, it generates execution handler classes for each instruction definition in the set. Every cutscene player node maintains its own instances of these classes.
They come with two methods that can be implemented by the user:
- `_initialize(player : CutscenePlayer)`: is called by a cutscene player when it enters the scene tree. The `Initialize` code is inserted into this method.
- `_execute(arguments : Array[String], delta_time : float)`: is called by the cutscene player when it encounters an instruction of the matching type. The `Execute` code is inserted into this method.

Lastly, the `Members` property can be used to insert various user-defined class members into the generated class.

### Execution Handler Contents
In addition to their above, the generated classes also come with a few built-in members that can be used:
- Constants:
  - `OPCODE : String`: the opcode of the instruction definition.
  - `PARAMETERS : Array[String]`: the parameter IDs of the instruction definition.
  - `PARAMETER_COUNT : int`: the number of parameters in the instruction definition.
- Variables:
  - `player : CutscenePlayer`: the cutscene player that maintains the execution handler.
- Functions:
  - `end()`: notifies the player that it must stop executing its current cutscene. Equivalent to an `END` instruction.
  - `goto(target : String)`: notifies the player that it must jump to the target label. Equivalent to a `GTO` instruction.
  - `warning(message : String)`: prints a warning message. Equivalent to an `WRN` instruction.
  - `error(message : String)`: prints an error message, and notifies the player that it must stop executing its current cutscene. Equivalent to an `ERR` instruction.
  - `get_parameter_id(index : int) -> String`: get the ID of a parameter.
  - `get_parameter_index(id : String) -> int`: get the index of a parameter.
  - `get_register(name : String) -> Register`: get a register with some name (see below). If the register didn't exist yet, it will be created automatically.

### Registers
Execution handlers can write to and read from registers. Each register is a double-ended queue of `Variant` objects, and is accessible from every execution handler. Each register contains the following functions:
  - `push(value : String)`: add a value to the register.
  - `pop(value : String) -> String`: remove the newest value from the register and return it.
  - `dequeue(value : String) -> String`: remove the oldest value from the register and return it.
  - `front() -> String`: return the newest value of the register, without removing it.
  - `back() -> String`: return the oldest value of the register, without removing it.

### Special Syntaxes
#### Arguments
Arguments can be accessed with their parameter id, using the syntax `%<parameter_id>%`. For example: `%foo%` is equivalent to `arguments[get_parameter_index("foo")]`.
<br/> Make sure to not do this in the `_initialize` function (or any user-defined function that doesn't have access to the arguments)!

#### Registers
As a shorthand for the `get_register` function, you can use the syntax `$<register_name>$`. For example: `$foo$.push("bar")` is equivalent to `get_register("foo").push("bar")`.
<br/>The special syntax `$OPCODE$` accesses a register that uses the instruction's opcode as its name.

### Editor-only Instructions
You don't have to define an implementation for an instruction instruction definition, and instead leave its implementation at the value `null`. If you do so, then the instruction becomes an *editor-only instruction*.

Editor-onlies have no in-game meaning, and can be used to group various pre-instructions inside of a node. For example, you could have a `TextSequence` instruction that contains a list of `DialogText` instructions, allowing the user to create blocks of text without needing to give each line of dialog its own node.

Editor-onlies are stripped from imported programs by default.

## Editor Node Info
[TODO]

## Pre-Instructions
[TODO]
