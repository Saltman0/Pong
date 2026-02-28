using Godot;

public partial class GameInterface : Control
{
	[Signal]
	public delegate void GameUnpausedEventHandler();
	
	[Signal]
	public delegate void MatchReplayedEventHandler();
	
	[Signal]
	public delegate void ReturnToMainMenuRequestedEventHandler();
	
	private const int DefaultScore = 0;

	private const int DefaultTimeLeft = 60;

	private VBoxContainer _gameOverContainer;
	
	private VBoxContainer _pauseContainer;

	private RichTextLabel _scoreLeftLabel;
	
	private RichTextLabel _scoreRightLabel;

	private RichTextLabel _timerLabel;
	
	private RichTextLabel _winnerLabel;
	
	public override void _Ready()
	{
		_pauseContainer = GetNode<VBoxContainer>("PauseContainer");
		_gameOverContainer = GetNode<VBoxContainer>("GameOverContainer");
		_scoreLeftLabel = GetNode<RichTextLabel>("HeaderContainer/HBoxContainer/ScoreLeftLabel");
		_scoreRightLabel = GetNode<RichTextLabel>("HeaderContainer/HBoxContainer/ScoreRightLabel");
		_timerLabel = GetNode<RichTextLabel>("HeaderContainer/HBoxContainer/TimerLabel");
		_winnerLabel = GetNode<RichTextLabel>("GameOverContainer/WinnerLabel");
		
		_pauseContainer.GetNode<Button>("ContinueButton").Pressed += () =>
		{
			Rpc(nameof(OnContinueButtonPressed));
		};
		_pauseContainer.GetNode<Button>("RestartButton").Pressed += () =>
		{
			Rpc(nameof(OnRestartButtonPressed));
		};
		_pauseContainer.GetNode<Button>("ReturnToMainMenuButton").Pressed += () =>
		{
			Rpc(nameof(OnReturnToMainMenuButtonPressed));
		};
		_gameOverContainer.GetNode<Button>("ReplayButton").Pressed += () =>
		{
			Rpc(nameof(OnReplayButtonPressed));
		};
		_gameOverContainer.GetNode<Button>("ReturnToMainMenuButton").Pressed += () =>
		{
			Rpc(nameof(OnReturnToMainMenuButtonPressed));
		};
	}
	
	public void UpdateScore(int newScore, string side)
	{
		if (side == "left")
		{
			GD.PushWarning("UpdateLeftScore: " + newScore);
			_scoreLeftLabel.Text = newScore.ToString();
		} else if (side == "right")
		{
			GD.PushWarning("UpdateRightScore: " + newScore);
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
