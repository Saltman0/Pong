using Godot;

public partial class Wall : StaticBody2D
{
	public override void _Ready()
	{
		GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Ball)
		{
			GetNode<CpuParticles2D>("CpuParticles2D").SetEmitting(true);
		}
	}
}
