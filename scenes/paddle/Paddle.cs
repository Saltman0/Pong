using Godot;

public partial class Paddle : CharacterBody2D
{
	[Export]
	public float Speed = 400.0f;
	
	[Export] 
	public Color Color;

	public float Direction;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
		GetNode<CpuParticles2D>("CpuParticles2D").Color = Color;
		GetNode<Polygon2D>("Polygon2D").Color = Color;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Ball)
		{
			GetNode<CpuParticles2D>("CpuParticles2D").SetEmitting(true);
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Velocity = new Vector2(Velocity.X, Direction * Speed);
		MoveAndSlide();
	}
}
