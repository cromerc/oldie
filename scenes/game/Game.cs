using Godot;

public sealed class Game : Node
{
    [Export]
    private int _energy = 3;
    [Export]
    private int _bolts = 0;
    public int Energy { get { return _energy; } set { _energy = value; } }
    public int Bolts { get { return _bolts; } set { _bolts = value; } }

    public enum PhysicsLayer : ushort
    {
        Player = 1,
        Platform = 2,
        Enemy = 3,
        Collectable = 4,
        Checkpoint = 5
    }

    public override void _Ready()
    {
        var eventBus = GetNode<Event>("/root/Event");
        eventBus.Connect("EnergyCollected", this, "OnEnergyCollected");
        eventBus.Connect("BoltCollected", this, "OnBoltCollected");
    }

    public void OnEnergyCollected(int energy)
    {
        _energy += energy;
    }

    public void OnBoltCollected(int bolts)
    {
        _bolts += bolts;
    }
}
