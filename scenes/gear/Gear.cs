using Godot;
using System;

public class Gear : Area2D
{
	public override void _Process(float delta)
	{
		Sprite sprite = GetNode<Sprite>("Sprite");
		sprite.Rotate(Mathf.Deg2Rad(2));
	}
}
