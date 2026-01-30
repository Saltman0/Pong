using Godot;
using System;

public partial class Game : Node2D
{
	private Goal _goalLeft;
	private Goal _goalRight;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_goalLeft = GetNode<Goal>("GoalLeft");
		_goalRight = GetNode<Goal>("GoalRight");

		_goalLeft.GoalScored += OnGoalScored;
		_goalRight.GoalScored += OnGoalScored;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnGoalScored(Goal goal)
	{
		GD.Print($"Goal scored in: {goal.Name}");
	}
}
