using Godot;

public sealed class Event : Node
{
	[Signal]
	delegate void EnergyCollected(int energy);
	[Signal]
	delegate void BoltCollected(int bolt);
	[Signal]
	delegate void InvincibilityCollected();
	[Signal]
	delegate void FireCollected();
	[Signal]
	delegate void PlayerTouched();
	[Signal]
	delegate void DebugState(string state);
	[Signal]
	delegate void CameraView(Rect2 rect);
}
