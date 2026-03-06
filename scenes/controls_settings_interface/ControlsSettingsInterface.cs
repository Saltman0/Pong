using System.Collections.Generic;
using System.Linq;
using Godot;
using Pong.managers;

public partial class ControlsSettingsInterface : Control
{
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
	
	public void UpdateControlSettings()
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
	
	public void SaveControlsSettings()
	{
		SettingsManager.Instance.SaveValue("Controls", "move_up", SettingsManager.Instance.DefaultMoveUpP1InputEvent);
		SettingsManager.Instance.SaveValue("Controls", "move_down", SettingsManager.Instance.DefaultMoveDownP1InputEvent);
		SettingsManager.Instance.SaveValue("Controls", "move_up_2", SettingsManager.Instance.DefaultMoveUpP2InputEvent);
		SettingsManager.Instance.SaveValue("Controls", "move_down_2", SettingsManager.Instance.DefaultMoveDownP2InputEvent);

		SettingsManager.Instance.LoadControls();
	}
}
