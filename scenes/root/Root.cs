using Godot;

public partial class Root : Node
{
	private MainMenu _mainMenu;
	
	private Game _game;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainMenu = GetNode<MainMenu>("MainMenu");
		
		ConnectToMainMenu();
	}
	
	public void OnSingleplayerSelected()
	{
		RemoveChild(_mainMenu);
		
		AddChild(GD.Load<PackedScene>("res://scenes/game/game.tscn").Instantiate());
		_game = GetNode<Game>("Game");
		_game.IsMultiplayer = false;
		_game.ReturnToMainMenu += OnReturnToMainMenuRequested;
	}
	
	public void OnMultiplayerLocalSelected()
	{
		RemoveChild(_mainMenu);
		
		AddChild(GD.Load<PackedScene>("res://scenes/game/game.tscn").Instantiate());
		_game = GetNode<Game>("Game");
		_game.IsMultiplayer = true;
		_game.ReturnToMainMenu += OnReturnToMainMenuRequested;
	}
	
	public void OnMultiplayerOnlineSelected()
	{
		RemoveChild(_mainMenu);
		
		AddChild(GD.Load<PackedScene>("res://scenes/game/game.tscn").Instantiate());
		_game = GetNode<Game>("Game");
		_game.IsMultiplayer = true;
		_game.ReturnToMainMenu += OnReturnToMainMenuRequested;
	}
	
	public void OnSettingsSelected()
	{
		RemoveChild(GetNode<MainMenu>("MainMenu"));
		GD.Print("Settings button pressed");
	}
	
	public void OnQuitSelected()
	{
		GetTree().Quit();
	}
	
	private void OnReturnToMainMenuRequested()
	{
		RemoveChild(_game);
		
		AddChild(GD.Load<PackedScene>("res://scenes/main_menu/main_menu.tscn").Instantiate());
		_mainMenu = GetNode<MainMenu>("MainMenu");
		ConnectToMainMenu();
	}

	private void ConnectToMainMenu()
	{
		_mainMenu.SingleplayerSelected += OnSingleplayerSelected;
		_mainMenu.MultiplayerLocalSelected += OnMultiplayerLocalSelected;
		_mainMenu.MultiplayerOnlineSelected += OnMultiplayerOnlineSelected;
		_mainMenu.SettingsSelected += OnSettingsSelected;
		_mainMenu.QuitSelected += OnQuitSelected;
	}
}
