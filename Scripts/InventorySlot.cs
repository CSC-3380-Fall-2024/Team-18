using Godot;
using System;
using System.Collections.Generic;
/*
	Summary:
	Script for placing the inventory items into the correct slots. Currently untouched.
	*/
public partial class InventorySlot : Control
{
	//Inventory icon for item
	public Sprite2D icon;
	//Label for the amount of items; NOT the quantity itself.
	public Label quantity_label;
	//Label for the effect of the item; NOT the effect itself.
	public Label item_effect;
	//Label for the name of the item; NOT the name itself.
	public Label item_name;
	//Label for the type of the item; NOT the type itself.
	public Label item_type;
	public ColorRect details_panel;
	public ColorRect usage_panel;
	//Global Reference
	public Global glbl;

	public Dictionary<string, dynamic> item;
	// Called when the node enters the scene tree for the first time.
	//Gets the node references for all of the required variables.
	public override void _Ready()
	{
		icon = GetNode<Sprite2D>("Inner_Border/ItemIcon");
		quantity_label = GetNode<Label>("Inner_Border/ItemQuantity");
		item_effect = GetNode<Label>("Outer_Border2/Details_Panel/Item_Effect");
		item_name = GetNode<Label>("Outer_Border2/Details_Panel/Item_Name");
		item_type = GetNode<Label>("Outer_Border2/Details_Panel/Item_Type");
		details_panel = GetNode<ColorRect>("Outer_Border2");
		usage_panel = GetNode<ColorRect>("Usage_Panel");
		glbl = GetNode<Global>("/root/Global");
		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/*
	Summary:
	Checks if the current inventory slot was clicked on; shows the usage menu if it was.
	*/

	private void OnItemButtonPressed()
	{
		if (item != null && usage_panel.Visible == false && details_panel.Visible == true)
		{
			usage_panel.Visible = true;
			details_panel.Visible = !details_panel.Visible;
		}
		else if (item!= null && details_panel.Visible == false && usage_panel.Visible == false)
		{
			details_panel.Visible = !details_panel.Visible;
		}
	}
	/*
	Summary:
	Checks if the mouse is hovered over the current inventory slot; shows the details menu if it was.
	*/
	private void OnItemButtonMouseEntered()
	{

		if (item != null)
		{
			details_panel.Visible = true;
		}
	}
	/*
	Summary:
	Checks if the mouse stops hovering over the current inventory slot; hides the usage menu if it was.
	*/
	private void OnItemButtonMouseExited()
	{
		if (item != null)
		{
			details_panel.Visible = false;
		}
	}
	/*
	Summary:
	Method for creating an empty inventory slot.
	*/
	public void SetEmpty()
	{
		icon.Texture = null;
		quantity_label.Text = "";
	}
	/*
	Summary:
	Method for creating an inventory slot with an item in it.

	Params:
	Dictionary<string, dynamic> new_item: The item being added to the inventory menu.
	*/
	public void SetItem(Dictionary<string, dynamic> new_item)
	{
		item = new_item;
		icon.Texture = new_item["item_texture"];
		quantity_label.Text = item["quantity"].ToString();
		item_name.Text = item["item_name"].ToString();
		item_type.Text = item["item_type"].ToString();
		if (item["item_effect"] != "")
		{
			item_effect.Text = "+ " + item["item_effect"].ToString();
		}
		else
		{
			item_effect.Text = "";
		}

	}
	/*
	Summary:
	If the discard button is pressed, removes the current item from the inventory.
	*/
	public void OnDiscardButtonPressed()
	{
		if(item != null){
			glbl.RemoveItem(item["item_type"], item["item_effect"]);
		}
	}
	/*
	Summary:
	Checks Item type to determine validity of use. Then after the item type is determined and filtered 
	for use, then the item is removed, and field item effects are used. Effects are sent using CustomSingals.
	These signals are used for Battle implementation.
	*/
	public void OnUseButtonPressed()
	{
		if(item["item_effect"] == "healing")
		{
			glbl.health = Math.Min(glbl.health+50, 100);
			glbl.RemoveItem(item["item_type"], item["item_effect"]);
			glbl.custom_signals.EmitSignal(nameof(CustomSignals.OnItemUsed),item_effect);
		}
		if(item["item_effect"] == "traps")
		{
			glbl.trapped = true;
			glbl.RemoveItem(item["item_type"], item["item_effect"]);
			glbl.custom_signals.EmitSignal(nameof(CustomSignals.OnItemUsed),item["item_effect"]);
		}
		if(item["item_effect"] == "firebomb" && glbl.isBattling == false) 
		{
			GD.Print("You can't use that now."); //textbox maybe?
		}
		if(item["item_effect"] == "firebomb" && glbl.isBattling == true)
		{
			glbl.RemoveItem(item["item_type"], item["item_effect"]);
			glbl.custom_signals.EmitSignal(nameof(CustomSignals.OnItemUsed),item["item_effect"]);
		}
		if(item["item_type"] == "Equipment")
		{
			if(item["item_name"] == "SteelSword" && glbl.isBattling == false) 
			{
				glbl.weapon = item["item_name"];
				glbl.damage = glbl.basedamage + item["equip_effect"];
				glbl.RemoveItem(item["item_type"], item["item_effect"]);
			}
		}
		if(item["item_type"] == "fish")
		{
			glbl.health = glbl.max_health;
			glbl.RemoveItem(item["item_type"], item["item_effect"]);
			glbl.custom_signals.EmitSignal(nameof(CustomSignals.OnItemUsed),item_effect);
		}
		if(item["item_effect"] == "key" && (glbl.isBattling == true || glbl.door == true))
		{
			glbl.door = true;
			GD.Print("You can't use that now.");
		}
		if(item["item_effect"] == "key" && glbl.isBattling == false && glbl.door == false)
		{
			glbl.door = true;
			glbl.RemoveItem(item["item_type"], item["item_effect"]);
			glbl.custom_signals.EmitSignal(nameof(CustomSignals.OnItemUsed),item["item_effect"]);
		}
		
			
		// Dont know what to do with a map.
		

	}
	
	public override void _Input(InputEvent @event) 
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			if(!usage_panel.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
			{
				usage_panel.Visible = false;
			}
		}
	}
}
