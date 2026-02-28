using System;
using Godot;

public partial class Paddle : CharacterBody2D
{
	public enum ControllerType { LeftPlayer, RightPlayer, AI, OnlinePeer }
	
	[Export] public Vector2 SyncPosition { get; set; }
	[Export] public ControllerType Controller = ControllerType.LeftPlayer;
	[Export] public float Speed = 400.0f;
	[Export] public Color Color = Colors.White;
	[Export] public long Id { get; set; }
	[Export] public Ball TrackedBall;
	
	public override void _Ready()
	{
		GetNode<Polygon2D>("Polygon2D").Color = Color;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Controller == ControllerType.OnlinePeer)
		{
			if (IsMultiplayerAuthority())
			{
				Velocity = new Vector2(Velocity.X, GetInputDirection() * Speed);
				MoveAndSlide();
        
				SyncPosition = GlobalPosition;
			}
			else
			{
				GlobalPosition = GlobalPosition.Lerp(SyncPosition, (float)delta * 25.0f);
			}
		}
		else
		{
			Velocity = new Vector2(Velocity.X, GetInputDirection() * Speed);
			MoveAndSlide();
		}
	}

	public void SetProperties(string name, long id, ControllerType controllerType)
	{
		Name = name;
		Id = id;
		Controller = controllerType;

		if (Controller ==  ControllerType.OnlinePeer)
		{
			SetMultiplayerAuthority((int)Id);
		}
		
		GD.PushError("SetProperties Paddle : " + Name + " - " + Id + " - " + Controller);
	}
	
	private float GetInputDirection()
	{
		float inputDirection = 0.0f;
		
		switch (Controller)
		{
			case ControllerType.LeftPlayer:
				inputDirection = Input.GetAxis("move_up", "move_down");
				break;
			case ControllerType.RightPlayer:
				inputDirection = Input.GetAxis("move_up_2", "move_down_2");
				break;
			case ControllerType.AI:
				inputDirection = FollowBall();
				break;
			case ControllerType.OnlinePeer:
				if (IsMultiplayerAuthority())
				{
					inputDirection = Input.GetAxis("move_up", "move_down");
				}
				break;
		}

		return inputDirection;
	}
	
	private float FollowBall()
	{
		float diff = TrackedBall.Position.Y - Position.Y;
		float deadzone = 5.0f;

		return Math.Abs(diff) > deadzone ? Math.Sign(diff) : 0.0f;
	}
}
