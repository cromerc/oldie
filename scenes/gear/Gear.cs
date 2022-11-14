using Godot;

public class Gear : Area2D
{
	private Sprite _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite>("Sprite");
	}

	public override void _Process(float delta)
	{
		_sprite.Rotate(Mathf.Deg2Rad(2));
	}
}
