# Class Manual
This document is intended as an explanation for the various classes in this module.

# 1. Nodes
The module comes with three node classes: `CutscenePlayer`, `CutsceneActor` and `CutsceneTrigger`.

## Cutscene Players
Cutscene players form the core of the module. They are nodes that can execute [cutscene program resources](https://github.com/RustyRoboticsBV/Rusty.Cutscenes/blob/master/ClassManual.md#cutscene-programs).

Cutscene players can be set up to either start playing automatically, or in response to being manually activated.
After this, they will continue playing either until they reach an `END` or `ERR` instruction or are manually stopped.

Cutscene players require access to an [instruction set](https://github.com/RustyRoboticsBV/Rusty.Cutscenes/blob/master/ClassManual.md#instruction-sets) to function.

## Cutscene Actors
Cutscene actors are nodes that represent objects in the scene that are known to and can be manipulated by the cutscene players.

Actors have a name string that players use to reference them. All actors in the scene should have an unique name!

## Cutscene Triggers
A cutscene trigger is a node that can activate a cutscene player.

There are three types of triggers:
- Triggers that activate if an actor enters some area.
- Triggers that activate if the player presses some button.
- Triggers that activate if an actor is inside some area, and the player presses some button.

# 2. Resources
## Cutscene Programs
Cutscene programs represent the actual cutscenes, which can be played by the `CutscenePlayer` node.

They are stored as CSV files with an assembly-like structure: each line represents a single instruction, where the left-most cell contains the instruction's opcode and the other cells contain the arguments (which correspond to the instruction definition's parameters).

Cutscene players start from a `BEG` instruction, and step through the program top-to-bottom, executing each instruction as they encounter them. When an `END` instruction is reached, the player stops.
`GTO` and `LAB` instructions are used to jump to skip parts of the program, or to implement loops.

## Instruction Definitions
You can think of instruction definitions as function declarations for the various instructions that can appear in cutscene programs.

They contain an opcode, parameters and implementation, as well as a bunch of editor-relevant metadata that has no in-game meaning.

## Instruction Sets
Instruction sets contain a list of instruction definitions. These can be accessed by their opcodes or their index.
Every instruction in the set should have an unique opcode - if not, then instructions with duplicate opcodes will not be able to be accessed properly.

Instruction sets can be nested - we refer to nested sets as *modules*. From the outside, instructions from modules can be accessed as if they were part of the parent instruction set.

## Parameters
Parameters define the function arguments of an instruction, and how these arguments are drawn in the editor.
In-game, all arguments are variants, but in the editor they can be any of the following types: bool, int, int slider, float, float slider, char, single-line text, multiline text, color and output.

Parameters are referenced by their ID - this is the only field with in-game meaning. They also come with some metadata and an editor default value. Some parameters have additional data, such as a min and max value for sliders.

## Implementations
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
If you want an instruction to be instantiable as an editor node, you must set its editor node info.
It contains some data about how the node should appear in the editor, such as colors and a minimum width. You can also set its menu priority.

Instructions with their editor node info set to `null` can only appear as secondary instructions inside other nodes (see below).

## Compile Rules
Sometimes, you want an editor node to compile to multiple instructions. This is where compile rules come in.
These allow the editor to generate *secondary instructions* whenever it compiles an editor node. There are two types:
- `Pre-Instructions`: are compiled before the main instruction.
- `Post-Instructions`: are compiled after the main instruction.

There are five different type of compile rules that may be used. They come in two categories:
- `InstructionRule`: always generate exactly one copy of an instruction when used.
- Containers: these don't directly compile into instructions, but allow you to create complex structures of compile rules:
	- `OptionRule`: defines that a compile rule is *optional*. The user can enable or disable the rule in the editor.
	- `ChoiceRule`: gives the user a choice between one of several compile rules.
	- `TupleRule`: defines a fixed sequence of compile rules. The elements may be of different types.
	- `ListRule`: defines a list of compile rules. The list is resizable. All elements are of the same type.

So for you could, for example, have a list of 'SetDialog' instructions. Or an optional 'OpenDialogWindow' instruction.
Or a list of tuples, where one of the tuple elements is a choice between three different instructions. The possibilities are endless!

See the [editor manual](TODO) for more info - this module merely contains the *definitions* for these compile rules.
