using Godot;
using System;
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
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		grid_container = GetNode<GridContainer>("GridContainer");
		grid_container.Connect(Global.SignalName.InventoryUpdated, Callable.From(On_Inventory_Updated));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void On_Inventory_Updated()
	{
		Clear_Grid_Container();
	}
	public void Clear_Grid_Container()
	{
		while (grid_container.GetChildCount() > 0)
		{
			var child = grid_container.GetChild(0);
			grid_container.RemoveChild(child);
			child.QueueFree();
		}
	}
}
