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
	public bool IsMultiplayer;
	
	[Export]
	public int TimeLeft;
	
	[Export]
	public int LeftScore;
	
	[Export]
	public int RightScore;
	
	private Paddle _paddleLeft;
	
	private Paddle _paddleRight;
	
	private Ball _ball;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_paddleLeft = GetNode<Paddle>("PaddleLeft");
		_paddleRight = GetNode<Paddle>("PaddleRight");
		_ball = GetNode<Ball>("Ball");
		
		GetNode<Goal>("GoalLeft").GoalScored += OnGoalScored;
		GetNode<Goal>("GoalRight").GoalScored += OnGoalScored;
		GetNode<Timer>("Timer").Timeout += OnTimerTimeout;
		ScoreUpdated += GetNode<GameInterface>("GameInterface").OnScoreUpdated;
		TimeUpdated += GetNode<GameInterface>("GameInterface").OnTimeUpdated;
		GameOver += GetNode<GameInterface>("GameInterface").OnGameOver;

		EmitSignalScoreUpdated(LeftScore, "left");
		EmitSignalScoreUpdated(RightScore, "right");
		EmitSignalTimeUpdated(TimeLeft);
	}

	public override void _PhysicsProcess(double delta)
	{
		Paddle paddleLeft = GetNode<Paddle>("PaddleLeft");
		Paddle paddleRight = GetNode<Paddle>("PaddleRight");
		
		paddleLeft.Direction = Input.GetAxis("move_up", "move_down");
		
		if (IsMultiplayer)
		{
			paddleRight.Direction = Input.GetAxis("move_up", "move_down");
		}
		else
		{
			FollowBall(paddleRight);
		}
	}

	private void OnGoalScored(Goal goal)
	{
		if (goal.Name.Equals("GoalLeft"))
		{
			RightScore++;
			EmitSignalScoreUpdated(RightScore, "right");
		} else if (goal.Name.Equals("GoalRight"))
		{
			LeftScore++;
			EmitSignalScoreUpdated(LeftScore, "left");
		}
		
		ResetRound();
	}

	private void OnTimerTimeout()
	{
		TimeLeft--;
		EmitSignalTimeUpdated(TimeLeft);
		if (TimeLeft == 0)
		{ 
			if (LeftScore > RightScore)
			{
				EmitSignalGameOver("left");
			} else if (RightScore > LeftScore) {
				EmitSignalGameOver("right");
			} else {
				EmitSignalGameOver("none");
			}
		}
	}

	private void ResetRound()
	{
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
}
