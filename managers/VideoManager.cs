using Godot;

public partial class VideoManager : Node
{
    public static VideoManager Instance { get; private set; }
    
    public override void _Ready()
    {
        Instance = this;
    }

    public void SetWindowScale(int multiplier)
    {
        DisplayServer.WindowSetSize(new Vector2I(640, 360) * multiplier);
    }

    public void SetWindowMode(DisplayServer.WindowMode windowMode)
    {
        DisplayServer.WindowSetMode(windowMode);
    }
    
    public void SetVsync(DisplayServer.VSyncMode vsyncMode)
    {
        DisplayServer.WindowSetVsyncMode(vsyncMode);
    }
}