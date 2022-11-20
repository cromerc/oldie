using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    [Export]
    private bool _isInvincible { get; set; } = false;
    [Export]
    private bool _canDoubleJump { get; set; } = true;
    [Export]
    private bool _cayoteTime { get; set; } = true;
    [Export]
    private int _maxWalkSpeed { get; set; } = 50;
    [Export]
    private int _maxRunSpeed { get; set; } = 150;
    private Event _eventBus;
    private Vector2 _velocity;
    private AnimatedSprite _sprite;
    private Camera2D _camera;
    private Timer _cayoteTimer;

    private enum State
    {
        Idle,
        Walk,
        Jump,
        DoubleJump,
        Fall,
        Swim
    };

    private Dictionary<State, Action<float>> _states;
    private Stack<State> _stateStack;

    public override void _Ready()
    {
        _eventBus = GetNode<Event>("/root/Event");
        _eventBus.Connect("PlayerTouched", this, "PlayerTouched");
        _velocity = new Vector2();
        _sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        _camera = GetNode<Camera2D>("Camera2D");

        _cayoteTimer = GetNode<Timer>("CayoteTimer");

        _states = new Dictionary<State, Action<float>>();
        _states.Add(State.Idle, Idle);
        _states.Add(State.Walk, Walk);
        _states.Add(State.Fall, Fall);
        _states.Add(State.Jump, Jump);
        _states.Add(State.DoubleJump, Jump);

        _stateStack = new Stack<State>();
        _stateStack.Push(State.Idle);
    }

    public override void _PhysicsProcess(float delta)
    {
        _states[_stateStack.Peek()].Invoke(delta);

        var maxSpeed = (Input.IsActionPressed("run")) ? _maxRunSpeed : _maxWalkSpeed;

        _velocity.x = Mathf.Clamp(_velocity.x, -_maxRunSpeed, _maxRunSpeed);

        if (_velocity.x > maxSpeed)
        {
            _velocity.x = Mathf.Lerp(maxSpeed, _velocity.x, 0.1f);
        }

        if (_velocity.x < -maxSpeed)
        {
            _velocity.x = Mathf.Lerp(-maxSpeed, _velocity.x, 0.1f);
        }

        _velocity = MoveAndSlide(_velocity, Vector2.Up);

        // Make sure the player is drawn in pixel perfect spacing
        Mathf.Round(Position.x);
        Mathf.Round(Position.y);

        var count = GetSlideCount();
        for (var i = 0; i < count; i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            var collider = (Node) collision.Collider;
            if (collider.IsInGroup("Enemy"))
            {
                _eventBus.EmitSignal("PlayerTouched");
            }
            collision.Dispose();
        }
    }

    private void ApplyGravity(float delta)
    {
        _velocity.y += (int) ProjectSettings.GetSetting("physics/2d/default_gravity") * delta * 5;
    }

    private void EnterState(State state)
    {
        _eventBus.EmitSignal("DebugState", state.ToString());
        _stateStack.Push(state);
        ChangeStateMechanics();
        ChangeStateAnimation();
    }

    private void ExitState()
    {
        _stateStack.Pop();
        _eventBus.EmitSignal("DebugState", _stateStack.Peek().ToString());
        ChangeStateAnimation();
    }

    private void ChangeStateMechanics()
    {
        switch (_stateStack.Peek())
        {
            case State.Jump:
                _velocity.y = -250;
                break;
            case State.DoubleJump:
                _velocity.y = -260;
                break;
            default:
                _cayoteTimer.Stop();
                break;
        }
    }

    private void ChangeStateAnimation()
    {
        switch (_stateStack.Peek())
        {
            case State.Idle:
                _sprite.Play("idle");
                break;
            case State.Walk:
                _sprite.Play("walk");
                break;
            case State.Fall:
                _sprite.Play("jump");
                break;
            case State.Jump:
                _sprite.Play("jump");
                break;
        }
    }

    private void Idle(float delta)
    {
        ApplyGravity(delta);

        _velocity.x = Mathf.Lerp(_velocity.x, 0, 0.5f);

        if (Input.IsActionPressed("right") || Input.IsActionPressed("left"))
        {
            EnterState(State.Walk);
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            EnterState(State.Jump);
        }

        if (!IsOnFloor())
        {
            EnterState(State.Fall);
            return;
        }
    }

    private void Walk(float delta)
    {
        ApplyGravity(delta);

        if (Input.IsActionPressed("right"))
        {
            _velocity.x += (_velocity.x < 0) ? 9 : 3;

            // Ice:
            //_velocity.x += 3;

            _sprite.FlipH = false;
        }
        else if (Input.IsActionPressed("left"))
        {
            _velocity.x -= (_velocity.x > 0) ? 9 : 3;

            // Ice:
            //_velocity.x -= 3;

            _sprite.FlipH = true;
        }
        else
        {
            ExitState();
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            EnterState(State.Jump);
            return;
        }

        if (!IsOnFloor())
        {
            if (!_cayoteTime)
            {
                EnterState(State.Fall);
            }
            else
            {
                if (_cayoteTimer.TimeLeft == 0)
                {
                    _cayoteTimer.Start();
                }
            }
            return;
        }
    }

    private void Fall(float delta)
    {
        JumpMove(delta);

        if (IsOnFloor())
        {
            ExitState();
            return;
        }
    }

    private void Jump(float delta)
    {
        JumpMove(delta);

        if (_canDoubleJump && _stateStack.Peek() != State.DoubleJump && Input.IsActionJustPressed("jump"))
        {
            EnterState(State.DoubleJump);
            return;
        }

        if (IsOnFloor())
        {
            ExitState();
            return;
        }
    }

    private void JumpMove(float delta)
    {
        ApplyGravity(delta);

        if (Input.IsActionJustReleased("jump"))
        {
            if (_velocity.y < -100)
            {
                _velocity.y = -100;
                return;
            }
        }

        if (Input.IsActionPressed("right"))
        {
            _velocity.x += (_velocity.x >= 20) ? 2 : 5;
            _sprite.FlipH = false;
        }
        else if (Input.IsActionPressed("left"))
        {
            _velocity.x -= (_velocity.x <= -20) ? 2 : 5;
            _sprite.FlipH = true;
        }
    }

    public void OnCayoteTimerTimeout()
    {
        EnterState(State.Fall);
    }

    public void PlayerTouched()
    {
        GD.Print("Touched");
    }
}
