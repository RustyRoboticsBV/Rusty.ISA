# Rusty.ISA
A generic *instruction set architecture* module (ISA for short), written for the Godot game engine in C#. It allows you to:
- Define an instruction set.
- Write programs using this instruction set.
- Execute these programs from within a Godot game.

See [here](https://github.com/RustyRoboticsBV/Rusty.ISA.Editor) for a graph-based editor.

## Terminology
In this module, we define a *process* as a node that can run a *program*, which consist of *instruction instances*. In order to execute instructions, the process uses an *instruction set*, which contains *instruction definitions*. Each definition contains an opcode, parameters and an implementation (in GDScript). The instances contain an opcode and arguments (one per parameter). The opcode is used to match an instance with its definition.

A process can start executing a program at explicitly-defined start points, and stops when it reaches an end point. Programs may have multiple start points. Flow control is done via goto and label instructions.

## Built-in Instructions
The module only comes with a few built-in instructions, which are necessary for its core functioning. They are:
- `BEG(name)`: marks a start point, from which a process can start running the program.
- `END()`: terminates the current program.
- `LAB(name)`: marks a jump target, to which goto statements can jump.
- `GTO(target)`: a goto statement. It moves execution to the label with the specified name.
- `LOG(message)`: prints a message to the console.
- `WRN(message)`: prints a warning message.
- `ERR(message)`: prints an error message and terminates the program.
- `LAN(id)`: defines a language for localization purposes.
- `LOC(parameter, language, text)`: attaches localization data to a parameter of the next instruction.
