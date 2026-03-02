using Godot;
using Pong.managers;

public partial class SettingsInterface : Control
{
	[Export] private OptionButton _windowModeOptionButton;
	[Export] private OptionButton _resolutionOptionButton;
	[Export] private OptionButton _vsyncOptionButton;
	[Export] private Button _saveButton;
	[Export] private Button _returnButton;

	[Export] private HSlider _masterSlider;
	[Export] private HSlider _musicSlider;
	[Export] private HSlider _sfxSlider;
	[Export] private HSlider _uiSlider;
	
	public override void _Ready()
	{
		_saveButton.Pressed += OnSaveButtonPressed;
		_returnButton.Pressed += OnReturnButtonPressed;
	}

	private void OnSaveButtonPressed()
	{
		SaveVideoSettings();
		SaveAudioSettings();
	}
	
	private void OnReturnButtonPressed()
	{
		SceneManager.Instance.SwitchScene(
			GD.Load<PackedScene>("res://scenes/main_menu_interface/main_menu_interface.tscn")
				.Instantiate<MainMenuInterface>()
		);
	}

	private void SaveVideoSettings()
	{
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.MaximizeDisabled, false);
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.ResizeDisabled, false);
		
		VideoManager.SetWindowMode((DisplayServer.WindowMode)_windowModeOptionButton.GetSelectedId());
		VideoManager.SetWindowScale(_resolutionOptionButton.GetSelectedId());
		VideoManager.SetVsync((DisplayServer.VSyncMode)_vsyncOptionButton.GetSelectedId());
		
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.MaximizeDisabled, true);
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.ResizeDisabled, true);
	}
	
	private void SaveAudioSettings()
	{
		AudioManager.Instance.SetVolume("Master", (float)_masterSlider.Value);
		AudioManager.Instance.SetVolume("Music", (float)_musicSlider.Value);
		AudioManager.Instance.SetVolume("SFX", (float)_sfxSlider.Value);
		AudioManager.Instance.SetVolume("UI", (float)_uiSlider.Value);
	}
}
