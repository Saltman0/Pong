using Godot;

public partial class OnlineMultiplayerMenuInterface : Control
{
	[Signal]
	public delegate void ReturnToMainMenuButtonPressedEventHandler();
	
	public override void _Ready()
	{
		GetNode<Button>("MainContainer/HostGameButton").Pressed += OnHostGameButtonPressed;
		GetNode<Button>("MainContainer/JoinGameContainer/JoinGameButton").Pressed += OnJoinGameButtonPressed;
		GetNode<LineEdit>("MainContainer/JoinGameContainer/IpAddressInput").TextChanged += text =>
		{
			GetNode<Button>("MainContainer/JoinGameContainer/JoinGameButton").Disabled = text.Length == 0;
		};
		GetNode<Button>("MainContainer/ReturnToMainMenuButton").Pressed += OnReturnToMainMenuButtonPressed;
		
		NetworkManager.Instance.PlayerConnected += OnPlayerConnected;
		NetworkManager.Instance.ConnectionFailed += OnConnectionFailed;
	}
	
	public override void _ExitTree()
	{
		if (NetworkManager.Instance != null)
		{
			NetworkManager.Instance.PlayerConnected -= OnPlayerConnected;
			NetworkManager.Instance.ConnectionFailed -= OnConnectionFailed;
		}
    
		base._ExitTree();
	}

	private void OnHostGameButtonPressed()
	{
		NetworkManager.Instance.CreateServer();

		LobbyInterface lobbyInterface = GD.Load<PackedScene>(
			"res://scenes/lobby_interface/lobby_interface.tscn"
		).Instantiate<LobbyInterface>();
		lobbyInterface.ReturnButtonPressed += OnReturnButtonPressed;
		lobbyInterface.ServerDisconnected += OnServerDisconnected;
		
		SceneManager.Instance.SwitchScene(lobbyInterface);
	}
	
	private void OnJoinGameButtonPressed()
	{
		NetworkManager.Instance.CreateClient(
			GetNode<LineEdit>("MainContainer/JoinGameContainer/IpAddressInput").Text
		);
		
		GetNode<Button>("MainContainer/JoinGameContainer/JoinGameButton").Text = "Pending...";
	}
	
	private void OnReturnToMainMenuButtonPressed()
	{
		SceneManager.Instance.SwitchScene(
			GD.Load<PackedScene>("res://scenes/main_menu_interface/main_menu_interface.tscn")
				.Instantiate<MainMenuInterface>()
		);
	}
	
	private void OnReturnButtonPressed()
	{
		SceneManager.Instance.SwitchScene(
			GD.Load<PackedScene>("res://scenes/main_menu_interface/main_menu_interface.tscn")
				.Instantiate<MainMenuInterface>()
		);
	}
	
	private void OnServerDisconnected()
	{
		MessageInterface messageInterface = GD.Load<PackedScene>(
			"res://scenes/message_interface/message_interface.tscn"
		).Instantiate<MessageInterface>();
		messageInterface.SetMessageText("Le serveur a été clos.");
		messageInterface.SetVisibleOkButton(true);
		messageInterface.MessageClosed += () =>
		{
			SceneManager.Instance.SwitchScene(
				GD.Load<PackedScene>(
					"res://scenes/online_multiplayer_menu_interface/online_multiplayer_menu_interface.tscn"
				)
				.Instantiate<OnlineMultiplayerMenuInterface>()
			);
		};
		
		SceneManager.Instance.SwitchScene(messageInterface);
	}
	
	private void OnPlayerConnected()
	{
		GD.PushWarning("OnPlayerConnected");
		LobbyInterface lobbyInterface = GD.Load<PackedScene>(
			"res://scenes/lobby_interface/lobby_interface.tscn"
		).Instantiate<LobbyInterface>();
		lobbyInterface.ReturnButtonPressed += OnReturnButtonPressed;
		lobbyInterface.ServerDisconnected += OnServerDisconnected;
		
		SceneManager.Instance.SwitchScene(lobbyInterface);
	}
	
	private void OnConnectionFailed()
	{
		MessageInterface messageInterface = GD.Load<PackedScene>(
			"res://scenes/message_interface/message_interface.tscn"
		).Instantiate<MessageInterface>();
		messageInterface.SetMessageText("La connexion au serveur a échoué.");
		messageInterface.SetVisibleOkButton(true);
		messageInterface.MessageClosed += () =>
		{
			SceneManager.Instance.SwitchScene(
				GD.Load<PackedScene>(
					"res://scenes/online_multiplayer_menu_interface/online_multiplayer_menu_interface.tscn"
				)
				.Instantiate<OnlineMultiplayerMenuInterface>()
			);
		};
		
		SceneManager.Instance.SwitchScene(messageInterface);
	}
}
