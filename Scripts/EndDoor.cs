using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EndDoor : Node2D
{
	public Sprite2D icon_sprite;

	bool player_in_range = false;

	[Export]
	public int door_number { get; set; } = 0;

	public Global glbl;
	private Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		icon_sprite = GetNode<Sprite2D>("Sprite2D");
		glbl = GetNode<Global>("/root/Global");
		
	
			
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (glbl.door == true && player_in_range && Input.IsActionJustPressed("open") )
		{
			
			//Need to be able to cease player movement and hide the UI when game ends, but player not under same branch
			player = GetTree().Root.GetNode<Player>("Main/Player");
			player.interact_ui.Visible = false;
			player.EnableMovement = false;
			
			//End scene loaded here
			PackedScene endScene = GD.Load<PackedScene>("res://Scenes/end_screen.tscn");
			EndScreen EndingTest = endScene.Instantiate<EndScreen>();
			glbl.door = false;
			AddChild(EndingTest);
		}
	}

	
	public void OnArea2DBodyEntered(Node2D body)
	{
	if (body.IsInGroup("Player") && body is Player player)
	{
		player_in_range = true;

		if (glbl.door == false)
		{
			player.interact_text.Text = "NEED KEY TO END GAME.";
		}
		else
		{
			player.interact_text.Text = "Press O to End Game.";
		}

		player.interact_ui.Visible = true;
		GD.Print("yes");
	}
}

	public void OnArea2DBodyExited(Node2D body)
	{
		if(body.IsInGroup("Player") && body is Player player)
		{
			player_in_range = false;
			player.interact_ui.Visible = false;
		
		}
	}


}
