using Godot;
using System;

public partial class DialogueScene : Control
{
	public Global glbl;
	public BaseDialogue dialogue;
	public Player player;
	public Label text;
	public GridContainer grid_container;
	public PackedScene option_scene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		glbl = GetNode<Global>("/root/Global");
		text = GetNode<Label>("NPCDialogue/NPCDialogue_Inner/NPCDialogue_Text");
		option_scene = GD.Load<PackedScene>("res://Scenes/dialogue_option.tscn");
		grid_container = GetNode<GridContainer>("NPCDialogue/NPCDialogue_Options");
		glbl.custom_signals.OnDialogueOptionPressed += OnDialogueOptionPressed;
		text.Text = dialogue.dialogue;
		ShowOptions();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
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

	public void ShowOptions()
	{
		ClearGridContainer();
		foreach(BaseOptions option in dialogue.options)
		{
			//Takes the dialogue option and instantiates it as a member of the DialogueOption class.
			DialogueOption scene = option_scene.Instantiate() as DialogueOption;
			//adds a new slot to the dialogue box
			grid_container.AddChild(scene);
			//if empty, make invisible
			if(option == null)
			{
				scene.Visible = false;
			}
			scene.option_text.Text = option.text;
			scene.battle_enemy = option.battle_enemy;
			scene.karma = option.karma;
			scene.effects = option.effects;
			scene.item_creator = option.item_info;

		}
	}
	public void OnDialogueOptionPressed(){
		glbl.custom_signals.OnDialogueOptionPressed -= OnDialogueOptionPressed;
		QueueFree();
	}

}
