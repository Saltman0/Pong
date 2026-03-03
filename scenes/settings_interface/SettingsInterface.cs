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
	[Export] private Dictionary<string, Button> _inputMaps;
	private Button _selectedControlsButton;
	
	public override void _Ready()
	{
		_moveUpP1Button.Pressed += () => OnControlsButtonPressed(_moveUpP1Button);
		_moveDownP1Button.Pressed += () => OnControlsButtonPressed(_moveDownP1Button);
		_moveUpP2Button.Pressed += () => OnControlsButtonPressed(_moveUpP2Button);
		_moveDownP2Button.Pressed += () => OnControlsButtonPressed(_moveDownP2Button);
		_saveButton.Pressed += OnSaveButtonPressed;
		_returnButton.Pressed += OnReturnButtonPressed;
		
		LoadControlsButtons();
	}

	public override void _Input(InputEvent @event)
	{
		if (_selectedControlsButton != null)
		{
			GD.Print(@event.AsText());
			//ControlsManager.SaveControl();

			_selectedControlsButton = null;
		}
		
		base._Input(@event);
	}

	private void LoadControlsButtons()
	{
		ControlsManager.LoadControls();
		InputEvent moveUpP1InputEvent = InputMap.ActionGetEvents("move_up").First();
		InputEvent moveDownP1InputEvent = InputMap.ActionGetEvents("move_down").First();
		InputEvent moveUpP2InputEvent = InputMap.ActionGetEvents("move_up_2").First();
		InputEvent moveDownP2InputEvent = InputMap.ActionGetEvents("move_down_2").First();
		
		_moveUpP1Button.Text = moveUpP1InputEvent.AsText();
		_moveDownP1Button.Text = moveDownP1InputEvent.AsText();
		_moveUpP2Button.Text = moveUpP2InputEvent.AsText();
		_moveDownP2Button.Text = moveDownP2InputEvent.AsText();
	}
	
	private void OnControlsButtonPressed(Button selectedControlsButton)
	{
		LoadControlsButtons();
		selectedControlsButton.Text = "?";
		_selectedControlsButton = selectedControlsButton;
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
	
	private void SaveControlsSettings()
	{
		
	}
}
