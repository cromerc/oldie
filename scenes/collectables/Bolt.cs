using Godot;
using System;

public class Bolt : Area2D
{
    private Event _eventBus;

    public override void _Ready()
    {
        _eventBus = GetNode<Event>("/root/Event");
		GetNode<AnimatedSprite>("AnimatedSprite").Play();
    }

	public void OnBoltBodyEntered(Node body)
	{
		Disconnect("body_entered", this, "OnBoltBodyEntered");
		SetCollisionMaskBit((int) Game.PhysicsLayer.Player, false);
		SetCollisionLayerBit((int) Game.PhysicsLayer.Collectable, false);
		if (String.Equals(body.Name, "Player"))
		{
			_eventBus.EmitSignal("BoltCollected", 1);
		}
		QueueFree();
	}
}
