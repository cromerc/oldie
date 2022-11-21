using Godot;
using System;

public class Energy : Area2D
{
	private Event _event;
	private AnimatedSprite _sprite;

	public override void _Ready()
	{
		_event = GetNode<Event>("/root/Event");
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public void OnEnergyBodyEntered(Node body)
	{
		Disconnect("body_entered", this, "OnEnergyBodyEntered");
		SetCollisionMaskBit((int) Game.PhysicsLayer.Player, false);
		SetCollisionLayerBit((int) Game.PhysicsLayer.Collectable, false);
		if (String.Equals(body.Name, "Player"))
		{
			_event.EmitSignal("EnergyCollected", 1);
		}
		QueueFree();
	}

	public void OnVisibilityNotifier2DScreenEntered()
	{
		_sprite.Play();
	}

	public void OnVisibilityNotifier2DScreenExited()
	{
		_sprite.Stop();
	}
}
