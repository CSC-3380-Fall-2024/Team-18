using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ItemDoor : Node2D
{
	public Sprite2D icon_sprite;

	bool player_in_range = false;

	[Export]
	public int door_number { get; set; } = 0;

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
		if (player_in_range && Input.IsActionJustPressed("open") )
		{
			KeyCheck();
		}
	}

	public void KeyCheck(){
		Dictionary<string, dynamic> door = new Dictionary<string, dynamic>();
		door.Add("door_number", door_number);
		if(glbl.DoorOpen(door) == true){
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
