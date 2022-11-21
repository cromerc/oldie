using Godot;
using System;

public class Camera : Camera2D
{
    private Event _event;

    public override void _Ready()
    {
        _event = GetNode<Event>("/root/Event");
    }

    public override void _PhysicsProcess(float delta)
    {
        var rect = GetViewportRect();
        var owner = (Node2D) Owner;
        rect.Position = owner.Position - rect.Size / 2;
        _event.EmitSignal("CameraView", rect);
    }
}
