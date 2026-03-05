using System.Linq;
using Godot;

namespace Pong.managers;

public static class SettingsManager
{
    private const string SavePath = "user://settings.cfg";

    private const int DefaultMaxFps = 60;
    
    private static readonly Vector2I DefaultResolution = new Vector2I(640, 360);
    
    public static bool LoadControls()
    {
        ConfigFile config = new ConfigFile();
        if (config.Load(SavePath) != Error.Ok || !config.HasSection("Controls")) return false;
        
        foreach (string action in config.GetSectionKeys("Controls"))
        {
            InputEvent inputEvent = (InputEvent)config.GetValue("Controls", action);
            InputMap.ActionEraseEvents(action);
            InputMap.ActionAddEvent(action, inputEvent);
        }

        return true;
    }
    
    public static void SaveDefaultControls()
    {
        InputEvent moveUpP1InputEvent = InputMap.ActionGetEvents("move_up").First();
        InputEvent moveDownP1InputEvent = InputMap.ActionGetEvents("move_down").First();
        InputEvent moveUpP2InputEvent = InputMap.ActionGetEvents("move_up_2").First();
        InputEvent moveDownP2InputEvent = InputMap.ActionGetEvents("move_down_2").First();
		
        SaveValue("Controls", "move_up", moveUpP1InputEvent);
        SaveValue("Controls", "move_down", moveDownP1InputEvent);
        SaveValue("Controls", "move_up_2", moveUpP2InputEvent);
        SaveValue("Controls", "move_down_2", moveDownP2InputEvent);
    }
    
    public static bool LoadAudio()
    {
        ConfigFile config = new ConfigFile();
        if (config.Load(SavePath) != Error.Ok || !config.HasSection("Audio")) return false;
		
        foreach (string busName in config.GetSectionKeys("Audio"))
        {
            float volume = (float)config.GetValue("Audio", busName);
            AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex(busName), volume);
        }

        return true;
    }
    
    public static void SaveDefaultAudio()
    {
        SaveValue("Audio", "Master", 1.0f);
        SaveValue("Audio", "Music", 1.0f);
        SaveValue("Audio", "SFX", 1.0f);
        SaveValue("Audio", "UI", 1.0f);
    }
    
    public static bool LoadVideo()
    {
        ConfigFile config = new ConfigFile();
        if (config.Load(SavePath) != Error.Ok || !config.HasSection("Video")) return false;
        
        foreach (string video in config.GetSectionKeys("Video"))
        {
            DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.MaximizeDisabled, false);
            DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.ResizeDisabled, false);

            int value = (int)config.GetValue("Video", video);
            switch (video)
            {
                case "WindowMode":
                    DisplayServer.WindowSetMode((DisplayServer.WindowMode) value);
                    break;
                case "Resolution":
                    DisplayServer.WindowSetSize(DefaultResolution * value);
                    break;
                case "Vsync":
                    DisplayServer.WindowSetVsyncMode((DisplayServer.VSyncMode) value);
                    break;
                case "Framerate":
                    Engine.SetMaxFps(value);
                    break;
            }
        }
        
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.MaximizeDisabled, true);
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.ResizeDisabled, true);

        return true;
    }
    
    public static void SaveDefaultVideo()
    {
        Vector2I screenSize = DisplayServer.ScreenGetSize(DisplayServer.WindowGetCurrentScreen());
        
        int multiplier = screenSize.X / DefaultResolution.X;
        
        SaveValue("Video", "WindowMode", (int) DisplayServer.WindowMode.ExclusiveFullscreen);
        SaveValue("Video", "Resolution", multiplier);
        SaveValue("Video", "Vsync", (int) DisplayServer.VSyncMode.Enabled);
        SaveValue("Video", "Framerate", DefaultMaxFps);
    }

    public static void SaveValue(string section, string key, Variant value)
    {
        ConfigFile config = new ConfigFile();
        config.Load(SavePath);
        config.SetValue(section, key, value);
        config.Save(SavePath);
    }
    
    public static Variant GetValue(string section, string key, Variant defaultValue)
    {
        ConfigFile config = new ConfigFile();
        if (config.Load(SavePath) != Error.Ok || !config.HasSection(section)) return defaultValue;

        return config.GetValue(section, key, defaultValue);
    }
}