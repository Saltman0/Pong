using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void ScoreUpdatedEventHandler(int newScore, string side);
	
	[Signal]
	public delegate void TimeUpdatedEventHandler(int seconds);
	
	[Signal]
	public delegate void GameIsOverEventHandler(string winner);
	
	[Signal]
	public delegate void GameIsPausedEventHandler();
	
	[Signal]
	public delegate void ReturnToMainMenuEventHandler();
	
	private const int DefaultScore = 0;

	private const int DefaultTimeLeft = 60;

	public bool IsMultiplayer = false;
	
	private int _timeLeft = DefaultTimeLeft;
	
	private int _leftScore = DefaultScore;
	
	private int _rightScore = DefaultScore;
	
	private Paddle _paddleLeft;
	
	private Paddle _paddleRight;
	
	private Ball _ball;
	
	public override void _Ready()
	{
		_paddleLeft = GetNode<Paddle>("PaddleLeft");
		_paddleRight = GetNode<Paddle>("PaddleRight");
		_ball = GetNode<Ball>("Ball");
		
		GetNode<Goal>("GoalLeft").GoalScored += OnGoalScored;
		GetNode<Goal>("GoalRight").GoalScored += OnGoalScored;
		GetNode<Timer>("Timer").Timeout += OnTimerTimeout;
		
		GameInterface gameInterface = GetNode<GameInterface>("GameInterface");
		ScoreUpdated += gameInterface.OnScoreUpdated;
		TimeUpdated += gameInterface.OnTimeUpdated;
		GameIsOver += gameInterface.OnGameOver;
		GameIsPaused += gameInterface.OnGamePaused;
		gameInterface.UnpauseGame += () => { GetTree().Paused = false; };
		gameInterface.ReplayMatch += () => { ResetRound(); };
		
		// We send a signal to the main scene (Root) in the goal to intercept the signal
		// so we can return in the main menu of the game
		gameInterface.ReturnToMainMenu += () => { EmitSignalReturnToMainMenu(); };
		
		EmitSignalScoreUpdated(_leftScore, "left");
		EmitSignalScoreUpdated(_rightScore, "right");
		EmitSignalTimeUpdated(_timeLeft);
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
		Paddle paddleLeft = GetNode<Paddle>("PaddleLeft");
		Paddle paddleRight = GetNode<Paddle>("PaddleRight");
		
		paddleLeft.Direction = Input.GetAxis("move_up", "move_down");
		
		if (IsMultiplayer)
		{
			paddleRight.Direction = Input.GetAxis("move_up_2", "move_down_2");
		}
		else
		{
			FollowBall(paddleRight);
		}
	}

	private void OnGoalScored(string side)
	{
		if (side == "left")
		{
			_rightScore++;
			EmitSignalScoreUpdated(_rightScore, "right");
		} 
		
		if (side == "right")
		{
			_leftScore++;
			EmitSignalScoreUpdated(_leftScore, "left");
		}
		
		ResetPositions();
	}

	private void OnTimerTimeout()
	{
		_timeLeft--;
		
		EmitSignalTimeUpdated(_timeLeft);
		
		if (_timeLeft == 0)
		{ 
			if (_leftScore > _rightScore)
			{
				EmitSignalGameIsOver("left");
			} else if (_rightScore > _leftScore) {
				EmitSignalGameIsOver("right");
			} else {
				EmitSignalGameIsOver("none");
			}
		}
	}
	
	private void ResetPositions()
	{
		_paddleLeft.Position = GetNode<Marker2D>("MarkerPaddleLeft").Position;
		_paddleRight.Position = GetNode<Marker2D>("MarkerPaddleRight").Position;
		_ball.Position = GetNode<Marker2D>("MarkerBall").Position;
		_ball.Launch();
	}

	private void ResetRound()
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
		EmitSignalGameIsPaused();
		GetTree().Paused = true;
	}
}
