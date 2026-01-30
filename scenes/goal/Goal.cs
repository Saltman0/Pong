using Godot;
using System;

public partial class Goal : Area2D
{
	[Signal]
	public delegate void GoalScoredEventHandler(Goal goal);
	
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
		EmitSignal(SignalName.GoalScored, this);
	}
}
