using Godot;
using System.Collections.Generic;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A cutscene track. Handles the flow-control of a cutscene player.
    /// </summary>
    public class CutsceneTrack
    {
        /* Public properties. */
        public CutsceneProgram Program { get; private set; }
        public int ProgramCounter { get; private set; }

        public bool IsOutOfBounds => ProgramCounter < 0 || ProgramCounter >= Program.Length;
        public InstructionInstance Current => Program[ProgramCounter];

        /* Private properties. */
        private Dictionary<string, int> StartLookup { get; set; }
        private Dictionary<string, int> LabelLookup { get; set; }

        /* Constructors. */
        public CutsceneTrack(CutsceneProgram program)
        {
            Program = program;
        }

        /* Public methods. */
        public void Start(string startPoint)
        {
            EnsureStartLookup();

            try
            {
                ProgramCounter = StartLookup[startPoint];
            }
            catch
            {
                GD.PrintErr($"Could not find start point '{startPoint}' in cutscene program '{Program.ResourceName}'.");
            }
        }

        public void Advance()
        {
            ProgramCounter++;
        }

        public void Jump(int target)
        {
            ProgramCounter = target;
        }

        public void Jump(string label)
        {
            EnsureLabelLookup();
            try
            {
                Jump(LabelLookup[label]);
            }
            catch
            {
                GD.PrintErr($"Could not find label '{label}' in cutscene program '{Program.Name}'.");
            }
        }

        public void Execute()
        {
            InstructionInstance instance = Program[ProgramCounter];
        }

        /* Private methods. */
        private void EnsureStartLookup()
        {
            if (StartLookup != null)
                return;

            StartLookup = new();
            for (int i = 0; i < Program.Length; i++)
            {
                if (Program[i].Opcode == "BEG")
                {
                    string name = Program[i].Arguments[0];
                    if (!StartLookup.TryAdd(name, i))
                        GD.PrintErr($"Duplicate start point with name '{name}' detected in cutscene program '{Program.ResourceName}'!");
                }
            }
        }

        private void EnsureLabelLookup()
        {
            if (LabelLookup != null)
                return;

            LabelLookup = new();
            for (int i = 0; i < Program.Length; i++)
            {
                if (Program[i].Opcode == "LAB")
                {
                    string name = Program[i].Arguments[0];
                    if (!LabelLookup.TryAdd(name, i))
                        GD.PrintErr($"Duplicate label with name '{name}' detected in cutscene program '{Program.Name}'!");
                }
            }
        }
    }
}