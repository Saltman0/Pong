using Godot;
using Pong.managers;

public partial class AccessibilitySettingsInterface : Control
{
	[Export] private OptionButton _colorblindModeOptionButton;
	
	public void UpdateAccessibilitySettings()
	{
		SelectOption(
			_colorblindModeOptionButton, 
			"Accessibility", 
			"ColorblindMode", 
			SettingsManager.DefaultColorblindMode
		);
	}
	
	public void SaveAccessibilitySettings()
	{
		SettingsManager.SaveValue(
			"Accessibility", 
			"ColorblindMode", 
			_colorblindModeOptionButton.GetSelectedId()
		);
		
		SettingsManager.LoadAccessibility();
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
