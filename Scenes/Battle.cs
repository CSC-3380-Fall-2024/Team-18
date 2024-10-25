using Godot;
using System;

[Signal]
public delegate void TextClosed();

public partial class Battle : Control
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		AddUserSignal(nameof(TextClosed));
		
		GetNode<Control>("Textbox").Hide();
		GetNode<Panel>("Actions").Hide();
		
		display_text("A wild enemy appears.");
		await ToSignal(this, nameof(TextClosed));
		GD.Print("Fuck1");
		GetNode<Panel>("Actions").Show();
	}
	public override void _Input(InputEvent @event)
	{
		if ((Input.IsActionJustPressed("ui_accept") || Input.IsMouseButtonPressed(MouseButton.Left)) && (GetNode<Control>("Textbox").Visible == true))
		{
			GetNode<Control>("Textbox").Hide();
			EmitSignal(nameof(TextClosed));
		}
	}
	public void display_text(String text)
	{
		GetNode<Control>("Textbox").Show();
		GetNode<Label>("Textbox/Label").Text = text;
	}

	public async void  _on_run_pressed()
	{
		display_text("Escaped Successfully!");
		await ToSignal(this, nameof(TextClosed));
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
