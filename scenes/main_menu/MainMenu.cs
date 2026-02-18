using Godot;
using System;

public partial class MainMenu : Node
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
		MainMenuInterface mainMenuInterface = GetNode<MainMenuInterface>("MainMenuInterface");
		
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.SingleplayerButtonPressed, 
			Callable.From(OnSingleplayerButtonPressed)
		);
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.MultiplayerLocalButtonPressed, 
			Callable.From(OnMultiplayerLocalButtonPressed)
		);
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.MultiplayerOnlineButtonPressed, 
			Callable.From(OnMultiplayerOnlineButtonPressed)
		);
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.SettingsButtonPressed, 
			Callable.From(OnSettingsButtonPressed)
		);
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.QuitButtonPressed, 
			Callable.From(OnQuitButtonPressed)
		);
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
