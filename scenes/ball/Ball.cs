using Godot;

public partial class Ball : CharacterBody2D
{
	[Export] private float _speed = 500.0f;
	[Export] public Vector2 SyncPosition { get; set; }
	[Export] public Vector2 SyncVelocity { get; set; }
	
	public override void _Ready()
	{
		if (!Multiplayer.HasMultiplayerPeer() || Multiplayer.IsServer())
		{
			Launch();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		bool isAuthority = !Multiplayer.HasMultiplayerPeer() || Multiplayer.IsServer();
		if (isAuthority)
		{
			KinematicCollision2D collision = MoveAndCollide(Velocity * _speed * (float)delta);

			if (collision != null)
			{
				Velocity = Velocity.Bounce(collision.GetNormal());
				if (collision.GetCollider() is Paddle)
				{
					Velocity *= 1.05f;
				}
			}

			if (Multiplayer.HasMultiplayerPeer())
			{
				SyncPosition = GlobalPosition;
				SyncVelocity = Velocity;
			}
		}
		else
		{
			Velocity = SyncVelocity;
			
			KinematicCollision2D collision = MoveAndCollide(Velocity * _speed * (float)delta);
			if (collision != null)
			{
				Velocity = Velocity.Bounce(collision.GetNormal());
				if (collision.GetCollider() is Paddle)
				{
					Velocity *= 1.05f;
				}
			}
			
			GlobalPosition = GlobalPosition.Lerp(SyncPosition, (float)delta * 5.0f);
		}
	}

	public void Launch()
	{
		Velocity = new Vector2(1.0f, 0.0f);
		SyncVelocity = Velocity;
	}
}
