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
	// Signal that sends Item Used's Name
	[Signal]
	public delegate void OnItemUsedEventHandler(string ItemEffect);
 	// Signal that tells when the player has stopped interacting with an NPC.
  	[Signal]
	public delegate void OnDialogueOptionPressedEventHandler();
}
