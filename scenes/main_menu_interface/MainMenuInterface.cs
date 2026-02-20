using Godot;

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
		EmitSignalSingleplayerButtonPressed();
	}
	
	public void OnMultiplayerLocalButtonPressed()
	{
		EmitSignalMultiplayerLocalButtonPressed();
	}
	
	public void OnMultiplayerOnlineButtonPressed()
	{
		EmitSignalMultiplayerOnlineButtonPressed();
	}
	
	public void OnSettingsButtonPressed()
	{
		EmitSignalSettingsButtonPressed();
	}
	
	public void OnQuitButtonPressed()
	{
		EmitSignalQuitButtonPressed();
	}
}
