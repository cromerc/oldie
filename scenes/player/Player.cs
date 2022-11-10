using Godot;

public class Player : KinematicBody2D
{
    [Export]
    private bool _invincible { get; set; }
    private Event _eventBus;
    private Vector2 _velocity;

    public override void _Ready()
    {
        _eventBus = GetNode<Event>("/root/Event");
        _eventBus.Connect("PlayerTouched", this, "PlayerTouched");
        _invincible = false;
        _velocity = new Vector2();
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity.x = 0;

        if (Input.IsActionPressed("right"))
        {
            _velocity.x += 50;
        }

        if (Input.IsActionPressed("left")) {
            _velocity.x -= 50;
        }

        _velocity = MoveAndSlide(_velocity, Vector2.Up);

        var count = GetSlideCount();
        for (var i = 0; i < count; i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            var collider = (Node) collision.Collider;
            if (collider.IsInGroup("Enemy"))
            {
                _eventBus.EmitSignal("PlayerTouched");
            }
        }
    }

    private void PlayerTouched() {
        GD.Print("Touched");
    }
}
