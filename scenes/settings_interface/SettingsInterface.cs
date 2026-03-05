using Godot;
using Pong.managers;

public partial class SettingsInterface : Control
{
	[Export] private VideoSettingsInterface _videoSettingsInterface;
	[Export] private AudioSettingsInterface _audioSettingsInterface;
	[Export] private ControlsSettingsInterface _controlsSettingsInterface;
	
	[Export] private Button _saveButton;
	[Export] private Button _returnButton;
	
	public override void _Ready()
	{
		_saveButton.Pressed += OnSaveButtonPressed;
		_returnButton.Pressed += OnReturnButtonPressed;
		
		_controlsSettingsInterface.UpdateControlSettings();
		_audioSettingsInterface.UpdateAudioSettings();
		_videoSettingsInterface.UpdateVideoSettings();
	}
	
	private void OnSaveButtonPressed()
	{
		_videoSettingsInterface.SaveVideoSettings();
		_audioSettingsInterface.SaveAudioSettings();
		_controlsSettingsInterface.SaveControlsSettings();
	}
	
	private void OnReturnButtonPressed()
	{
		SceneManager.Instance.SwitchScene(
			GD.Load<PackedScene>("res://scenes/main_menu_interface/main_menu_interface.tscn")
				.Instantiate<MainMenuInterface>()
		);
	}
	
	private void SelectOption(OptionButton optionButton, string section, string key, int defaultValue)
	{
		for (int i = 0; i < optionButton.ItemCount; i++)
		{
			int itemId = optionButton.GetItemId(i);
			if (itemId == (int) SettingsManager.GetValue(section, key, defaultValue))
			{
				optionButton.Selected = optionButton.GetItemIndex(itemId);
			}
		}
	}
}
