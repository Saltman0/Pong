using Godot;

public partial class GameInterface : Control
{
	[Signal]
	public delegate void UnpauseGameEventHandler();
	
	[Signal]
	public delegate void ReplayMatchEventHandler();
	
	[Signal]
	public delegate void ReturnToMainMenuEventHandler();

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
		
		_pauseContainer.GetNode<Button>("ContinueButton").Pressed += OnContinueButtonPressed;
		_pauseContainer.GetNode<Button>("RestartButton").Pressed += OnRestartButtonPressed;
		_pauseContainer.GetNode<Button>("ReturnToMainMenuButton").Pressed += OnReturnToMainMenuButtonPressed;
		_gameOverContainer.GetNode<Button>("ReplayButton").Pressed += OnReplayButtonPressed;
		_gameOverContainer.GetNode<Button>("ReturnToMainMenuButton").Pressed += OnReturnToMainMenuButtonPressed;
	}
	
	public void OnScoreUpdated(int newScore, string side)
	{
		if (side == "left")
		{
			_scoreLeftLabel.Text = newScore.ToString();
		} else if (side == "right")
		{
			_scoreRightLabel.Text = newScore.ToString();
		}
	}

	public void OnTimeUpdated(int seconds)
	{
		_timerLabel.Text = seconds.ToString();
	}

	public void OnGameOver(string winner)
	{
		_gameOverContainer.Visible = true;
		if (winner != "none")
		{
			_winnerLabel.Text = winner.ToUpper() + " paddle won !";
		} else {
			_winnerLabel.Text = "It's a draw !";
		}
	}

	public void OnGamePaused()
	{
		_pauseContainer.Visible = true;
	}
	
	public void OnContinueButtonPressed()
	{
		_pauseContainer.Visible = false;
		EmitSignalUnpauseGame();
	}
	
	public void OnRestartButtonPressed()
	{
		ResetUi();
		EmitSignalUnpauseGame();
		EmitSignalReplayMatch();
	}

	public void OnReplayButtonPressed()
	{
		ResetUi();
		EmitSignalUnpauseGame();
		EmitSignalReplayMatch();
	}
	
	public void OnReturnToMainMenuButtonPressed()
	{
		EmitSignalUnpauseGame();
		EmitSignalReturnToMainMenu();
	}

	private void ResetUi()
	{
		_pauseContainer.Visible = false;
		_gameOverContainer.Visible = false;
		_scoreLeftLabel.Text = "0";
		_scoreRightLabel.Text = "0";
		_timerLabel.Text = "0";
	}
}
