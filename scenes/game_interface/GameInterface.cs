using Godot;
using System;

public partial class GameInterface : Control
{
	[Signal]
	public delegate void ReplayMatchEventHandler();
	
	[Signal]
	public delegate void ReturnToMainMenuEventHandler();
	
	public override void _Ready()
	{
		GetNode<Button>("Main/ReplayButton").Pressed += OnReplayButtonPressed;
		GetNode<Button>("Main/ReturnToMainMenuButton").Pressed += OnReturnToMainMenuButtonPressed;
	}
	
	public void OnScoreUpdated(int newScore, string side)
	{
		if (side == "left")
		{
			GetNode<RichTextLabel>("Header/HBoxContainer/ScoreLeftLabel").Text = newScore.ToString();
		} else if (side == "right")
		{
			GetNode<RichTextLabel>("Header/HBoxContainer/ScoreRightLabel").Text = newScore.ToString();
		}
	}

	public void OnTimeUpdated(int seconds)
	{
		GetNode<RichTextLabel>("Header/HBoxContainer/TimerLabel").Text = seconds.ToString();
	}

	public void OnGameOver(string winner)
	{
		GetNode<VBoxContainer>("Main").Visible = true;
		if (winner != "none")
		{
			GetNode<RichTextLabel>("Main/WinnerLabel").Text = winner.ToUpper() + " paddle won !";
		} else {
			GetNode<RichTextLabel>("Main/WinnerLabel").Text = "It's a draw !";
		}
	}

	public void OnReplayButtonPressed()
	{
		ResetUi();
		EmitSignalReplayMatch();
	}
	
	public void OnReturnToMainMenuButtonPressed()
	{
		EmitSignalReturnToMainMenu();
	}

	private void ResetUi()
	{
		GetNode<VBoxContainer>("Main").Visible = false;
		GetNode<RichTextLabel>("Header/HBoxContainer/ScoreLeftLabel").Text = "0";
		GetNode<RichTextLabel>("Header/HBoxContainer/ScoreRightLabel").Text = "0";
		GetNode<RichTextLabel>("Header/HBoxContainer/TimerLabel").Text = "0";
	}
}
