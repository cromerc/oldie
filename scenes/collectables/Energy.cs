using Godot;
using System;

public class Energy : Area2D
{
	private Event _event;

	public override void _Ready()
	{
		_event = GetNode<Event>("/root/Event");
		GetNode<AnimatedSprite>("AnimatedSprite").Play();
	}

	public void OnEnergyBodyEntered(Node body)
	{
		Disconnect("body_entered", this, "OnEnergyBodyEntered");
		SetCollisionMaskBit((int) Game.PhysicsLayer.Player, false);
		SetCollisionLayerBit((int) Game.PhysicsLayer.Collectable, false);
		if (body.Name.Equals("Player"))
		{
			_event.EmitSignal("EnergyCollected", 1);
		}
		QueueFree();
	}
}
