using Godot;
using System;
using Rusty.ISA;

[GlobalClass]
public partial class Player : Node
{
    [Export] Process Process;
    [Export] string StartLabel = "A";

    public override void _Ready()
    {
        Process.Play(StartLabel);
    }
}
