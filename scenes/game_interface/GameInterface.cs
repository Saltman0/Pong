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
		
		_pauseContainer.GetNode<Button>("ContinueButton").Pressed += OnContinueButtonPressed;
		_pauseContainer.GetNode<Button>("RestartButton").Pressed += OnRestartButtonPressed;
		_pauseContainer.GetNode<Button>("ReturnToMainMenuButton").Pressed += OnReturnToMainMenuButtonPressed;
		_gameOverContainer.GetNode<Button>("ReplayButton").Pressed += OnReplayButtonPressed;
		_gameOverContainer.GetNode<Button>("ReturnToMainMenuButton").Pressed += OnReturnToMainMenuButtonPressed;
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
	
	public void HidePauseContainer()
	{
		_pauseContainer.Visible = false;
	}
	
	private void OnContinueButtonPressed()
	{
		_pauseContainer.Visible = false;
		EmitSignalGameUnpaused();
	}
	
	private void OnRestartButtonPressed()
	{
		ResetUi();
		EmitSignalGameUnpaused();
		EmitSignalMatchReplayed();
	}

	private void OnReplayButtonPressed()
	{
		ResetUi();
		EmitSignalGameUnpaused();
		EmitSignalMatchReplayed();
	}
	
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
