using Godot;
using System;
using System.Collections.Generic;
/*
Summary:
Manages displaying the items in the invetory correctly. 
Note:
Currently broken due to an error with connecting to an incorrect signal. Will try to fix later.
- London
*/
public partial class InventoryUi : Control
{
	[Export]
	public GridContainer grid_container;

	public Global glbl;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		glbl = GetNode<Global>("/root/Global");
		grid_container = GetNode<GridContainer>("GridContainer");
		glbl.custom_signals = GetNode<CustomSignals>("/root/CustomSignals");
		
		/*
		I had used this at some point to connect some signal not through glbl, 
		but dropped it because of Godot 4.3's weird syntax for the connection and global signals worked better
		However, we had a problem where signals weren't being disconnected on QueueFree(), so this is the solution.
		*/
		glbl.custom_signals.Connect("InventoryUpdated", new Callable(this, nameof(OnInventoryUpdated)));
		OnInventoryUpdated();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	/*
	Summary:
	Listens for the InventoryUpdated signal; if recieved, calls ClearGridContainer and resets the inventory menu to match the current state of the inventory array.
	*/
	public void OnInventoryUpdated()
	{
		ClearGridContainer();
		foreach(Dictionary<string, dynamic> item in glbl.inventory)
		{
			//Takes the loaded inventory slot from Global and instantiates it as a member of the InventorySlot class.
			InventorySlot scene = glbl.inventory_slot_scene.Instantiate() as InventorySlot;
			//adds a new slot to the inventory
			grid_container.AddChild(scene);
			//if not empty, add with item
			if(item != null)
			{
				scene.SetItem(item);
			}
			//if empty, add with no item
			else
			{
				scene.SetEmpty();
			}
		}
	}
	/*
	Summary:
	Wipes the inventory clear to allow it to be re-displayed.
	*/
	public void ClearGridContainer()
	{
		//While there are children in grid_container, remove child.
		while (grid_container.GetChildCount() > 0)
		{
			var child = grid_container.GetChild(0);
			grid_container.RemoveChild(child);
			child.QueueFree();
		}
	}
}
