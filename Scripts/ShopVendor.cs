using Godot;
using System;
using System.Collections.Generic;
public partial class ShopVendor : Node2D
{
	public Global glbl;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		glbl = GetNode<Global>("/root/Global");
		var texture_loader = GD.Load<Texture2D>("res://Assets/Assets/Icons/icon1.png"); 
		Dictionary<string, dynamic> item = new Dictionary<string, dynamic>();
		item.Add("quantity", 1);
		item.Add("item_type", "fish");
		item.Add("item_name", "fish");
		item.Add("item_texture", texture_loader);
		item.Add("item_effect", "fish");
		item.Add("item_price", 100);
		glbl.shop[0] = item;
		GD.Print("item type = " + glbl.shop[0]["item_type"]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}