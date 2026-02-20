using Godot;

public partial class MainMenu : Node
{
	[Signal]
	public delegate void SingleplayerSelectedEventHandler();
	
	[Signal]
	public delegate void MultiplayerLocalSelectedEventHandler();
	
	[Signal]
	public delegate void MultiplayerOnlineSelectedEventHandler();
	
	[Signal]
	public delegate void SettingsSelectedEventHandler();
	
	[Signal]
	public delegate void QuitSelectedEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MainMenuInterface mainMenuInterface = GetNode<MainMenuInterface>("MainMenuInterface");
		
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.SingleplayerButtonPressed, 
			Callable.From(OnSingleplayerSelected)
		);
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.MultiplayerLocalButtonPressed, 
			Callable.From(OnMultiplayerLocalSelected)
		);
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.MultiplayerOnlineButtonPressed, 
			Callable.From(OnMultiplayerOnlineSelected)
		);
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.SettingsButtonPressed, 
			Callable.From(OnSettingsSelected)
		);
		mainMenuInterface.Connect(
			MainMenuInterface.SignalName.QuitButtonPressed, 
			Callable.From(OnQuitSelected)
		);
	}

	public void OnSingleplayerSelected()
	{
		EmitSignalSingleplayerSelected();
	}
	
	public void OnMultiplayerLocalSelected()
	{
		EmitSignalMultiplayerLocalSelected();
	}
	
	public void OnMultiplayerOnlineSelected()
	{
		EmitSignalMultiplayerOnlineSelected();
	}
	
	public void OnSettingsSelected()
	{
		EmitSignalSettingsSelected();
	}
	
	public void OnQuitSelected()
	{
		EmitSignalQuitSelected();
	}
}
