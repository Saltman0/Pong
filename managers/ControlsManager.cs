using System.Linq;

namespace Pong.managers;

using Godot;

public static class ControlsManager
{
	private const string SavePath = "user://settings.cfg";
	
	public static void SaveDefaultControls()
	{
		InputEvent moveUpP1InputEvent = InputMap.ActionGetEvents("move_up").First();
		InputEvent moveDownP1InputEvent = InputMap.ActionGetEvents("move_down").First();
		InputEvent moveUpP2InputEvent = InputMap.ActionGetEvents("move_up_2").First();
		InputEvent moveDownP2InputEvent = InputMap.ActionGetEvents("move_down_2").First();
		
		SaveControl("move_up", moveUpP1InputEvent);
		SaveControl("move_down", moveDownP1InputEvent);
		SaveControl("move_up_2", moveUpP2InputEvent);
		SaveControl("move_down_2", moveDownP2InputEvent);
	}

	public static void SaveControl(string actionName, InputEvent inputEvent)
	{
		ConfigFile config = new ConfigFile();
		config.Load(SavePath);
		config.SetValue("Controls", actionName, inputEvent);
		config.Save(SavePath);
	}
	
	public static bool LoadControls()
	{
		ConfigFile config = new ConfigFile();
		if (config.Load(SavePath) != Error.Ok) return false;
		
		foreach (string action in config.GetSectionKeys("Controls"))
		{
			InputEvent inputEvent = (InputEvent)config.GetValue("Controls", action);
			InputMap.ActionEraseEvents(action);
			InputMap.ActionAddEvent(action, inputEvent);
		}

		return true;
	}
}
