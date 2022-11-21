using Godot;
using System;

public class Enemy : KinematicBody2D
{
    [Export]
    private Vector2 _startPosition;
    public Vector2 StartPosition { get { return _startPosition; } }
    private Event _event;

    public override void _Ready()
    {
        _event = GetNode<Event>("/root/Event");
        _startPosition = Position;
    }

    public virtual void Reset()
    {
        // Shove him off screen somewhere until we can respawn him
        Position = new Vector2(-100, - 100);
        _event.Connect("CameraView", this, "OnCameraView");
    }

    public void OnCameraView(Rect2 rect)
    {
        rect.Size += new Vector2(32, 32);
        if (!rect.HasPoint(StartPosition))
        {
            // We can respawn him because the player is not in the spawn point
            _event.Disconnect("CameraView", this, "OnCameraView");
            Position = StartPosition;
        }
    }
}
