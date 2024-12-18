using Godot;
using System;
using System.Collections.Generic;
/*
	Summary:
	Script for placing the inventory items into the correct slots. Currently untouched.
	*/
public partial class ShopSlot : Control
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
	public Label item_price;
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
		item_effect = GetNode<Label>("Outer_Border2/Details_Panel/Item_Effect");
		item_name = GetNode<Label>("Outer_Border2/Details_Panel/Item_Name");
		item_type = GetNode<Label>("Outer_Border2/Details_Panel/Item_Type");
		item_price = GetNode<Label>("Usage_Panel/ItemPrice");
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
		if (item != null)
		{
			usage_panel.Visible = !usage_panel.Visible;
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
		item_price.Text = "";
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
		item_price.Text = "$" + item["item_price"].ToString();
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
	public void OnPurchaseButtonPressed()
	{
		GD.Print("Signal recieved");
		if(item != null && item["item_price"] < glbl.money){
			glbl.AddItem(item);
			glbl.money -= item["item_price"];
		}
	}
}
