using Godot;

public partial class Goal : Area2D
{
	[Export] 
	private string _side;
	
	[Signal]
	public delegate void GoalScoredEventHandler(string side);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBallEntered;
	}

	public void OnBallEntered(Node2D body)
	{
		EmitSignalGoalScored(_side);
	}
}
