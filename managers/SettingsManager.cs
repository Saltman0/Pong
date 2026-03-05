using System.Linq;
using Godot;

namespace Pong.managers;

public static class SettingsManager
{
    /** Default path **/
    private const string SavePath = "user://settings.cfg";

    /** Default video values **/
    public static readonly DisplayServer.WindowMode DefaultWindowMode = DisplayServer.WindowMode.ExclusiveFullscreen;
    public static readonly DisplayServer.VSyncMode DefaultVsync = DisplayServer.VSyncMode.Enabled;
    public static readonly int DefaultFramerate = 0;
    public static readonly Vector2I DefaultResolution = new Vector2I(640, 360);
    
    /** Default audio values **/
    public static readonly float DefaultMasterVolume = 1.0f;
    public static readonly float DefaultMusicVolume = 1.0f;
    public static readonly float DefaultSfxVolume = 1.0f;
    public static readonly float DefaultUiVolume = 1.0f;
    
    /** Default control values **/
    public static readonly InputEvent DefaultMoveUpP1InputEvent = InputMap.ActionGetEvents("move_up").First();
    public static readonly InputEvent DefaultMoveDownP1InputEvent = InputMap.ActionGetEvents("move_down").First();
    public static readonly InputEvent DefaultMoveUpP2InputEvent = InputMap.ActionGetEvents("move_up_2").First();
    public static readonly InputEvent DefaultMoveDownP2InputEvent = InputMap.ActionGetEvents("move_down_2").First();
    
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
        SaveValue("Controls", "move_up", DefaultMoveUpP1InputEvent);
        SaveValue("Controls", "move_down", DefaultMoveDownP1InputEvent);
        SaveValue("Controls", "move_up_2", DefaultMoveUpP2InputEvent);
        SaveValue("Controls", "move_down_2", DefaultMoveDownP2InputEvent);
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
        SaveValue("Audio", "Master", DefaultMasterVolume);
        SaveValue("Audio", "Music", DefaultMusicVolume);
        SaveValue("Audio", "SFX", DefaultSfxVolume);
        SaveValue("Audio", "UI", DefaultUiVolume);
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

    public static int GetMultiplierResolution()
    {
        Vector2I screenSize = DisplayServer.ScreenGetSize(DisplayServer.WindowGetCurrentScreen());
        
        return screenSize.X / DefaultResolution.X;
    }
    
    public static void SaveDefaultVideo()
    {
        SaveValue("Video", "WindowMode", (int) DefaultWindowMode);
        SaveValue("Video", "Resolution", GetMultiplierResolution());
        SaveValue("Video", "Vsync", (int) DefaultVsync);
        SaveValue("Video", "Framerate", DefaultFramerate);
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