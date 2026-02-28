using Godot;

public partial class Game : Node
{
	public enum GameMode: byte
	{
		Singleplayer,
		LocalMultiplayer,
		OnlineMultiplayer
	}
	
	private const int DefaultScore = 0;
	private const int DefaultTimeLeft = 60;
	private int _timeLeft = DefaultTimeLeft;
	private int _leftScore = DefaultScore;
	private int _rightScore = DefaultScore;
	
	[Signal]
	public delegate void ReturnToMainMenuEventHandler();
	
	[Export] public GameMode CurrentMode;
	[Export] public long LeftPlayerId;
	[Export] public long RightPlayerId;
	[Export] private GameInterface _gameInterface;
	[Export] private Paddle _paddleLeft;
	[Export] private Paddle _paddleRight;
	[Export] private Ball _ball;
	[Export] private Goal _goalLeft;
	[Export] private Goal _goalRight;
	[Export] private Marker2D _markerPaddleLeft;
	[Export] private Marker2D _markerPaddleRight;
	[Export] private Marker2D _markerBall;
	[Export] private Timer _timer;
	
	public override void _Ready()
	{
		SetPaddleProperties();
		
		_gameInterface.UpdateScore(_leftScore, "left");
		_gameInterface.UpdateScore(_rightScore, "right");
		_gameInterface.UpdateTimeLeft(_timeLeft);
		_gameInterface.GameUnpaused += () => { GetTree().Paused = false; };
		_gameInterface.MatchReplayed += OnMatchReplayed;
		_gameInterface.ReturnToMainMenuRequested += () => { EmitSignalReturnToMainMenu(); };
		
		_goalLeft.GoalScored += OnGoalScored;
		_goalRight.GoalScored += OnGoalScored;
		_timer.Timeout += OnTimerTimeout;
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
		
		Rpc(nameof(ResetPositions));
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
	
	[Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
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
