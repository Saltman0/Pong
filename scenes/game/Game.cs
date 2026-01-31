using Godot;
using System;

public partial class Game : Node
{
	private int _leftScore;
	private int _rightScore;
	private Goal _goalLeft;
	private Goal _goalRight;
	private Timer _timer;
	private Control _gameInterface;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_leftScore = 0;
		_rightScore = 0;
		_goalLeft = GetNode<Goal>("GoalLeft");
		_goalRight = GetNode<Goal>("GoalRight");
		_timer = GetNode<Timer>("Timer");

		_goalLeft.GoalScored += OnGoalScored;
		_goalRight.GoalScored += OnGoalScored;
		_timer.Timeout += OnTimerTimeout;
	}
	
	private void OnGoalScored(Goal goal)
	{
		if (goal.Name.Equals("GoalLeft"))
		{
			_leftScore++;
			GetNode<RichTextLabel>("GameInterface/Header/HBoxContainer/ScoreLeft").Text = _leftScore.ToString();
		} else if (goal.Name.Equals("GoalRight"))
		{
			_rightScore++;
			GetNode<RichTextLabel>("GameInterface/Header/HBoxContainer/ScoreRight").Text = _rightScore.ToString();
		}
	}

	private void OnTimerTimeout()
	{
		GD.Print("Fini.");
		// TODO Afficher le nom du vainqueur
		// TODO Proposer de refaire une partie (tout réinitiliaser)
		// TODO Revenir au menu principal
	}
}
