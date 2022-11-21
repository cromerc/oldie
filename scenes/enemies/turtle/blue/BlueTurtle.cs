using Godot;
using System;

public class BlueTurtle : EnemyWalking
{
    private AnimatedSprite _sprite;

    public override void _Ready()
    {
        base._Ready();
        _sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _sprite.Play("walk");
    }

    public void OnVisibilityNotifier2DScreenExited()
    {
        Reset();
    }
}
