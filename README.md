# Rusty.Cutscenes
A C# cutscene module for the Godot game engine. Programs can be created using the [editor app](https://github.com/RustyRoboticsBV/Rusty.CutsceneEditor), which is not part of this module.

It consists of the following components:

## Nodes
### Cutscene Player
TODO

### Cutscene Actor
TODO

### Cutscene Trigger
TODO

## Resources
### Instruction Set
A dictionary of instruction definitions, specifying which instructions may be used in cutscene programs.

### Instruction Definition
The definitions for the entries in the instruction set. Definitions are stored in the XML format, and must start with an *instruction_definition* node. Inside, the following nodes may exist:
- *opcode*: the identifier of the instruction.
- Meta-data:
  - *icon*: the icon of the instruction.
  - *display_name*: the human-readable name of the instruction.
  - *description*: the human-readable description of the instruction.
  - *category*: the category of the instruction.
- Parameters definitions:
  - *bool*: a boolean parameter. 
  - *int*: an integer parameter.
  - *float*: a floating-point parameter.
  - *int_slider*: an integer slider parameter.
  - *float_slider*: an floating-point slider parameter.
  - *char*: a character parameter.
  - *line*: a single-line string parameter.
  - *multiline*: a multi-line string parameter. 
  - *color*: a color parameter.
  - *output*: a node output parameter.
- *editor_node*: if present, then the instruction can be instantiated in the cutscene editor.
 - *code*: the implementation of the instruction (in GDScript).
- Compile rule definitions:
  - *option*: an option compile rule.
  - *choice*: a choice compile rule.
  - *tuple* a tuple compile rule.
  - *list*: a list compile rule.
  - *pre*: an instruction rule.

#### Parameter Definitions
Each parameter definition can have the following nodes:
- *id*: the identifier of the parameter.
- *display_name*: the human-readable name of the parameter.
- *default*: the default value of the parameter.
- *min*: (for sliders only) the minimum value of the parameter.
- *max*: (for sliders only) the maximum value of the parameter.

### Cutscene Program
A program of cutscene instructions, using an assembly-like syntax where each instruction has the form [*Opcode, Arg~1~, Arg~2~, ..., Arg~n~*]. Programs are stored in the CSV file format.
