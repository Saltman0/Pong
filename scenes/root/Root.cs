using Godot;
using System;

public partial class Root : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MainMenu mainMenu = GetNode<MainMenu>("MainMenu");
		
		mainMenu.Connect(
			MainMenu.SignalName.SingleplayerButtonPressed, 
			Callable.From(OnSingleplayerButtonPressed)
		);
		mainMenu.Connect(
			MainMenu.SignalName.MultiplayerLocalButtonPressed, 
			Callable.From(OnMultiplayerLocalButtonPressed)
		);
		mainMenu.Connect(
			MainMenu.SignalName.MultiplayerOnlineButtonPressed, 
			Callable.From(OnMultiplayerOnlineButtonPressed)
		);
		mainMenu.Connect(
			MainMenu.SignalName.SettingsButtonPressed, 
			Callable.From(OnSettingsButtonPressed)
		);
		mainMenu.Connect(
			MainMenu.SignalName.QuitButtonPressed, 
			Callable.From(OnQuitButtonPressed)
		);
	}
	
	public void OnSingleplayerButtonPressed()
	{
		RemoveChild(GetNode<MainMenu>("MainMenu"));
		AddChild(GD.Load<PackedScene>("res://scenes/game/game.tscn").Instantiate());
		GetNode<Game>("Game").IsMultiplayer = false;
	}
	
	public void OnMultiplayerLocalButtonPressed()
	{
		RemoveChild(GetNode<MainMenu>("MainMenu"));
		AddChild(
			GD.Load<PackedScene>("res://scenes/game/game.tscn").Instantiate()
		);
		GetNode<Game>("Game").IsMultiplayer = true;
	}
	
	public void OnMultiplayerOnlineButtonPressed()
	{
		RemoveChild(GetNode<MainMenu>("MainMenu"));
		GD.Print("Multiplayer online button pressed");
	}
	
	public void OnSettingsButtonPressed()
	{
		RemoveChild(GetNode<MainMenu>("MainMenu"));
		GD.Print("Settings button pressed");
	}
	
	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
