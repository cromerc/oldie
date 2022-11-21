using Godot;
using System;

public class Enemy : KinematicBody2D
{
    [Export]
    private Vector2 _startPosition;
    public Vector2 StartPosition { get { return _startPosition; } }

    public override void _Ready()
    {
        _startPosition = Position;
    }

    public virtual void Reset()
    {
        Position = _startPosition;
    }
}
