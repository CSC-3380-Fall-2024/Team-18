using Godot;
using System;
using System.Collections;

public partial class ItemDoor : Node2D
{
	public Sprite2D icon_sprite;

	bool player_in_range = false;

	public Global glbl;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		icon_sprite = GetNode<Sprite2D>("Sprite2D");
		glbl = GetNode<Global>("/root/Global");
	
			
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player_in_range && Input.IsActionJustPressed("open") && glbl.door == true)
		{
			QueueFree();
		}
	}

	public void OnArea2DBodyEntered(Player body)
	{
		GD.Print("Body Entered Type: ", body.GetType());
		if(body is Player player && body.IsInGroup("Player"))
		{
			GD.Print("yes");
			player_in_range = true;
			
		}
		
	}

	public void OnArea2DBodyExited(Player body)
	{
		if(body is Player player && body.IsInGroup("Player"))
		{
			player_in_range = false;
		
		}
	}


}
