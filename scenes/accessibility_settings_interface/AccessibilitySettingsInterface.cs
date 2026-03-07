using Godot;
using Pong.managers;

public partial class AccessibilitySettingsInterface : Control
{
	[Export] private OptionButton _colorblindModeOptionButton;
	
	public void UpdateAccessibilitySettings()
	{
		_colorblindModeOptionButton.Selected = (int) SettingsManager.Instance.GetValue(
			"Accessibility", "ColorblindMode", SettingsManager.Instance.DefaultColorblindMode
		);
	}
	
	public void SaveAccessibilitySettings()
	{
		SettingsManager.Instance.SaveValue(
			"Accessibility", 
			"ColorblindMode", 
			_colorblindModeOptionButton.GetSelectedId()
		);
		
		SettingsManager.Instance.LoadAccessibility();
	}
	
	public void ResetAccesibilitySettings()
	{
		_colorblindModeOptionButton.Selected = (int) SettingsManager.Instance.GetValue(
			"Accessibility", "ColorblindMode", SettingsManager.Instance.DefaultColorblindMode
		);
	}
}
