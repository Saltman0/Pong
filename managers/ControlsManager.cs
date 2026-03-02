namespace Pong.managers;

using Godot;

public static class ControlsManager
{
	private const string SavePath = "user://settings.cfg";

	public static void SaveControl(string actionName, InputEvent inputEvent)
	{
		ConfigFile config = new ConfigFile();
		config.Load(SavePath);
		config.SetValue("Controls", actionName, inputEvent);
		config.Save(SavePath);
	}
	
	public static void LoadControls()
	{
		ConfigFile config = new ConfigFile();
		if (config.Load(SavePath) != Error.Ok) return;
		
		foreach (string action in config.GetSectionKeys("Controls"))
		{
			InputEvent inputEvent = (InputEvent)config.GetValue("Controls", action);
			InputMap.ActionEraseEvents(action);
			InputMap.ActionAddEvent(action, inputEvent);
		}
	}
}
