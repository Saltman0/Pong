using Godot;
using System;

public partial class MainMenuInterface : Control
{
	[Signal]
	public delegate void SingleplayerButtonPressedEventHandler();
	
	[Signal]
	public delegate void MultiplayerLocalButtonPressedEventHandler();
	
	[Signal]
	public delegate void MultiplayerOnlineButtonPressedEventHandler();
	
	[Signal]
	public delegate void SettingsButtonPressedEventHandler();
	
	[Signal]
	public delegate void QuitButtonPressedEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("VBoxContainer/SingleplayerButton").Pressed += OnSingleplayerButtonPressed;
		GetNode<Button>("VBoxContainer/MultiplayerLocalButton").Pressed += OnMultiplayerLocalButtonPressed;
		GetNode<Button>("VBoxContainer/MultiplayerOnlineButton").Pressed += OnMultiplayerOnlineButtonPressed;
		GetNode<Button>("VBoxContainer/SettingsButton").Pressed += OnSettingsButtonPressed;
		GetNode<Button>("VBoxContainer/QuitButton").Pressed += OnQuitButtonPressed;
	}

	public void OnSingleplayerButtonPressed()
	{
		EmitSignal(SignalName.SingleplayerButtonPressed);
	}
	
	public void OnMultiplayerLocalButtonPressed()
	{
		EmitSignal(SignalName.MultiplayerLocalButtonPressed);
	}
	
	public void OnMultiplayerOnlineButtonPressed()
	{
		EmitSignal(SignalName.MultiplayerOnlineButtonPressed);
	}
	
	public void OnSettingsButtonPressed()
	{
		EmitSignal(SignalName.SettingsButtonPressed);
	}
	
	public void OnQuitButtonPressed()
	{
		EmitSignal(SignalName.QuitButtonPressed);
	}
}
