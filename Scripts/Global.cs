using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/*
Summary:
This class is responsible for keeping track of important data that should always be loaded, such as the player's inventory and characterbody2d.
This is a SINGLETON. In order to reference anything in this class, please do the following:
Create a variable of type Global (I use glbl as the variable name)
In _Ready, add "glbl = GetNode<Global>("/root/Global");"
This will create a reference to the already loaded Global Script.
*/

public partial class Global : Node
{
	// Inventory array, stores all items as well as their data (quantities, types, etc.)
	public dynamic[] inventory = new dynamic[30];
	public dynamic[] quests = new dynamic[10];
	public dynamic[] shop = new dynamic[10];
	public int money = 1000;
	//Signal library; uses the CustomSignals script.
	public CustomSignals custom_signals;
	//The player. Starts as null, and refers to the player via method when the game starts to run.
	 public CharacterBody2D player_node = null;
	
	//Exportable Battle Stats
	public int health = 50;
	public int max_health = 100;
	
	//Logic for allowing the switching of weapons
	public String weapon = "fists";
	public int basedamage = 10;
	public int damage = 10;
	
	//For Implementation of Field/Battle Items
	public bool isBattling = false;
	
	//logic for trap item. Can be used outside of battle.
	public bool trapped = false;
	public bool door = false;
	public int karma = 1000;

	 //Loads the 'inventory_slot' scene, and stores it here.
	 public PackedScene inventory_slot_scene;
	 //Loads the 'shop_slot' scene, and stores it here.
	 public PackedScene shop_slot_scene;

	 public int key_count = 0;

	//Global Singleton reference.
	 public Global glbl;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		inventory_slot_scene = GD.Load<PackedScene>("res://Scenes/inventory_slot.tscn");
		shop_slot_scene = GD.Load<PackedScene>("res://Scenes/shop_slot.tscn");
		custom_signals = GetNode<CustomSignals>("/root/CustomSignals");
		glbl = GetNode<Global>("/root/Global");
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
	public bool AddItem( Dictionary<string, dynamic> item){
		for(int i = 0; i < inventory.Length; i++) 
		{
			
			//Checks for if the current item is of the same type and has the same effect
			if ((inventory[i] != null) && (inventory[i]["item_type"] == item["item_type"]) && (inventory[i]["item_effect"] == item["item_effect"]))
			{
				
				
				//adds the amount of items to the quantity key in the item's dictionary, sends a signal and returns true.
				inventory[i]["quantity"] += item["quantity"];
				glbl.custom_signals.EmitSignal(nameof(CustomSignals.InventoryUpdated));
				return true;
				//checks for an empty spot in the inventory
			} else if (inventory[i] == null)
			{
				//Adds the item to the inventory at index i, emits a signal and returns true.
				inventory[i] = item;
				glbl.custom_signals.EmitSignal(nameof(CustomSignals.InventoryUpdated));
				return true;
			}
		}
		
		//if no spots are available, returns false.
		return false;
	}


	public bool DoorOpen(Dictionary<string, dynamic> door){
		//checks if key count is correct
		if(key_count > door["door_number"]){
			return true;
		}
		else{
			return false;
		}
	}
	/*
	Summary:
	Called from InventorySLot -> "OnDiscardButtonPressed." removes 1 from the quantity of the item selected, and removes that item from the inventory if out of items.

	Params:
	item_type: the type of the item being removed
	item_effect: the effect of the item being removed.
	*/
	public bool RemoveItem(dynamic item_type, dynamic item_effect)
	{
		for(int i = 0; i < inventory.Length; i++)
		{
			if ((inventory[i] != null) && (inventory[i]["item_type"] == item_type) && (inventory[i]["item_effect"] == item_effect))
			{
				//subtracts 1 to the quantity key in the item's dictionary, sends a signal and returns true.
				inventory[i]["quantity"]-= 1;
				//if there are no more items, removes it from inventory entirely.
				if(inventory[i]["quantity"] == 0)
				{
					inventory[i] = null;
					InventoryShift(glbl.inventory, i);
				}
				glbl.custom_signals.EmitSignal(nameof(CustomSignals.InventoryUpdated));
				return true;
			}
		}
		return false;
	}

	
	/* 
	Summary:
	This method keeps the player loaded globally to allow other methods to keep the player constantly identified.
	Params:
	CharacterBody2D player: The player character. Cannot be null or reference anything other than the player character.
	*/
		public void SetPlayerReference(CharacterBody2D player)
		{
			player_node = player;
		}
	/*
	Summary:
	Moves items in inventory in response to an open position.

	Params: 
	inventory: the inventory of the player
	index: the spot at which an opening was created.
	*/
		private void InventoryShift(dynamic[] inventory, int index)
		{
			//starts at the next item in the list.
			int i = index + 1;
			while(inventory[i] != null)
			{
				inventory[i-1] = inventory[i];
				i++;
			}
			//does one more iteration after to move the final item.
			inventory[i-1] = inventory[i];
		}
}
