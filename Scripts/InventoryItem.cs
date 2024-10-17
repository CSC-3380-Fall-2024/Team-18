using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

//Allows code to run in the editor as well as in game.
[Tool]
/*
Summary:
This class is responsible for keeping track of item data, including the name, type, effect and sprite of the item, as well as whether it can be picked up by the player.
*/
public partial class InventoryItem : Node2D
{
	//The sprite for the item
	public Sprite2D icon_sprite;
	//The type of item. Can be modified in godot directly ([Export])
	[Export]
    public string item_type { get; set; } = "";
	[Export]
	public string item_name { get; set; } = "";
	[Export]
    public Godot.Texture2D item_texture { get; set; }
	//Classification regarding whether the item can be destroyed, as well as how it works when used.
	[Export]
    public string item_effect{ get; set; } = "";

	//Links to the scene for this script.
	string scene_path = "res://Scenes/Inventory_Item:tscn";

	bool player_in_range = false;
	/*
	Summary:
	Called when the node enters the scene tree for the first time.
	Updates icon_sprite to be the correct texture.
	*/
	public override void _Ready()
	{
		icon_sprite = GetNode<Sprite2D>("Sprite2D");
	
			if (!Engine.IsEditorHint())
			{
				icon_sprite.Texture = item_texture;
			}		
	}

	/*
	Summary:
	Called every frame. 
	Updates icon_sprite to be the correct texture as well as checks to see if the player is picking up an item.
	Params:
	delta: the elapsed time since the previous frame.
	*/
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
			{
				icon_sprite.Texture = item_texture;
			}
		//Checks if the player is close enough and the button to pick up an item is pressed.	
		if (player_in_range && Input.IsActionJustPressed("ui_add"))
		{
			Pickup_Item();
		}
	}
	/*
	Summary:
	Creates a new item dictionary to store in the inventory. Properties are gathered from the exported variables in Godot, as well as the scen path.
	Calls the Add_Item function in Global to add the item to the inventory.
	*/
	public void Pickup_Item()
	{
		Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
		item.Add("quantity", 1);
		item.Add("item_type", item_type);
		item.Add("item_name", item_name);
		item.Add("item_texture", item_texture);
		item.Add("item_effect", item_effect);
		item.Add("scene_path", scene_path);
		
		GD.Print("past dict");
		if(Global.player_node != null){
			bool success = Global.Add_Item(item);
			GD.Print("ran");
		}
	}
	/*
	Summary:
	Godot signal for when the player enters range. Allows the player to pick up the item, and displays a notification that the item can be picked up.
	Params:
	body: must be the Player class.
	*/
	public void OnArea2DBodyEntered(Player body)
	{
		GD.Print("ran");
		//checks for the player specifically
		if(body.IsInGroup("Player"))
		{
			GD.Print("in group player");
			player_in_range = true;
			body.interact_ui.Visible = true;
			GD.Print(body.interact_ui.Visible);
		}
	}
	/*
	Summary:
	Godot signal for when the player enters range. Disallows the player to pick up the item, and removes the notification that the item can be picked up.
	Params:
	body: must be the Player class.
	*/
	public void OnArea2DBodyExited(Player body)
	{
		if(body.IsInGroup("Player"))
		{
			player_in_range = false;
			body.interact_ui.Visible = false;
			GD.Print(body.interact_ui.Visible);
		}
	}
}
