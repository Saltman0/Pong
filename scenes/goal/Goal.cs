using Godot;
using System;

public partial class Goal : Area2D
{
	[Export] 
	private string _side;
	
	[Signal]
	public delegate void GoalScoredEventHandler(string side);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBallEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBallEntered(Node2D body)
	{
		EmitSignalGoalScored(_side);
	}
}
