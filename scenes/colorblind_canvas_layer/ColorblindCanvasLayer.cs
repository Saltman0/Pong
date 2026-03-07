using Godot;

public partial class ColorblindCanvasLayer : CanvasLayer
{
	[Export] private ColorRect _colorblindColorRect;
	
	public static ColorblindCanvasLayer Instance { get; private set; }
	
	public override void _Ready()
	{
		Instance = this;
	}

	public void SetColorblindMode(int colorblindMode)
	{
		ShaderMaterial colorblindShaderMaterial = (ShaderMaterial) _colorblindColorRect.Material;
		
		colorblindShaderMaterial.SetShaderParameter("mode", colorblindMode);
	}
}
