using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ItemCreator : Resource
{
    [Export]
    public int quantity  { get; set; }
	[Export]
    public string item_type  { get; set; }
    [Export]
    public string item_name  { get; set; }
    [Export]
    public Texture2D item_texture { get; set; }
    [Export]
    public int item_price  { get; set; }
    [Export]
    public string item_effect  { get; set; }
    [Export]
    public string equip_effect  { get; set; }
}
