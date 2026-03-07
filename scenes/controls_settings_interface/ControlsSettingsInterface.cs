using Godot;
using Pong.managers;

public partial class ControlsSettingsInterface : Control
{
	[Export] private Button _moveUpP1Button;
	[Export] private Button _moveDownP1Button;
	[Export] private Button _moveUpP2Button;
	[Export] private Button _moveDownP2Button;
	private Button _selectedControlsButton;
	
	private InputEvent _moveUpP1InputEvent = (InputEvent) SettingsManager.Instance.GetValue(
		"Controls", "move_up", SettingsManager.Instance.DefaultMoveUpP1InputEvent
	);
	private InputEvent _moveDownP1InputEvent = (InputEvent) SettingsManager.Instance.GetValue(
		"Controls", "move_down", SettingsManager.Instance.DefaultMoveDownP1InputEvent
	);
	private InputEvent _moveUpP2InputEvent = (InputEvent) SettingsManager.Instance.GetValue(
		"Controls", "move_up_2", SettingsManager.Instance.DefaultMoveUpP2InputEvent
	);
	private InputEvent _moveDownP2InputEvent = (InputEvent) SettingsManager.Instance.GetValue(
		"Controls", "move_down_2", SettingsManager.Instance.DefaultMoveDownP2InputEvent
	);
	
	public override void _Ready()
	{
		_moveUpP1Button.Pressed += () => OnControlsButtonPressed(_moveUpP1Button);
		_moveDownP1Button.Pressed += () => OnControlsButtonPressed(_moveDownP1Button);
		_moveUpP2Button.Pressed += () => OnControlsButtonPressed(_moveUpP2Button);
		_moveDownP2Button.Pressed += () => OnControlsButtonPressed(_moveDownP2Button);
	}

	public override void _Input(InputEvent @event)
	{
		if (_selectedControlsButton == null || @event is not InputEventKey)
		{
			return;
		}
		
		if (_selectedControlsButton.Name == _moveUpP1Button.Name)
		{
			_moveUpP1InputEvent = @event;
		} else if (_selectedControlsButton.Name == _moveDownP1Button.Name)
		{
			_moveDownP1InputEvent = @event;
		} else if (_selectedControlsButton.Name == _moveUpP2Button.Name)
		{
			_moveUpP2InputEvent = @event;
		} else if (_selectedControlsButton.Name == _moveDownP2Button.Name)
		{
			_moveDownP2InputEvent = @event;
		}
			
		_selectedControlsButton.Text = @event.AsText();
		
		_selectedControlsButton = null;
	}
	
	private void OnControlsButtonPressed(Button selectedControlsButton)
	{
		UpdateControlSettings();
		selectedControlsButton.Text = "?";
		_selectedControlsButton = selectedControlsButton;
	}
	
	public void UpdateControlSettings()
	{
		_moveUpP1Button.Text = _moveUpP1InputEvent.AsText();
		_moveDownP1Button.Text = _moveDownP1InputEvent.AsText();
		_moveUpP2Button.Text = _moveUpP2InputEvent.AsText();
		_moveDownP2Button.Text = _moveDownP2InputEvent.AsText();
	}
	
	public void SaveControlsSettings()
	{
		SettingsManager.Instance.SaveValue("Controls", "move_up", _moveUpP1InputEvent);
		SettingsManager.Instance.SaveValue("Controls", "move_down", _moveDownP1InputEvent);
		SettingsManager.Instance.SaveValue("Controls", "move_up_2", _moveUpP2InputEvent);
		SettingsManager.Instance.SaveValue("Controls", "move_down_2", _moveDownP2InputEvent);

		SettingsManager.Instance.LoadControls();
	}
	
	public void ResetControlsSettings()
	{
		_moveUpP1InputEvent = (InputEvent) SettingsManager.Instance.GetValue(
			"Controls", "move_up", SettingsManager.Instance.DefaultMoveUpP1InputEvent
		);
		_moveDownP1InputEvent = (InputEvent) SettingsManager.Instance.GetValue(
			"Controls", "move_down", SettingsManager.Instance.DefaultMoveDownP1InputEvent
		);
		_moveUpP2InputEvent = (InputEvent) SettingsManager.Instance.GetValue(
			"Controls", "move_up_2", SettingsManager.Instance.DefaultMoveUpP2InputEvent
		);
		_moveDownP2InputEvent = (InputEvent) SettingsManager.Instance.GetValue(
			"Controls", "move_down_2", SettingsManager.Instance.DefaultMoveDownP2InputEvent
		);

		_moveUpP1Button.Text = _moveUpP1InputEvent.AsText();
		_moveDownP1Button.Text = _moveDownP1InputEvent.AsText();
		_moveUpP2Button.Text = _moveUpP2InputEvent.AsText();
		_moveDownP2Button.Text = _moveDownP2InputEvent.AsText();
	}
}
