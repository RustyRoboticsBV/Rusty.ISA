# Rusty.ISA
A generic *instruction set architecture* module (ISA for short), written for the Godot game engine. It allows you to:
- Define an instruction set.
- Write programs using this instruction set.
- Execute these programs from within a Godot game.

The module is implemented in C#.

Related repos:
- [Editor](https://github.com/RustyRoboticsBV/Rusty.ISA.Editor): A graph-based editor interface. While you can write programs by hand or in the inspector, this is not encouraged.
- [Importer](https://github.com/RustyRoboticsBV/Rusty.ISA.Importer): A set of plugins that can import some of the resources in this module from various file types.

## Terminology
In this module, we define a *process* as a node that can run a *program*, which consist of *instruction instances*. In order to execute instructions, the process uses an *instruction set*, which contains *instruction definitions*. Each definition contains an opcode, parameters and an implementation. The instances contain an opcode and arguments. The opcode is used to match an instance with its definition.

A process can start executing a program at explicitly-defined start points, and stops when it reaches an end point. Programs may have multiple start points. Flow control is done via goto and label instructions.

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

The implementation is split up into three text properties: `Initialize`, `Execute` and `Members`. Each is expected to be in _**GDScript**_.

When a process starts executing a program, it first generates execution handler classes for each instruction definition in its instruction set.
They come with two methods that can be implemented by the user:
- `_initialize(process : Process)`: is called by a process right after it generates the class. The `Initialize` code is inserted into this method.
- `_execute(arguments : Array[Variant], delta_time : float)`: is called by the process when it encounters an instruction of the matching type. The `Execute` code is inserted into this method.

Lastly, the `Members` property can be used to insert various user-defined class members into the generated class.

### Dependencies
The implementation also contains an array of dependency strings. These allow you to define global class names that the instruction needs to exist in order to function. If one or more dependency cannot be found, then the instruction's execution handler cannot be generated.

If a process encounters an instruction with missing dependencies during program execution, it will print an error and terminate.

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

Make sure to NOT do this in the `_initialize` function (or any user-defined function that doesn't have access to the arguments)!

#### Registers
As a shorthand for the `get_register` function, you can use the syntax `$<register_name>$`. For example: `$foo$.push("bar")` is equivalent to `get_register("foo").Push("bar")`.

The register class uses the PascalCase naming convention for its methods, as is standard for C#. However, if you use this syntax, you can instead use the snake_case convention that GDScript uses.

The special syntax `$OPCODE$` accesses a register that uses the instruction's opcode as its name.

## Built-in Instructions
The module only comes with a few built-in instructions that are necessary for its core functioning. They are:
- `BEG(name)`: marks a start point, from which a process can start running the program.
- `END()`: terminates the current program.
- `LAB(name)`: marks a jump target, to which goto statements can jump.
- `GTO(target)`: a goto statement. It moves execution to the label with the specified name.
- `WRN(message)`: prints a warning message.
- `ERR(message)`: prints an error message and terminates the program.


## Serialization
The program, instruction definition and instruction set classes can be serialized and deserialized, using classes from the `Serialization` sub-namespace.

### Programs
Programs follow the CSV file format. Rows are separated by linebreaks and cells are separated by commas. Rows are allowed to have different number of cells.

If a cell contains a comma or double-quote, it must be enclosed with double-quotes. Within enclosed cells, the string `""` is interpreted as a single double-quote character. For example, the cell `"A,""B"""` is interpreted as `A,"B"`.

Furthermore, the following format constraints are in place:
- Every row represents exactly one instruction. This means that cells may not contain line-break character. Instead, line-breaks are represented with the string `\n`.
  - Consequently, the string `\n` is represented as `\\n`.
- The first cell of a row must contain the instruction's opcode. The cells that come after are interpreted as arguments.


So the structure of each program is as follows:

    opcode_1,arg_1,...,arg_n
    ...
    opcode_n,arg_1,...,arg_n

### Instruction Sets
Instruction sets are stored as ZIP files. The file should have the following internal structure:

    ROOT
     ├<category_name_1>
     │ ├<opcode_1>
     │ │ ├Def.xml
     │ │ └Icon.png
     │ ├...
     │ └<opcode_n>
     │   └...
     ├...
     └<category_name_n>
       └...

So the path to a file is always is category/opcode/file. The serializer places instructions without a category in a folder called "uncategorized". A definition is always within the same sub-folders as its icon (if it has one). This sub-folder should contain no other files.

### Instruction Definitions
Instruction definitions are serialized using the XML format. Unless otherwise specified, all XML elements are optional.

    <!-- Root element. Required. The opcode could be unique within the instruction set! -->
    <definition opcode="str">
    
     <!-- The parameters. Each parameter requires an id attribute. Each ID should be unique within this file! -->
     
     <!-- A boolean parameter. -->
     <bool id="str">
      <name>str</name>
      <desc>str</desc>
      <default>false</default>
      <preview>str</preview>
     </bool>
     
     <!-- An integer parameter. -->
     <int id="str">
      <name>str</name>
      <desc>str</desc>
      <default>0</default>
      <preview>str</preview>
     </int>

     <!-- An integer slider parameter. -->
     <islider id="str">
      <name>str</name>
      <desc>str</desc>
      <default>0</default>
      <min>0</min>
      <max>0</max>
      <preview>str</preview>
     </islider>
     
     <!-- A floating-point parameter. -->
     <float id="str">
      <name>str</name>
      <desc>str</desc>
      <default>0.0</default>
      <preview>str</preview>
     </float>

     <!-- A floating-point slider parameter. -->
     <fslider id="str">
      <name>str</name>
      <desc>str</desc>
      <default>0.0</default>
      <min>0.0</min>
      <max>0.0</max>
      <preview>str</preview>
     </fslider>

     <!-- A character parameter. -->
     <char id="str">
      <name>str</name>
      <desc>str</desc>
      <default>c</default>
      <preview>str</preview>
     </char>

     <!-- A single-line text parameter. -->
     <textline id="str">
      <name>str</name>
      <desc>str</desc>
      <default>str</default>
      <preview>str</preview>
     </textline>

     <!-- A multi-line text parameter. -->
     <multiline id="str">
      <name>str</name>
      <desc>str</desc>
      <default>str</default>
      <preview>str</preview>
     </multiline>

     <!-- A color parameter. -->
     <color id="str">
      <name>str</name>
      <desc>str</desc>
      <default>#FFFFFFFF</default>
      <preview>str</preview>
     </color>

     <!-- An output parameter. -->
     <output id="str">
      <name>str</name>
      <desc>str</desc>
      <remove_default>true</remove_default>
      <preview_arg>str</preview_arg>
      <preview>str</preview>
     </output>
     
     
     
     <!-- The implementation. -->
     <implementation>
      <deps>str1,str2,...,strn</deps>
      <members>str</members>
      <init>str</init>
      <exec>str</exec>
     </implementation>
     
     
     
     <!-- The editor node info. -->
     <editor_node>
      <priority>0</priority>
      <min_width>0</min_width>
      <min_height>0</min_height>
      <main_color>#FFFFFF</main_color>
      <text_color>#FFFFFF</text_color>
      <preview>str</preview>
      <word_wrap>true</word_wrap>
     </editor_node>
     
     <!-- The preview code. -->
     <preview>str</preview>
     
     
     
     <!-- The compile rules. Each rule should contain an unique ID. Nested rules only need an ID that is unique within their
          parent rule. -->
     
     <!-- The pre-instruction block. May contain any number of compile rule elements. -->
     <pre>
      
      <!-- An option rule. -->
      <option id="str">
       <name>str</name>
       <desc>str</desc>
       <enabled>true</enabled>
       (nested rule)
       <preview>str</preview>
      </option>
      
      <!-- A choice rule. -->
      <choice id="str">
       <name>str</name>
       <desc>str</desc>
       <selected>0</selected>
       (nested rule(s))
       <preview>str</preview>
      </choice>
      
      <!-- A tuple rule. -->
      <tuple id="str">
       <name>str</name>
       <desc>str</desc>
       (nested rule(s))
       <preview>str</preview>
      </tuple>
      
      <!-- A list rule. -->
      <list id="str">
       <name>str</name>
       <desc>str</desc>
       <button_text>true</button_text>
       (nested rule)
       <preview>str</preview>
      </list>
      
      <!-- An instruction rule. -->
      <instruction id="str">
       <name>str</name>
       <desc>str</desc>
       <opcode>str</opcode>
       <preview>str</preview>
      </instruction>
      
     </pre>
     
     <!-- The post-instruction block. The contents are identical to the pre-instruction block. -->
     <post>

      (rules)

     </post>
     
    </definition>

## Previews
You can control how elements of a node are drawn in the editor using previews. They can be defined for editor nodes, instruction definitions, parameters and compile rules. Previews take the form of GDScript expressions and can refer to the previews of other objects, allowing you to create dynamic behavior for them.

Preview expressions should either return a string, or write to a string variable named `result`. Simple, one-line expressions don't need to do this explicitly.

So the following are all valid preview strings:

    result = "preview";
<br/>

    return "preview";
<br/>

    "preview";

But the following is NOT:

    "preview";
    "preview";

### Special Syntaxes
The following special syntaxes can be used:
- `%<parameter_id>%`: Gets replaced with the value of an argument when used in a parameter preview, and with the preview of a parameter when used anywhere else.
- `[<compile_rule_id>]`: Gets replaced with the preview of a compile rule. You can only reference top-level compile rules with this syntax, not nested compile rules! The exception to this is tuple rules, which can also access their direct children via this syntax. Other compile rules can access the previews of their direct children via the following syntaxes:
  - `[target]`: Can be used in instruction rules to access its instruction's preview.
  - `[option]`: Can be used in option rules to access its child rule's preview. Returns the empty string if the option has been disabled.
  - `[selected]`: Can be used in choice rules to access the preview of the selected choice.
  - `[element n]`: Can be used in list rules to access the preview of the nth element. `n` should be a valid integer literal or integer variable.
  - `[count]`: Can be used in list rule to access the number of elements. Returns an integer, not a string!
- `<base>`: can be used in editor node previews to access the instruction's regular preview.

Be careful to not introduce loops when using compile rule references!

### Notes
- If the editor node preview is left blank, the instruction definition's preview is used instead.
- The previews of output parameters are used as their output slot labels.
- Instruction definition previews are also used as instruction rule previews. Editor node previews are not.
- If an instruction definition does not define a preview, this will result in an empty string.
