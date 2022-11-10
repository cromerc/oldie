using Godot;
using System;

public class Energy : Area2D
{
	Event eventBus;

	public override void _Ready()
	{
		eventBus = GetNode<Event>("/root/Event");
		GetNode<AnimatedSprite>("AnimatedSprite").Play();
	}

	public void OnEnergyBodyEntered(Node body)
	{
		Disconnect("body_entered", this, "OnEnergyBodyEntered");
		SetCollisionMaskBit((int) Game.PhysicsLayer.Player, false);
		SetCollisionLayerBit((int) Game.PhysicsLayer.Collectable, false);
		if (String.Equals(body.Name, "Player"))
		{
			eventBus.EmitSignal("EnergyCollected", 1);
		}
		QueueFree();
	}
}
