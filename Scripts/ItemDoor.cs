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
		if (glbl.door == true && player_in_range && Input.IsActionJustPressed("open") )
		{
			glbl.door = false;
			QueueFree();
		}
	}

	//public void KeyCheck(){
		//Dictionary<string, dynamic> door = new Dictionary<string, dynamic>();
		//door.Add("door_number", door_number);
		//if(glbl.DoorOpen(door) == true){
			//QueueFree();
		//}
	//}

public void OnArea2DBodyEntered(Node2D body)
	{
	if (body.IsInGroup("Player") && body is Player player)
	{
		player_in_range = true;

		if (glbl.door == false)
		{
			player.interact_text.Text = "NEED KEY TO OPEN DOOR.";
		}
		else
		{
			player.interact_text.Text = "Press O to Open Door.";
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
