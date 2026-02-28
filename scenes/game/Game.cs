using Godot;

public partial class Game : Node
{
	public enum GameMode: byte
	{
		Singleplayer,
		LocalMultiplayer,
		OnlineMultiplayer
	}
	
	[Signal]
	public delegate void ReturnToMainMenuEventHandler();
	
	private const int DefaultScore = 0;

	private const int DefaultTimeLeft = 60;
	
	public GameMode CurrentMode = GameMode.Singleplayer;
	
	public long LeftPlayerId = 1;
	
	public long RightPlayerId = 2;
	
	[Export]
	private int _timeLeft = DefaultTimeLeft;
	
	[Export]
	private int _leftScore = DefaultScore;
	
	[Export]
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
		
		_markerPaddleLeft = GetNode<Marker2D>("MarkerPaddleLeft");
		_markerPaddleRight = GetNode<Marker2D>("MarkerPaddleRight");
		_markerBall = GetNode<Marker2D>("MarkerBall");
		
		_ball = GetNode<Ball>("Ball");
		_paddleLeft = GetNode<Paddle>("PaddleLeft");
		_paddleRight = GetNode<Paddle>("PaddleRight");
		
		SetPaddleProperties();
		
		GetNode<Goal>("GoalLeft").GoalScored += OnGoalScored;
		GetNode<Goal>("GoalRight").GoalScored += OnGoalScored;
		GetNode<Timer>("Timer").Timeout += OnTimerTimeout;
		
		_gameInterface.UpdateScore(_leftScore, "left");
		_gameInterface.UpdateScore(_rightScore, "right");
		_gameInterface.UpdateTimeLeft(_timeLeft);
		
		_gameInterface.GameUnpaused += () => { GetTree().Paused = false; };
		_gameInterface.MatchReplayed += OnMatchReplayed;
		_gameInterface.ReturnToMainMenuRequested += () => { EmitSignalReturnToMainMenu(); };
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			Rpc(nameof(PauseGame));
		}
	}
	
	private void SetPaddleProperties()
	{
		Paddle.ControllerType leftControllerType;
		Paddle.ControllerType rightControllerType;
		
		switch (CurrentMode)
		{
			case GameMode.Singleplayer:
				leftControllerType = Paddle.ControllerType.LeftPlayer;
				rightControllerType = Paddle.ControllerType.AI;
				break;
			case GameMode.LocalMultiplayer:
				leftControllerType = Paddle.ControllerType.LeftPlayer;
				rightControllerType = Paddle.ControllerType.RightPlayer;
				break;
			case GameMode.OnlineMultiplayer:
				leftControllerType = Paddle.ControllerType.OnlinePeer;
				rightControllerType = Paddle.ControllerType.OnlinePeer;
				break;
			default:
				leftControllerType = Paddle.ControllerType.LeftPlayer;
				rightControllerType = Paddle.ControllerType.AI;
				break;
		}
		
		_paddleLeft.SetProperties("Paddle" + LeftPlayerId, LeftPlayerId, leftControllerType);
		_paddleRight.SetProperties("Paddle" + RightPlayerId, RightPlayerId, rightControllerType);
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
		_paddleLeft.Position = _markerPaddleLeft.Position;
		_paddleRight.Position = _markerPaddleRight.Position;
		_ball.Position = _markerBall.Position;
		_ball.Launch();
	}

	private void OnMatchReplayed()
	{
		_timeLeft = DefaultTimeLeft;
		_leftScore = DefaultScore;
		_rightScore = DefaultScore;
		_paddleLeft.Position = _markerPaddleLeft.Position;
		_paddleRight.Position = _markerPaddleRight.Position;
		_ball.Position = _markerBall.Position;
		_ball.Launch();
	}

	[Rpc(
		MultiplayerApi.RpcMode.AnyPeer, 
		CallLocal = true, 
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, 
		TransferChannel = 0
	)]
	private void PauseGame()
	{
		_gameInterface.DisplayPauseContainer();
		GetTree().Paused = true;
	}
}
