using Godot;
using System;
using System.Collections;

public partial class ItemDoor : Node2D
{

	bool player_in_range = false;

	public Global glbl;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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

	public void OnArea2DBodyEntered(Node2D body)
	{
		GD.Print("Body Entered Type: ", body.GetType());
		if(body.IsInGroup("Player") && body is Player player)
		{
			GD.Print("yes");
			player_in_range = true;
			
		}
		
	}

	public void OnArea2DBodyExited(Node2D body)
	{
		if(body.IsInGroup("Player") && body is Player player)
		{
			player_in_range = false;
		
		}
	}


}
