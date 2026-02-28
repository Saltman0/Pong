using Godot;

public partial class Main : Node
{
	private Game _game;
	
	public static Main Instance { get; private set; }
	
	public override void _Ready()
	{
		SceneManager.Instance.CurrentScene = GetNode<MainMenuInterface>("MainMenuInterface");
		
		Instance = this;
	}
	
	[Rpc(
		MultiplayerApi.RpcMode.AnyPeer, 
		CallLocal = true, 
		TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, 
		TransferChannel = 0
	)]
	public void StartGame(Game.GameMode gameMode, long leftPlayerId = 1, long rightPlayerId = 2)
	{
		Game game = GD.Load<PackedScene>("res://scenes/game/game.tscn").Instantiate<Game>();
		game.CurrentMode = gameMode;
		if (game.CurrentMode == Game.GameMode.OnlineMultiplayer)
		{
			game.LeftPlayerId = leftPlayerId;
			game.RightPlayerId = rightPlayerId;
		}
		game.ReturnToMainMenu += () =>
		{
			SceneManager.Instance.SwitchScene(
				GD.Load<PackedScene>("res://scenes/main_menu_interface/main_menu_interface.tscn")
					.Instantiate<MainMenuInterface>()
			);
		};
		
		SceneManager.Instance.SwitchScene(game);
	}
}
