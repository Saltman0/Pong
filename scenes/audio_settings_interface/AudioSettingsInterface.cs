using Godot;
using Pong.managers;

public partial class AudioSettingsInterface : Control
{
	[Export] private HSlider _masterSlider;
	[Export] private HSlider _musicSlider;
	[Export] private HSlider _sfxSlider;
	[Export] private HSlider _uiSlider;
	
	public void UpdateAudioSettings()
	{
		_masterSlider.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Master"));
		_musicSlider.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Music"));
		_sfxSlider.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("SFX"));
		_uiSlider.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("UI"));
	}
	
	public void SaveAudioSettings()
	{
		SettingsManager.Instance.SaveValue("Audio", "Master", (float)_masterSlider.Value);
		SettingsManager.Instance.SaveValue("Audio", "Music", (float)_musicSlider.Value);
		SettingsManager.Instance.SaveValue("Audio", "SFX", (float)_sfxSlider.Value);
		SettingsManager.Instance.SaveValue("Audio", "UI", (float)_uiSlider.Value);

		SettingsManager.Instance.LoadAudio();
	}
	
	public void ResetAudioSettings()
	{
		_masterSlider.Value = (float) SettingsManager.Instance.GetValue(
			"Audio", "Master", SettingsManager.Instance.DefaultMasterVolume
		);
		_musicSlider.Value = (float) SettingsManager.Instance.GetValue(
			"Audio", "Music", SettingsManager.Instance.DefaultMusicVolume
		);
		_sfxSlider.Value = (float) SettingsManager.Instance.GetValue(
			"Audio", "SFX", SettingsManager.Instance.DefaultSfxVolume
		);
		_uiSlider.Value = (float) SettingsManager.Instance.GetValue(
			"Audio", "UI", SettingsManager.Instance.DefaultUiVolume
		);
	}
}
