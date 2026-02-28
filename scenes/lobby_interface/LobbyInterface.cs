using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class LobbyInterface : Control
{
	[Signal]
	public delegate void ReturnButtonPressedEventHandler();
	
	[Signal]
	public delegate void ServerDisconnectedEventHandler();

	[Export]
	private long _leftPlayerId;
	
	[Export]
	private long _rightPlayerId;
	
	public override void _Ready()
	{
		GetNode<Button>("MainContainer/ActionsContainer/StartButton").Pressed += OnStartButtonPressed;
		GetNode<Button>("MainContainer/ActionsContainer/ReturnButton").Pressed += OnReturnButtonPressed;

		if (Multiplayer.IsServer())
		{
			NetworkManager.Instance.PlayerConnected += OnLobbyChanged;
			NetworkManager.Instance.PlayerDisconnected += OnLobbyChanged;
			OnLobbyChanged();
		}
		
		NetworkManager.Instance.ServerDisconnected += OnServerDisconnected;
	}
	
	public override void _ExitTree()
	{
		if (NetworkManager.Instance != null)
		{
			NetworkManager.Instance.PlayerConnected += OnLobbyChanged;
			NetworkManager.Instance.PlayerDisconnected += OnLobbyChanged;
			NetworkManager.Instance.ServerDisconnected += OnServerDisconnected;
		}
    
		base._ExitTree();
	}
	
	private void OnLobbyChanged()
	{
		GD.PushWarning("OnLobbyChanged");
		List<long> players = Multiplayer.GetPeers().Select(id => (long)id).ToList();
		players.Add(Multiplayer.GetUniqueId());
		players.Sort();
		
		long leftPlayerId = players.Count > 0 ? players[0] : 0;
		long rightPlayerId = players.Count > 1 ? players[1] : 0;

		SyncLobby(leftPlayerId, rightPlayerId);
	}
	
	private void SyncLobby(long player1, long player2)
	{
		_leftPlayerId = player1;
		_rightPlayerId = player2;
		
		GetNode<Button>("MainContainer/PlayersContainer/PlayerLeftButton").Text = "Player " + _leftPlayerId;
		GetNode<Button>("MainContainer/PlayersContainer/PlayerRightButton").Text = "Player " + _rightPlayerId;
		
		GetNode<Button>("MainContainer/ActionsContainer/StartButton").Disabled = 
			_leftPlayerId == 0 || _rightPlayerId == 0;
	}
	
	private void OnStartButtonPressed()
	{
		Main.Instance.Rpc(
			nameof(Main.Instance.StartGame),
			(byte)Game.GameMode.OnlineMultiplayer,
			_leftPlayerId,
			_rightPlayerId
		);
	}
	
	private void OnReturnButtonPressed()
	{
		NetworkManager.Instance.DisconnectPlayer();
		
		SceneManager.Instance.SwitchScene(
			GD.Load<PackedScene>(
				"res://scenes/online_multiplayer_menu_interface/online_multiplayer_menu_interface.tscn"
			).Instantiate<OnlineMultiplayerMenuInterface>()
		);
		
	}
	
	private void OnServerDisconnected()
	{
		NetworkManager.Instance.DisconnectPlayer();
		
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
}
