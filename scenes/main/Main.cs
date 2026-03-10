using Godot;
using Pong.managers;

public partial class Main : Node
{
	private Game _game;
	
	public static Main Instance { get; private set; }
	
	public override void _Ready()
	{
		bool isGeneralLoaded = SettingsManager.Instance.LoadGeneral();
		bool isVideoLoaded = SettingsManager.Instance.LoadVideo();
		bool isAudioLoaded = SettingsManager.Instance.LoadAudio();
		bool areControlsLoaded = SettingsManager.Instance.LoadControls();
		bool isAccessibilityLoaded = SettingsManager.Instance.LoadAccessibility();
		
		if (!isGeneralLoaded) { SettingsManager.Instance.SaveDefaultGeneral(); }
		if (!isVideoLoaded) { SettingsManager.Instance.SaveDefaultVideo(); }
		if (!isAudioLoaded) { SettingsManager.Instance.SaveDefaultAudio(); }
		if (!areControlsLoaded) { SettingsManager.Instance.SaveDefaultControls(); }
		if (!isAccessibilityLoaded) { SettingsManager.Instance.SaveDefaultAccessibility(); }
		
		SceneManager.Instance.CurrentScene = GetNode<MainMenuInterface>("MainMenuInterface");

		AudioManager.Instance.PlayMusic(GD.Load<AudioStreamOggVorbis>("res://assets/audio/Main.ogg"));
		
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
