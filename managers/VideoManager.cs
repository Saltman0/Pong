namespace Pong.managers;

using Godot;

public static class VideoManager
{
    public static void SetWindowScale(int multiplier)
    {
        DisplayServer.WindowSetSize(new Vector2I(640, 360) * multiplier);
    }

    public static void SetWindowMode(DisplayServer.WindowMode windowMode)
    {
        DisplayServer.WindowSetMode(windowMode);
    }
    
    public static void SetVsync(DisplayServer.VSyncMode vsyncMode)
    {
        DisplayServer.WindowSetVsyncMode(vsyncMode);
    }
}