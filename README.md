# Rusty.Cutscenes
A C#-based cutscene module for the Godot game engine. It contains all the core resources and scene nodes.

The core component is the *CutscenePlayer* node, which can play *CutsceneProgram* resources. These programs have an assembly-like syntax, where each line represents a single cutscene instruction (for example, moving a character or printing text to a dialog window). The players use an *InstructionSet* resource, which contains a list of *InstructionDefinition* resources. Each definition contains the opcode, parameters and implementation of the instruction, as well as some metadata and editor-only data.

Also included are the *CutsceneTrigger* and *CutsceneActor* nodes. The trigger node can be used to start a cutscene player whenever something enters the area of some collider, and the actor node represents any node that can be targeted by a cutscene.

Instruction definitions and cutscene programs can be created using the [editor app](https://github.com/RustyRoboticsBV/Rusty.CutsceneEditor), which is not part of this module. The importer plugins for the resources can be found in the [importer module](https://github.com/RustyRoboticsBV/Rusty.CutsceneImporter), which was separated for the sake of convenience when using git submodules.
