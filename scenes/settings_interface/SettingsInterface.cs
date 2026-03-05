using System.Linq;
using Godot;
using Godot.Collections;
using Pong.managers;

public partial class SettingsInterface : Control
{
	// Video settings
	[Export] private OptionButton _windowModeOptionButton;
	[Export] private OptionButton _resolutionOptionButton;
	[Export] private OptionButton _vsyncOptionButton;
	[Export] private LineEdit _framerateInput;
	[Export] private Button _saveButton;
	[Export] private Button _returnButton;

	// Audio settings
	[Export] private HSlider _masterSlider;
	[Export] private HSlider _musicSlider;
	[Export] private HSlider _sfxSlider;
	[Export] private HSlider _uiSlider;
	
	// Controls settings
	[Export] private Button _moveUpP1Button;
	[Export] private Button _moveDownP1Button;
	[Export] private Button _moveUpP2Button;
	[Export] private Button _moveDownP2Button;
	private Button _selectedControlsButton;
	private Dictionary<Button, string> _inputMaps;
	
	public override void _Ready()
	{
		_inputMaps = new Dictionary<Button, string>();
		_inputMaps.Add(_moveUpP1Button, "move_up");
		_inputMaps.Add(_moveDownP1Button, "move_down");
		_inputMaps.Add(_moveUpP2Button, "move_up_2");
		_inputMaps.Add(_moveDownP2Button, "move_down_2");

		_vsyncOptionButton.ItemSelected += itemSelectedIndex => OnVsyncOptionButtonItemSelected(itemSelectedIndex);
		_moveUpP1Button.Pressed += () => OnControlsButtonPressed(_moveUpP1Button);
		_moveDownP1Button.Pressed += () => OnControlsButtonPressed(_moveDownP1Button);
		_moveUpP2Button.Pressed += () => OnControlsButtonPressed(_moveUpP2Button);
		_moveDownP2Button.Pressed += () => OnControlsButtonPressed(_moveDownP2Button);
		_saveButton.Pressed += OnSaveButtonPressed;
		_returnButton.Pressed += OnReturnButtonPressed;
		
		UpdateControlSettings();
		UpdateAudioSettings();
		UpdateVideoSettings();
	}

	public override void _Input(InputEvent @event)
	{
		if (_selectedControlsButton == null || @event is not InputEventKey)
		{
			return;
		}
		
		string actionName = _inputMaps.First(x => x.Key == _selectedControlsButton).Value;
		
		InputMap.ActionEraseEvents(actionName);
		InputMap.ActionAddEvent(actionName, @event);
			
		_selectedControlsButton.Text = @event.AsText();
		
		_selectedControlsButton = null;
	}
	
	private void OnControlsButtonPressed(Button selectedControlsButton)
	{
		UpdateControlSettings();
		selectedControlsButton.Text = "?";
		_selectedControlsButton = selectedControlsButton;
	}

	private void OnVsyncOptionButtonItemSelected(long itemSelectedIndex)
	{
		DisplayServer.VSyncMode vsyncMode = (DisplayServer.VSyncMode) itemSelectedIndex;
		if (vsyncMode != DisplayServer.VSyncMode.Disabled)
		{
			_framerateInput.Text = "";
			_framerateInput.MaxLength = 0;
		}
		else
		{
			_framerateInput.MaxLength = 3;
		}
	}
	
	private void OnSaveButtonPressed()
	{
		SaveVideoSettings();
		SaveAudioSettings();
		SaveControlsSettings();
	}
	
	private void OnReturnButtonPressed()
	{
		SceneManager.Instance.SwitchScene(
			GD.Load<PackedScene>("res://scenes/main_menu_interface/main_menu_interface.tscn")
				.Instantiate<MainMenuInterface>()
		);
	}
	
	private void UpdateControlSettings()
	{
		InputEvent moveUpP1InputEvent = InputMap.ActionGetEvents("move_up").First();
		InputEvent moveDownP1InputEvent = InputMap.ActionGetEvents("move_down").First();
		InputEvent moveUpP2InputEvent = InputMap.ActionGetEvents("move_up_2").First();
		InputEvent moveDownP2InputEvent = InputMap.ActionGetEvents("move_down_2").First();
		
		_moveUpP1Button.Text = moveUpP1InputEvent.AsText();
		_moveDownP1Button.Text = moveDownP1InputEvent.AsText();
		_moveUpP2Button.Text = moveUpP2InputEvent.AsText();
		_moveDownP2Button.Text = moveDownP2InputEvent.AsText();
	}
	private void UpdateAudioSettings()
	{
		_masterSlider.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Master"));
		_musicSlider.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Music"));
		_sfxSlider.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("SFX"));
		_uiSlider.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("UI"));
	}
	
	private void UpdateVideoSettings()
	{
		SelectOption(
			_windowModeOptionButton, 
			"Video", 
			"WindowMode", 
			(int) SettingsManager.DefaultWindowMode
		);
		
		SelectOption(
			_resolutionOptionButton, 
			"Video", 
			"Resolution", 
			1
		);
		
		SelectOption(
			_vsyncOptionButton, 
			"Video", 
			"Vsync", 
			(int) DisplayServer.VSyncMode.Enabled
		);
		
		int framerate = (int) SettingsManager.GetValue(
			"Video", "Framerate", SettingsManager.DefaultFramerate
		);
		_framerateInput.Text = framerate == 0 ? "" : framerate.ToString();
	}

	private void SaveVideoSettings()
	{
		DisplayServer.WindowSetMode((DisplayServer.WindowMode)_windowModeOptionButton.GetSelectedId());
		DisplayServer.WindowSetSize(new Vector2I(640, 360) * _resolutionOptionButton.GetSelectedId());
		DisplayServer.WindowSetVsyncMode((DisplayServer.VSyncMode)_vsyncOptionButton.GetSelectedId());
		
		Engine.SetMaxFps(_framerateInput.Text.ToInt());
		
		SettingsManager.SaveValue("Video", "WindowMode", _windowModeOptionButton.GetSelectedId());
		SettingsManager.SaveValue("Video", "Resolution", _resolutionOptionButton.GetSelectedId());
		SettingsManager.SaveValue("Video", "Vsync", _vsyncOptionButton.GetSelectedId());
		
		int framerate = _framerateInput.Text != "" && _framerateInput.Text != "0" ? _framerateInput.Text.ToInt() : 0;
		SettingsManager.SaveValue("Video", "Framerate", framerate);
		
		SettingsManager.LoadVideo();
	}
	
	private void SaveAudioSettings()
	{
		SettingsManager.SaveValue("Audio", "Master", (float)_masterSlider.Value);
		SettingsManager.SaveValue("Audio", "Music", (float)_musicSlider.Value);
		SettingsManager.SaveValue("Audio", "SFX", (float)_sfxSlider.Value);
		SettingsManager.SaveValue("Audio", "UI", (float)_uiSlider.Value);

		SettingsManager.LoadAudio();
	}
	
	private void SaveControlsSettings()
	{
		SettingsManager.SaveValue("Controls", "move_up", SettingsManager.DefaultMoveUpP1InputEvent);
		SettingsManager.SaveValue("Controls", "move_down", SettingsManager.DefaultMoveDownP1InputEvent);
		SettingsManager.SaveValue("Controls", "move_up_2", SettingsManager.DefaultMoveUpP2InputEvent);
		SettingsManager.SaveValue("Controls", "move_down_2", SettingsManager.DefaultMoveDownP2InputEvent);

		SettingsManager.LoadControls();
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
