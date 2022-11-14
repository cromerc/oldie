using Godot;
using System;

public class Energy : Area2D
{
	private Event _eventBus;

	public override void _Ready()
	{
		_eventBus = GetNode<Event>("/root/Event");
		GetNode<AnimatedSprite>("AnimatedSprite").Play();
	}

	public void OnEnergyBodyEntered(Node body)
	{
		Disconnect("body_entered", this, "OnEnergyBodyEntered");
		SetCollisionMaskBit((int) Game.PhysicsLayer.Player, false);
		SetCollisionLayerBit((int) Game.PhysicsLayer.Collectable, false);
		if (String.Equals(body.Name, "Player"))
		{
			_eventBus.EmitSignal("EnergyCollected", 1);
		}
		QueueFree();
	}
}
