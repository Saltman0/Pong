using Godot;

public partial class SceneManager : Node
{
	public Node CurrentScene;
	
	public static SceneManager Instance { get; private set; }
	
	public override void _Ready()
	{
		Instance = this;
	}

	public void SwitchScene(Node newScene)
	{
		if (CurrentScene != null)
		{
			CurrentScene.QueueFree();
		}

		CurrentScene = newScene;
		
		GetNode<Main>("/root/Main").AddChild(CurrentScene);
	}
}
