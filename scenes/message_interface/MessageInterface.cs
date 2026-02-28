using Godot;

public partial class MessageInterface : VBoxContainer
{
	[Signal]
	public delegate void MessageClosedEventHandler();
	
	public override void _Ready()
	{
		GetNode<Button>("OkButton").Pressed += () => { EmitSignalMessageClosed(); };
	}
	
	public void SetMessageText(string message)
	{
		GetNode<RichTextLabel>("MessageText").Text = message;
	}
	
	public void SetVisibleOkButton(bool visible)
	{
		GetNode<Button>("OkButton").Visible = visible;
	}
}
