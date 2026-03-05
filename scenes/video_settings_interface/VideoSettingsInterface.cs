using Godot;
using Pong.managers;

public partial class VideoSettingsInterface : Control
{
	[Export] private OptionButton _windowModeOptionButton;
	[Export] private OptionButton _resolutionOptionButton;
	[Export] private OptionButton _vsyncOptionButton;
	[Export] private HSlider _framerateSlider;
	[Export] private RichTextLabel _nbFpsLabel;
	
	public override void _Ready()
	{
		_vsyncOptionButton.ItemSelected += OnVsyncOptionButtonItemSelected;
		_framerateSlider.ValueChanged += OnFramerateValueChanged;
	}
	
	private void OnVsyncOptionButtonItemSelected(long itemSelectedIndex)
	{
		DisplayServer.VSyncMode vsyncMode = (DisplayServer.VSyncMode) itemSelectedIndex;
		if (vsyncMode != DisplayServer.VSyncMode.Disabled)
		{
			_framerateSlider.Value = 0;
		}
	}
	
	private void OnFramerateValueChanged(double value)
	{
		int nbFps = (int) value;
		
		if (_vsyncOptionButton.GetSelectedId() != (int) DisplayServer.VSyncMode.Disabled)
		{
			nbFps = 0;
			_framerateSlider.Value = nbFps;
		}
		
		_nbFpsLabel.Text = nbFps.ToString();
	}
	
	public void UpdateVideoSettings()
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
			SettingsManager.GetMultiplierResolution()
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
		_framerateSlider.Value = framerate;
		_nbFpsLabel.Text = framerate.ToString();
	}
	
	public void SaveVideoSettings()
	{
		DisplayServer.WindowSetMode((DisplayServer.WindowMode)_windowModeOptionButton.GetSelectedId());
		DisplayServer.WindowSetSize(new Vector2I(640, 360) * _resolutionOptionButton.GetSelectedId());
		
		DisplayServer.VSyncMode vSyncMode = (DisplayServer.VSyncMode) _vsyncOptionButton.GetSelectedId();
		DisplayServer.WindowSetVsyncMode(vSyncMode);
		
		int framerate = vSyncMode == DisplayServer.VSyncMode.Disabled ? (int) _framerateSlider.Value : 0;
		Engine.SetMaxFps(framerate);
		
		SettingsManager.SaveValue("Video", "WindowMode", _windowModeOptionButton.GetSelectedId());
		SettingsManager.SaveValue("Video", "Resolution", _resolutionOptionButton.GetSelectedId());
		SettingsManager.SaveValue("Video", "Vsync", _vsyncOptionButton.GetSelectedId());
		SettingsManager.SaveValue("Video", "Framerate", framerate);
		
		SettingsManager.LoadVideo();
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
