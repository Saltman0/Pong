using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	[Export]
	private float _speed = 500.0f;
	
	public override void _Ready()
	{
		Random random = new Random();
		float randomX = (float)(random.NextDouble() * 2.0 - 1.0);
		float randomY = (float)(random.NextDouble() * 2.0 - 1.0);
		Velocity = new Vector2(randomX, randomY);
	}

	public override void _PhysicsProcess(double delta)
	{
		KinematicCollision2D collision = MoveAndCollide(Velocity * _speed * (float)delta);

		if (collision != null)
		{
			Velocity = Velocity.Bounce(collision.GetNormal()) * 1.02f;
		}
	}
}
