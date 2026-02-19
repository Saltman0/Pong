using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	[Export]
	private float _speed = 500.0f;
	
	public override void _Ready()
	{
		Launch();
	}

	public override void _PhysicsProcess(double delta)
	{
		KinematicCollision2D collision = MoveAndCollide(Velocity * _speed * (float)delta);

		if (collision != null)
		{
			Velocity = Velocity.Bounce(collision.GetNormal()) * 1.02f;
		}
	}

	public void Launch()
	{
		Velocity = new Vector2(1.0f, 0.0f);
	}
}
