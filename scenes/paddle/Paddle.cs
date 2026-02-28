using System;
using Godot;

public partial class Paddle : CharacterBody2D
{
	public enum ControllerType { LeftPlayer, RightPlayer, AI, OnlinePeer }
	
	[Export]
	public ControllerType Controller = ControllerType.LeftPlayer;
	
	[Export] 
	public float Speed = 400.0f;
	
	[Export] 
	public Color Color = Colors.White;
	
	[Export] 
	public long Id { get; set; }

	[Export]
	public Ball TrackedBall;
	
	public override void _Ready()
	{
		GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
		GetNode<CpuParticles2D>("CpuParticles2D").Color = Color;
		GetNode<Polygon2D>("Polygon2D").Color = Color;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Controller == ControllerType.OnlinePeer && !IsMultiplayerAuthority()) 
			return;
		
		Velocity = new Vector2(Velocity.X, GetInputDirection() * Speed);
		MoveAndSlide();
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
	
	private void OnBodyEntered(Node2D body)
	{
		if (body is Ball)
		{
			GetNode<CpuParticles2D>("CpuParticles2D").Emitting = true;
		}
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
