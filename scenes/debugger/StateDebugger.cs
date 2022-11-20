using Godot;
using System;

public class StateDebugger : Label
{
    private Event _event;

    public override void _Ready()
    {
        _event = GetNode<Event>("/root/Event");
        _event.Connect("DebugState", this, "OnDebugState");
    }

    private void OnDebugState(string state)
    {
        Text = state;
    }
}
