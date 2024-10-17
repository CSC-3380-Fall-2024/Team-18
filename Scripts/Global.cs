using Godot;
using System;
using System.Collections.Generic; 

/*
Summary:
This class is responsible for keeping track of important data that should always be loaded, such as the player's inventory and characterbody2d.
*/
public partial class Global : Node
{
	// Inventory array, stores all items as well as their data (quantities, types, etc.)
	static dynamic[] inventory = new dynamic[30];
	//Signal variable that fires when the inventory is updated.
	[Signal]
    public delegate void InventoryUpdatedEventHandler();
	//The player. Starts as null, and refers to the player via method when the game starts to run.
	 public static CharacterBody2D player_node = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/* 
	Summary:
	This method takes the item being either picked up or bought and adds it to the inventory.
	Params:
	Dictionary<string, dynamic> item: Takes in a dictionary with key string and a dynamic item type, key is the type of data being referenced and value is that piece of data.
	Items must have keys for item_type, item_effect, quanity, item_texture, item_name and scene_path.
	Returns:
	Returns true if it an add the item, returns false otherwise.
	*/
	public static bool Add_Item( Dictionary<string, dynamic> item){
		//Create a new GodotObject (lets it play nice with static variables.)
		GodotObject gdtobj = new();

		for(int i = 0; i < inventory.Length; i++) 
		{
			//Checks for if the current item is of the same type and has the same effect
			if ((inventory[i] != null) && (inventory[i]["item_type"] == item["item_type"]) && (inventory[i]["item_effect"] == item["item_effect"]))
			{
				//adds the amount of items to the quantity key in the item's dictionary, sends a signal and returns true.
				inventory[i]["quantity"] += item["quantity"];
				gdtobj.EmitSignal(SignalName.InventoryUpdated);
				GD.Print("quanity updated");
				return true;
				//checks for an empty spot in the inventory
			} else if (inventory[i] == null)
			{
				//Adds the item to the inventory at index i, emits a signal and returns true.
				inventory[i] = item;
				gdtobj.EmitSignal(SignalName.InventoryUpdated);
				GD.Print("new item added");
				return true;
			}
		}
		//if no spots are available, returns false.
		return false;
	}
	
	/* 
	Summary:
	This method keeps the player loaded globally to allow other methods to keep the player constantly identified.
	Params:
	CharacterBody2D player: The player character. Cannot be null or reference anything other than the player character.
	*/
		public static void Set_Player_Reference(CharacterBody2D player)
		{
			player_node = player;
		}
}

