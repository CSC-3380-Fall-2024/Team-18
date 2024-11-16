using Godot;
using System;
/*
Summary:
This class serves as a hub for all custom signals that need to be used.
*/
public partial class CustomSignals : Node
{
	//Signal that fires when the inventory is updated in any way.
	[Signal]
	public delegate void InventoryUpdatedEventHandler();
	[Signal]
	public delegate void OnShopOpenedEventHandler();
}
