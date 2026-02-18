using Godot;
using System;

public partial class Main : Node
{
	[Signal]
	public delegate void SingleplayerGamePressedEventHandler();
	
	[Signal]
	public delegate void MultiplayerLocalGamePressedEventHandler();
	
	[Signal]
	public delegate void MultiplayerOnlineGamePressedEventHandler();
	
	[Signal]
	public delegate void SettingsPressedEventHandler();
	
	[Signal]
	public delegate void QuitPressedEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SingleplayerGamePressed += GetNode<MainMenuInterface>("MainMenuInterface").OnSingleplayerButtonPressed;
		MultiplayerLocalGamePressed += GetNode<MainMenuInterface>("MainMenuInterface").OnMultiplayerLocalButtonPressed;
		MultiplayerOnlineGamePressed += GetNode<MainMenuInterface>("MainMenuInterface").OnMultiplayerOnlineButtonPressed;
		SettingsPressed += GetNode<MainMenuInterface>("MainMenuInterface").OnSettingsButtonPressed;
		QuitPressed += GetNode<MainMenuInterface>("MainMenuInterface").OnQuitButtonPressed;
	}
	
	
}
