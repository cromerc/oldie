using Godot;
using System;

public class Event : Node
{
	[Signal]
	delegate void GameStarted(string example);

	public override void _Ready()
	{
		var events = (Event) GetNode("/root/Event");
		events.Connect("GameStarted", this, "MyCallBack");
		
		events.EmitSignal("GameStarted", " my extra string");
	}

	public void MyCallBack(string example) {
		GD.Print("signal worked" + example);
	}
}
