using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Pong.managers;

public partial class SettingsManager : Node
{
    /** Default path **/
    private const string SavePath = "user://settings.cfg";

    /** Cached config file to avoid redundant disk I/O **/
    private ConfigFile _cachedConfig;

    /** Default general values **/
    public readonly Dictionary<int, string> LanguagesDictionary = new Dictionary<int, string>();
    public readonly int DefaultLanguage = 0;

    /** Default video values **/
    public readonly DisplayServer.WindowMode DefaultWindowMode = DisplayServer.WindowMode.ExclusiveFullscreen;
    public readonly DisplayServer.VSyncMode DefaultVsync = DisplayServer.VSyncMode.Enabled;
    public readonly int DefaultFramerate = 0;
    public readonly Vector2I DefaultResolution = new Vector2I(640, 360);
    
    /** Default audio values **/
    public readonly float DefaultMasterVolume = 1.0f;
    public readonly float DefaultMusicVolume = 1.0f;
    public readonly float DefaultSfxVolume = 1.0f;
    public readonly float DefaultUiVolume = 1.0f;
    
    /** Default control values **/
    public readonly InputEvent DefaultMoveUpP1InputEvent = InputMap.ActionGetEvents("move_up").First();
    public readonly InputEvent DefaultMoveDownP1InputEvent = InputMap.ActionGetEvents("move_down").First();
    public readonly InputEvent DefaultMoveUpP2InputEvent = InputMap.ActionGetEvents("move_up_2").First();
    public readonly InputEvent DefaultMoveDownP2InputEvent = InputMap.ActionGetEvents("move_down_2").First();
    
    /** Default accessibility values **/
    public readonly int DefaultColorblindMode = 0;
    
    public static SettingsManager Instance { get; private set; }
	
    public override void _Ready()
    {
        LanguagesDictionary.Add(0, "en");
        LanguagesDictionary.Add(1, "fr");
        LanguagesDictionary.Add(2, "es");
        Instance = this;
        LoadConfigFromDisk();
    }

    private ConfigFile LoadConfigFromDisk()
    {
        _cachedConfig = new ConfigFile();
        _cachedConfig.Load(SavePath);
        return _cachedConfig;
    }
    
    public bool LoadGeneral()
    {
        if (!_cachedConfig.HasSection("General")) return false;
        
        int selectedLanguage = (int) _cachedConfig.GetValue("General", "Language", DefaultLanguage);
        TranslationServer.SetLocale(LanguagesDictionary[selectedLanguage]);

        return true;
    }
    
    public void SaveDefaultGeneral()
    {
        SaveValue("General", "Language", DefaultLanguage);
    }
    
    public bool LoadControls()
    {
        if (!_cachedConfig.HasSection("Controls")) return false;
        
        foreach (string action in _cachedConfig.GetSectionKeys("Controls"))
        {
            InputEvent inputEvent = (InputEvent)_cachedConfig.GetValue("Controls", action);
            InputMap.ActionEraseEvents(action);
            InputMap.ActionAddEvent(action, inputEvent);
        }

        return true;
    }
    
    public void SaveDefaultControls()
    {
        SaveValue("Controls", "move_up", DefaultMoveUpP1InputEvent);
        SaveValue("Controls", "move_down", DefaultMoveDownP1InputEvent);
        SaveValue("Controls", "move_up_2", DefaultMoveUpP2InputEvent);
        SaveValue("Controls", "move_down_2", DefaultMoveDownP2InputEvent);
    }
    
    public bool LoadAudio()
    {
        if (!_cachedConfig.HasSection("Audio")) return false;
		
        foreach (string busName in _cachedConfig.GetSectionKeys("Audio"))
        {
            float volume = (float)_cachedConfig.GetValue("Audio", busName);
            AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex(busName), volume);
        }

        return true;
    }
    
    public void SaveDefaultAudio()
    {
        SaveValue("Audio", "Master", DefaultMasterVolume);
        SaveValue("Audio", "Music", DefaultMusicVolume);
        SaveValue("Audio", "SFX", DefaultSfxVolume);
        SaveValue("Audio", "UI", DefaultUiVolume);
    }
    
    public bool LoadVideo()
    {
        if (!_cachedConfig.HasSection("Video")) return false;
        
        foreach (string video in _cachedConfig.GetSectionKeys("Video"))
        {
            int value = (int)_cachedConfig.GetValue("Video", video);
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

        return true;
    }

    public int GetMultiplierResolution()
    {
        Vector2I screenSize = DisplayServer.ScreenGetSize(DisplayServer.WindowGetCurrentScreen());
        
        return screenSize.X / DefaultResolution.X;
    }
    
    public void SaveDefaultVideo()
    {
        SaveValue("Video", "WindowMode", (int) DefaultWindowMode);
        SaveValue("Video", "Resolution", GetMultiplierResolution());
        SaveValue("Video", "Vsync", (int) DefaultVsync);
        SaveValue("Video", "Framerate", DefaultFramerate);
    }
    
    public bool LoadAccessibility()
    {
        if (!_cachedConfig.HasSection("Accessibility")) return false;

        ColorblindCanvasLayer.Instance.SetColorblindMode(
            (int) _cachedConfig.GetValue("Accessibility", "ColorblindMode", DefaultColorblindMode)
        );

        return true;
    }
    
    public void SaveDefaultAccessibility()
    {
        SaveValue("Accessibility", "ColorblindMode", DefaultColorblindMode);
    }

    public void SaveValue(string section, string key, Variant value)
    {
        _cachedConfig.SetValue(section, key, value);
        _cachedConfig.Save(SavePath);
    }
    
    public Variant GetValue(string section, string key, Variant defaultValue)
    {
        if (!_cachedConfig.HasSection(section)) return defaultValue;

        return _cachedConfig.GetValue(section, key, defaultValue);
    }
}
