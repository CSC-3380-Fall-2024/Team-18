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
	[Export]
	public int item_price{get; set;} = 0;
	[Export]
	public int equip_effect{get; set;} = 0;

	//Global Reference
	public Global glbl;
	bool player_in_range = false;
	/*
	Summary:
	Called when the node enters the scene tree for the first time.
	Updates icon_sprite to be the correct texture.
	*/
	public override void _Ready()
	{
		icon_sprite = GetNode<Sprite2D>("Sprite2D");
		glbl = GetNode<Global>("/root/Global");
	
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
			PickupItem();
		}
	}
	/*
	Summary:
	Creates a new item dictionary to store in the inventory. Properties are gathered from the exported variables in Godot, as well as the scen path.
	Calls the Add_Item function in Global to add the item to the inventory.
	Deletes the item.
	*/
	public void PickupItem()
	{
		Dictionary<string, dynamic> item = new Dictionary<string, dynamic>
		{
			{ "quantity", 1 },
			{ "item_type", item_type },
			{ "item_name", item_name },
			{ "item_texture", item_texture },
			{ "item_effect", item_effect },
			{ "item_price", item_price },
			{ "equip_effect", equip_effect }
		};
		
		if(glbl.player_node != null){
			bool success = glbl.AddItem(item);
			if(success)
			{
				//Item is deleted from scene here.
				QueueFree();
			}
		}
	}
	/*
	Summary:
	Godot signal for when the player enters range. Allows the player to pick up the item, and displays a notification that the item can be picked up.
	Params:
	body: must be the Player class.
	*/
	public void OnArea2DBodyEntered(Node2D body)
	{
		//checks for the player specifically
		if(body.IsInGroup("Player") && body is Player player)
		{
			player_in_range = true;
			player.interact_text.Text = "Press E to Pick Up.";
			player.interact_ui.Visible = true;
		}
	}
	/*
	Summary:
	Godot signal for when the player enters range. Disallows the player to pick up the item, and removes the notification that the item can be picked up.
	Params:
	body: must be the Player class.
	*/
	public void OnArea2DBodyExited(Node2D body)
	{
		if(body.IsInGroup("Player") && body is Player player)
		{
			player_in_range = false;
			player.interact_ui.Visible = false;
		}
	}
}
