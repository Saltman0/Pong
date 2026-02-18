using Godot;
using System;

public partial class Paddle : CharacterBody2D
{
	[Export]
	private float _speed = 400.0f;
	
	[Export] 
	private Color _color;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Polygon2D>("Polygon2D").Color = _color;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Velocity = new Vector2(
			0, 
			Input.GetVector(
				"move_left", 
				"move_right", 
				"move_up", 
				"move_down"
			).Y
		) * _speed;

		MoveAndSlide();
	}
}
