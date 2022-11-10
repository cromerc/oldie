using Godot;

public sealed class Game : Node
{
    [Export]
    private int _energy { get; set; } = 3;
    [Export]
    private int _bolts { get; set; } = 0;

    public enum PhysicsLayer : ushort {
        Player = 1,
        Platform = 2,
        Enemy = 3,
        Collectable = 4
    }

    public override void _Ready()
    {
        var eventBus = GetNode<Event>("/root/Event");
        eventBus.Connect("EnergyCollected", this, "OnEnergyCollected");
    }

    private void OnEnergyCollected(int energy)
    {
        _energy += energy;
        if (OS.IsDebugBuild())
        {
            GD.Print("Energy: " + _energy);
        }
    }
}
