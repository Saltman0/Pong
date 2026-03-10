using Godot;
using Pong.managers;

public partial class GeneralSettingsInterface : Control
{
	[Export] private OptionButton _languageOptionButton;
	
	public void UpdateGeneralSettings()
	{
		_languageOptionButton.Selected = (int) SettingsManager.Instance.GetValue(
			"General", "Language", SettingsManager.Instance.DefaultLanguage
		);
	}
	
	public void SaveGeneralSettings()
	{
		SettingsManager.Instance.SaveValue(
			"General", 
			"Language", 
			_languageOptionButton.Selected
		);
		
		SettingsManager.Instance.LoadGeneral();
	}
	
	public void ResetGeneralSettings()
	{
		_languageOptionButton.Selected = (int) SettingsManager.Instance.GetValue(
			"General", "Language", SettingsManager.Instance.DefaultLanguage
		);
	}
}
