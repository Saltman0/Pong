using Godot;
using System;

public partial class Paddle : CharacterBody2D
{
	[Export]
	public float Speed = 400.0f;
	
	[Export] 
	public Color Color;

	public float Direction;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Polygon2D>("Polygon2D").Color = Color;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Velocity = new Vector2(Velocity.X, Direction * Speed);

		MoveAndSlide();
	}
}
