using Godot;
using System;

public class Bolts : Label
{
    private Game _game;
    private Event _event;

    public override void _Ready()
    {
        _game = GetNode<Game>("/root/Game");
        _event = GetNode<Event>("/root/Event");
        _event.Connect("BoltCollected", this, "OnBoltCollected");
        Text = "0";
    }

    public void OnBoltCollected(int bolts)
    {
        Text = _game.Bolts.ToString();
    }
}
