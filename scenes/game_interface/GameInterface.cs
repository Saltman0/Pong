using Godot;

public partial class GameInterface : Control
{
	[Signal] public delegate void GameUnpausedEventHandler();
	[Signal] public delegate void MatchReplayedEventHandler();
	[Signal] public delegate void ReturnToMainMenuRequestedEventHandler();
	
	private const int DefaultScore = 0;

	private const int DefaultTimeLeft = 60;

	[Export] private VBoxContainer _gameOverContainer;
	[Export] private VBoxContainer _pauseContainer;
	[Export] private RichTextLabel _scoreLeftLabel;
	[Export] private RichTextLabel _scoreRightLabel;
	[Export] private RichTextLabel _timerLabel;
	[Export] private RichTextLabel _winnerLabel;
	[Export] private Button _continueButton;
	[Export] private Button _restartButton;
	[Export] private Button _pauseReturnToMainMenuButton;
	[Export] private Button _replayButton;
	[Export] private Button _gameOverReturnToMainMenuButton;
	
	public override void _Ready()
	{
		_continueButton.Pressed += () =>
		{
			Rpc(nameof(OnContinueButtonPressed));
		};
		_restartButton.Pressed += () =>
		{
			Rpc(nameof(OnRestartButtonPressed));
		};
		_pauseReturnToMainMenuButton.Pressed += () =>
		{
			Rpc(nameof(OnReturnToMainMenuButtonPressed));
		};
		_replayButton.Pressed += () =>
		{
			Rpc(nameof(OnReplayButtonPressed));
		};
		_gameOverReturnToMainMenuButton.Pressed += () =>
		{
			Rpc(nameof(OnReturnToMainMenuButtonPressed));
		};
	}
	
	public void UpdateScore(int newScore, string side)
	{
		if (side == "left")
		{
			_scoreLeftLabel.Text = newScore.ToString();
		} else if (side == "right")
		{
			_scoreRightLabel.Text = newScore.ToString();
		}
	}
	
	public void UpdateTimeLeft(int seconds)
	{
		_timerLabel.Text = seconds.ToString();
	}
	
	public void DisplayGameOverContainer(string winner)
	{
		_gameOverContainer.Visible = true;
		if (winner != "none")
		{
			_winnerLabel.Text = winner.ToUpper() + " paddle won !";
		} else {
			_winnerLabel.Text = "It's a draw !";
		}
	}
	
	public void DisplayPauseContainer()
	{
		_pauseContainer.Visible = true;
	}
	
	[Rpc(
		MultiplayerApi.RpcMode.AnyPeer, 
		CallLocal = true, 
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, 
		TransferChannel = 0
	)]
	private void OnContinueButtonPressed()
	{
		_pauseContainer.Visible = false;
		EmitSignalGameUnpaused();
	}
	
	[Rpc(
		MultiplayerApi.RpcMode.AnyPeer, 
		CallLocal = true, 
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, 
		TransferChannel = 0
	)]
	private void OnRestartButtonPressed()
	{
		ResetUi();
		EmitSignalGameUnpaused();
		EmitSignalMatchReplayed();
	}

	[Rpc(
		MultiplayerApi.RpcMode.AnyPeer, 
		CallLocal = true, 
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, 
		TransferChannel = 0
	)]
	private void OnReplayButtonPressed()
	{
		ResetUi();
		EmitSignalGameUnpaused();
		EmitSignalMatchReplayed();
	}
	
	[Rpc(
		MultiplayerApi.RpcMode.AnyPeer, 
		CallLocal = true, 
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, 
		TransferChannel = 0
	)]
	private void OnReturnToMainMenuButtonPressed()
	{
		EmitSignalGameUnpaused();
		EmitSignalReturnToMainMenuRequested();
	}

	private void ResetUi()
	{
		_pauseContainer.Visible = false;
		_gameOverContainer.Visible = false;
		_scoreLeftLabel.Text = DefaultScore.ToString();
		_scoreRightLabel.Text = DefaultScore.ToString();
		_timerLabel.Text = DefaultTimeLeft.ToString();
	}
}
