using Godot;

public class Checkpoint : Area2D
{
    private AnimatedSprite _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public void OnCheckpointBodyEntered(Node body)
    {
        SetCollisionMaskBit((int) Game.PhysicsLayer.Player, false);
		SetCollisionLayerBit((int) Game.PhysicsLayer.Checkpoint, false);
        Disconnect("body_entered", this, "OnCheckpointBodyEntered");
        _sprite.Play("reached");
        _sprite.Connect("animation_finished", this, "OnReachedAnimationFinished");
    }

    public void OnReachedAnimationFinished()
    {
        _sprite.Disconnect("animation_finished", this, "OnReachedAnimationFinished");
        _sprite.Play("activated");
    }
}
