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
		if (MusicStreamPlayer != null)
		{
			MusicStreamPlayer.Stop();
			MusicStreamPlayer.QueueFree();
		}
		
		MusicStreamPlayer = new AudioStreamPlayer();
		MusicStreamPlayer.Name = "GlobalMusicPlayer";
		MusicStreamPlayer.Bus = "Music";
		MusicStreamPlayer.Stream = musicAudioStream;
		AddChild(MusicStreamPlayer);
		
		MusicStreamPlayer.Play();
	}
}
