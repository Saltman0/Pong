using Godot;

public partial class MainMenuInterface : Control
{
	public override void _Ready()
	{
		VBoxContainer mainContainer = GetNode<VBoxContainer>("MainContainer");
		
		mainContainer.GetNode<Button>("SingleplayerButton").Pressed += () =>
		{
			Main.Instance.StartGame(Game.GameMode.Singleplayer);
		};
		mainContainer.GetNode<Button>("MultiplayerLocalButton").Pressed += () =>
		{
			Main.Instance.StartGame(Game.GameMode.LocalMultiplayer);
		};
		mainContainer.GetNode<Button>("MultiplayerOnlineButton").Pressed += () =>
		{
			SceneManager.Instance.SwitchScene(
				GD.Load<PackedScene>(
					"res://scenes/online_multiplayer_menu_interface/online_multiplayer_menu_interface.tscn"
				).Instantiate<OnlineMultiplayerMenuInterface>()
			);
		};
		mainContainer.GetNode<Button>("SettingsButton").Pressed += () =>
		{
			SceneManager.Instance.SwitchScene(
				GD.Load<PackedScene>("res://scenes/settings_interface/settings_interface.tscn")
					.Instantiate<SettingsInterface>()
			);
		};
		mainContainer.GetNode<Button>("QuitButton").Pressed += () => { GetTree().Quit(); };
	}
}
