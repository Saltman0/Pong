using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void ReturnToMainMenuEventHandler();
	
	private const int DefaultScore = 0;

	private const int DefaultTimeLeft = 60;

	public bool IsMultiplayer = false;
	
	private int _timeLeft = DefaultTimeLeft;
	
	private int _leftScore = DefaultScore;
	
	private int _rightScore = DefaultScore;
	
	private GameInterface _gameInterface;
	
	private Paddle _paddleLeft;
	
	private Paddle _paddleRight;
	
	private Ball _ball;

	private Marker2D _markerPaddleLeft;
	
	private Marker2D _markerPaddleRight;
	
	private Marker2D _markerBall;
	
	public override void _Ready()
	{
		_gameInterface = GetNode<GameInterface>("GameInterface");
		_paddleLeft = GetNode<Paddle>("PaddleLeft");
		_paddleRight = GetNode<Paddle>("PaddleRight");
		_ball = GetNode<Ball>("Ball");
		_markerPaddleLeft = GetNode<Marker2D>("MarkerPaddleLeft");
		_markerPaddleRight = GetNode<Marker2D>("MarkerPaddleRight");
		_markerBall = GetNode<Marker2D>("MarkerBall");
		
		GetNode<Goal>("GoalLeft").GoalScored += OnGoalScored;
		GetNode<Goal>("GoalRight").GoalScored += OnGoalScored;
		GetNode<Timer>("Timer").Timeout += OnTimerTimeout;
		
		_gameInterface.UpdateScore(_leftScore, "left");
		_gameInterface.UpdateScore(_rightScore, "right");
		_gameInterface.UpdateTimeLeft(_timeLeft);
		
		_gameInterface.GameUnpaused += () => { GetTree().Paused = false; };
		_gameInterface.MatchReplayed += OnMatchReplayed;
		
		// We send a signal to the main scene (Root) in the goal to intercept the signal
		// so we can return in the main menu of the game
		_gameInterface.ReturnToMainMenuRequested += () => { EmitSignalReturnToMainMenu(); };
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			PauseGame();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		_paddleLeft.Direction = Input.GetAxis("move_up", "move_down");
		
		if (IsMultiplayer)
		{
			_paddleRight.Direction = Input.GetAxis("move_up_2", "move_down_2");
		}
		else
		{
			FollowBall(_paddleRight);
		}
	}

	private void OnGoalScored(string side)
	{
		if (side == "left")
		{
			_rightScore++;
			_gameInterface.UpdateScore(_rightScore, "right");
		}
		
		if (side == "right")
		{
			_leftScore++;
			_gameInterface.UpdateScore(_leftScore, "left");
		}
		
		ResetPositions();
	}

	private void OnTimerTimeout()
	{
		_timeLeft--;

		string winner;
		if (_timeLeft == 0)
		{ 
			if (_leftScore > _rightScore)
			{
				winner = "left";
			} else if (_rightScore > _leftScore) {
				winner = "right";
			} else {
				winner = "none";
			}
			_gameInterface.DisplayGameOverContainer(winner);
		} else {
			_gameInterface.UpdateTimeLeft(_timeLeft);
		}
	}
	
	private void ResetPositions()
	{
		_paddleLeft.Position = GetNode<Marker2D>("MarkerPaddleLeft").Position;
		_paddleRight.Position = GetNode<Marker2D>("MarkerPaddleRight").Position;
		_ball.Position = GetNode<Marker2D>("MarkerBall").Position;
		_ball.Launch();
	}

	private void OnMatchReplayed()
	{
		_timeLeft = DefaultTimeLeft;
		_leftScore = DefaultScore;
		_rightScore = DefaultScore;
		_paddleLeft.Position = GetNode<Marker2D>("MarkerPaddleLeft").Position;
		_paddleRight.Position = GetNode<Marker2D>("MarkerPaddleRight").Position;
		_ball.Position = GetNode<Marker2D>("MarkerBall").Position;
		_ball.Launch();
	}

	private void FollowBall(Paddle paddle)
	{
		float diff = _ball.Position.Y - paddle.Position.Y;
		float deadzone = 5.0f;

		paddle.Direction = Math.Abs(diff) > deadzone ? Math.Sign(diff) : 0.0f;
	}
	
	private void PauseGame()
	{
		_gameInterface.DisplayPauseContainer();
		GetTree().Paused = true;
	}
}
