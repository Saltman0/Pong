using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void ScoreUpdatedEventHandler(int newScore, string side);
	
	[Signal]
	public delegate void TimeUpdatedEventHandler(int seconds);
	
	[Signal]
	public delegate void GameOverEventHandler(string winner);

	[Export]
	private int _timeLeft;
	[Export]
	private int _leftScore;
	[Export]
	private int _rightScore;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Goal>("GoalLeft").GoalScored += OnGoalScored;
		GetNode<Goal>("GoalRight").GoalScored += OnGoalScored;
		GetNode<Timer>("Timer").Timeout += OnTimerTimeout;
		ScoreUpdated += GetNode<GameInterface>("GameInterface").OnScoreUpdated;
		TimeUpdated += GetNode<GameInterface>("GameInterface").OnTimeUpdated;
		GameOver += GetNode<GameInterface>("GameInterface").OnGameOver;

		EmitSignalScoreUpdated(_leftScore, "left");
		EmitSignalScoreUpdated(_rightScore, "right");
		EmitSignalTimeUpdated(_timeLeft);
	}
	
	private void OnGoalScored(Goal goal)
	{
		if (goal.Name.Equals("GoalLeft"))
		{
			_rightScore++;
			EmitSignalScoreUpdated(_leftScore, "right");
		} else if (goal.Name.Equals("GoalRight"))
		{
			_leftScore++;
			EmitSignalScoreUpdated(_leftScore, "left");
		}
		
	}

	private void OnTimerTimeout()
	{
		_timeLeft--;
		EmitSignalTimeUpdated(_timeLeft);
		if (_timeLeft == 0)
		{ 
			if (_leftScore > _rightScore)
			{
				EmitSignalGameOver("left");
			} else if (_rightScore > _leftScore) {
				EmitSignalGameOver("right");
			} else {
				EmitSignalGameOver("none");
			}
		}
	}
}
