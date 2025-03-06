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
- `END`: ends the current cutscene.
- `LAB(name)`: marks a jump target, to which goto statements can jump.
- `GTO(target)`: a goto statement. It moves execution to the label with the specified name.
- `WRN(message)`: prints an warning message.
- `ERR(message)`: prints an error message and ends the cutscene.

## Class Manual
See the [class manual document](https://github.com/RustyRoboticsBV/Rusty.Cutscenes/blob/master/ClassManual.md) for a more detailed, conceptual-level description of the important classes of this module.
