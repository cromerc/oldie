using Godot;
using System;

public class EnemyWalking : Enemy
{
    [Export]
    private int _gravity = (int) ProjectSettings.GetSetting("physics/2d/default_gravity");
    private Vector2 _velocity;

    public override void _Ready()
    {
        base._Ready();
        _velocity = new Vector2();
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity.y += (int) _gravity * delta * 5;
        _velocity.x = 25 * -1;
        _velocity = MoveAndSlide(_velocity, Vector2.Up);

        Mathf.Round(Position.x);
        Mathf.Round(Position.y);
    }

    public override void Reset()
    {
        base.Reset();
        _velocity = new Vector2();
    }
}
