# Rusty.Cutscenes
The core components of a generic, extendable cutscene system for the Godot game engine. It's mostly implemented in C# and partially in GDScript.

The module doesn't do much by itself - it merely provides the essential nodes, resources and other classes.

Related repositories:
- [Rusty.CutsceneSystem](https://github.com/RustyRoboticsBV/Rusty.CutsceneSystem): an augmented version that can actually be used in-game.
- [Rusty.CutsceneEditor](https://github.com/RustyRoboticsBV/Rusty.CutsceneEditor): an editor application.
- [Rusty.CustceneImporter](https://github.com/RustyRoboticsBV/Rusty.CutsceneImporter): a resource importer tool.

## Summary
In this module, cutscenes are conceptualized as *programs* that can be run by a *player*. Each program consists of a list of 'instructions'. Each instruction represents a single action (such as "print a line of dialog" or "move the camera").

Cutscene players make use of an *instruction set*, which contains a list of *instruction defitions*. Each definition contains all the necessary information that a player needs to handle instructions of that type (such as an opcode, parameter info and an implementation).

## Program Structure
Programs are stored in a CSV file format. Their internal structure resembles an assembly program. Each line starts with an opcode, and is followed by zero or more arguments. Opcodes and arguments are separated by commas.

If an opcode or argument contains commas or double quotes, it is surrounded by double quotes. Within a quoted string, two double-quotes in a row are interpreted as a single double-quote character.

Example of a simple cutscene program:

	PrintDialog,"Hello, world!"
	MoveCameraTo,100,102
	End

This program would print the text `Hello world!` to the dialog system, move the camera to coordinates `(100, 102)`, and lastly the end the scene.

## Built-in Instructions
This module only comes with a few built-in instructions that are necessary for the module's core functions. They are:
- `BEG(name)`: marks a start point, from which a cutscene player can start running the program. A cutscene may have multiple start points.
- `GTO(target)`: a goto statement. It moves execution to the label with the specified name.
- `LAB(name)`: marks a jump target, to which goto statements can jump.
- `END`: ends the current cutscene.
- `ERR(message)`: prints an error message and ends the cutscene.

## Instruction Implementations
Each instruction definition has an implementation field. This implementation is a resource with two text values: the `initialize` code and the `execute` code. Both are expected to be in _**GDScript**_.

When an instruction set is first referenced by the module, it generates execution handler classes for each instruction definition in the set. Both implementation strings are inserted in corresponding functions:
- `_initialize(player : CutscenePlayer)`
- `_execute(player : CutscenePlayer, arguments : Array[String], delta_time : float)`

In addition to their function arguments, the generated classes also come with a few features that the implementations may use:
- Constants:
  - `OPCODE : string`: the opcode of the instruction definition.
  - `PARAMETER_COUNT : int`: the number of arguments passed to the `_execute` function.
- Functions:
  - `end()`: notifies the player that it must stop executing its current cutscene. Equivalent to an `END` instruction.
  - `goto(target : String)`: notifies the player that it must jump to the target label. Equivalent to a `GTO` instruction.
  - `error(message : String)`: prints an error message, and notifies the player that it must stop executing its current cutscene. Equivalent to an `ERR` instruction.
  - `get_register(name : String) -> Register`: get a register (see below) with some name. If the register didn't exist yet, it is created automatically.
  - `get_parameter_id(index : int) -> String` get the ID of a parameter.
  - `get_parameter_index(id : String) -> int` get the index of a parameter.
- Registers: execution handlers can write to and read from registers. Each register is a double-ended queue of `Variant` objects, and is accessible from every execution handler. Each register contains the following functions:
  - `push(value : String)`: add a value to the register.
  - `pop(value : String) -> String`: remove the newest value from the register and return it.
  - `dequeue(value : String) -> String`: remove the oldest value from the register and return it.
  - `front() -> String`: return the newest value of the register, without removing it.
  - `back() -> String`: return the oldest value of the register, without removing it.

As a shorthand for the `get_register` function, you can use the syntax `$<register_name>$`. For example: `$foo$.push("bar")` is equivalent to `get_register("foo").push("bar")`.
<br/>The special syntax `$OPCODE$` gets a register that uses the instruction's opcode as its name.

The arguments of the execute function can be accessed by their parameter id, using the syntax `%<parameter_id>%`. For example: `%foo%` is equivalent to `arguments[get_parameter_index("foo")]`.
