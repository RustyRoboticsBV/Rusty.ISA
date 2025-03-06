﻿using Godot;
using System.Collections.Generic;

namespace Rusty.Cutscenes
{
    /// <summary>
    /// A node that can play cutscene programs.
    /// </summary>
    [GlobalClass]
    public partial class CutscenePlayer : Node
    {
        /* Public properties. */
        [Export] public InstructionSet InstructionSet { get; set; }
        [Export] public CutsceneProgram Program { get; set; }

        public bool IsPlaying => Track != null;
        public int ProgramCounter => Track.ProgramCounter;

        /* Private properties. */
        private CutsceneTrack Track { get; set; }
        private Dictionary<string, Node> ExecutionHandlers { get; set; }

        /* Public methods. */
        public void Play()
        {
            Track = new(Program);
        }

        public void Play(string startPointName)
        {
            Play();
            Track.Jump(startPointName);
        }

        public void Stop()
        {
            Track = null;
        }

        public void Jump(string targetLabel)
        {
            if (Track != null)
                Track.Jump(targetLabel);
        }

        /* Godot overrides. */
        public override void _EnterTree()
        {
            Play();
        }

        public override void _Process(double delta)
        {
            if (Track != null)
            {
                GD.Print(Track.ProgramCounter);
                if (Track.IsOutOfBounds)
                {
                    Stop();
                    return;
                }
                else
                {
                    Execute(Track.Current, delta);
                    Track?.Advance();
                }
            }
        }

        /* Private methods. */
        private void Execute(InstructionInstance instruction, double deltaTime)
        {
            EnsureExecutionHandlers();
            if (ExecutionHandlers.ContainsKey(instruction.Opcode))
                ExecutionHandlers[instruction.Opcode].Call("_execute", instruction.Arguments, deltaTime);
        }

        private void EnsureExecutionHandlers()
        {
            if (ExecutionHandlers != null)
                return;

            ExecutionHandlers = new();
            for (int i = 0; i < InstructionSet.Count; i++)
            {
                InstructionDefinition definition = InstructionSet[i];
                if (definition.Implementation != null)
                {
                    Node handler = new ExecutionHandler(definition).GetNode();
                    handler.Call("_initialize", this);
                    ExecutionHandlers.Add(definition.Opcode, handler);
                }
            }
        }
    }
}