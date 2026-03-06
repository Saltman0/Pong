using Godot;
using Pong.managers;

public partial class SettingsInterface : Control
{
	[Export] private VideoSettingsInterface _videoSettingsInterface;
	[Export] private AudioSettingsInterface _audioSettingsInterface;
	[Export] private ControlsSettingsInterface _controlsSettingsInterface;
	[Export] private AccessibilitySettingsInterface _accessibilitySettingsInterface;
	
	[Export] private Button _saveButton;
	[Export] private Button _returnButton;
	
	public override void _Ready()
	{
		_saveButton.Pressed += OnSaveButtonPressed;
		_returnButton.Pressed += OnReturnButtonPressed;
		
		_videoSettingsInterface.UpdateVideoSettings();
		_audioSettingsInterface.UpdateAudioSettings();
		_controlsSettingsInterface.UpdateControlSettings();
		_accessibilitySettingsInterface.UpdateAccessibilitySettings();
	}
	
	private void OnSaveButtonPressed()
	{
		_videoSettingsInterface.SaveVideoSettings();
		_audioSettingsInterface.SaveAudioSettings();
		_controlsSettingsInterface.SaveControlsSettings();
		_accessibilitySettingsInterface.SaveAccessibilitySettings();
	}
	
	private void OnReturnButtonPressed()
	{
		SceneManager.Instance.SwitchScene(
			GD.Load<PackedScene>("res://scenes/main_menu_interface/main_menu_interface.tscn")
				.Instantiate<MainMenuInterface>()
		);
	}
}
