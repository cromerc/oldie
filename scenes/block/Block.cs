using Godot;
using System;

public class Block : StaticBody2D
{
    private AnimatedSprite _sprite;
    private AnimationPlayer _animation;
    private Area2D _bolt;
    private AnimationPlayer _boltAnimation;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _animation = GetNode<AnimationPlayer>("AnimationPlayer");
        _bolt = GetNode<Area2D>("Bolt");
        _boltAnimation = GetNode<AnimationPlayer>("Bolt/AnimationPlayer");
    }

    public void Hit()
    {
        if (_animation.HasAnimation("hit"))
        {
            _animation.Play("hit");
            _boltAnimation.Play("bounce");
        }
    }

    public void OnAnimationPlayerAnimationFinished(string AnimName)
    {
        GD.Print(AnimName);
        if (AnimName.Equals("hit"))
        {
            _sprite.Play("opened");
            return;
        }

        if (AnimName.Equals("bounce"))
        {
            if (_bolt.HasMethod("Collected"))
            {
                _bolt.CallDeferred("Collected");
            }
            else
            {
                _bolt.QueueFree();
            }
            return;
        }
    }
}
