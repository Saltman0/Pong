namespace Pong.managers;

using Godot;

public partial class AudioManager : Node
{
	public static AudioStreamPlayer MusicStreamPlayer;
	
	public static AudioManager Instance { get; private set; }
	
	public override void _Ready()
	{
		Instance = this;
	}
	
	public void PlayMusic(AudioStream musicAudioStream)
	{
		MusicStreamPlayer = new AudioStreamPlayer();
		MusicStreamPlayer.Name = "GlobalMusicPlayer";
		MusicStreamPlayer.Bus = "Music";
		MusicStreamPlayer.Stream = musicAudioStream;
		AddChild(MusicStreamPlayer);
		
		MusicStreamPlayer.Play();
	}
	
	public void PlaySfx(AudioStream sfxAudioStream)
	{
		AudioStreamPlayer audioStreamPlayer = new AudioStreamPlayer();
		audioStreamPlayer.Name = "SfxPlayer";
		audioStreamPlayer.Bus = "SFX";
		audioStreamPlayer.Stream = sfxAudioStream;
		AddChild(audioStreamPlayer);
		
		audioStreamPlayer.Play();
	}
}
