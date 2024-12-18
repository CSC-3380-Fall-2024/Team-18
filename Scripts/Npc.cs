using Godot;
using System;


public partial class Npc: CharacterBody2D{
	public AnimatedSprite2D char_anim ;
	
	[Export] public Boolean talking = false;
	
	bool player_in_range = false;
	[Export] public CanvasLayer dialoguebox;

	Vector2 position_offset = new Vector2(20, -100);
	public Global glbl;
	public Player player_reference;

	//[Export] public CanvasLayer Presstalk;
	
	public override void _Ready(){
			char_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			dialoguebox = GetNode<CanvasLayer>("dialoguebox");
			//Presstalk = GetNode<CanvasLayer>("Presstalk");
			char_anim.Play("idle");
			glbl = GetNode<Global>("/root/Global");
			glbl.custom_signals.OnDialogueOptionPressed += OnDialogueOptionPressed;
			//dialoguebox.Visible = false;
			
			//Presstalk.Visible = false;

	}
	/* 
	THIS IS JUST TO TEST. PLEASE REMOVE IF YOU GET WORKING
	*/
	public override void _Process(double delta){
		if (player_in_range && Input.IsActionJustPressed("talk"))
		{
			player_reference.EnableMovement = false;
			player_reference.Presstalk.Visible = false;

			GD.Print("hello for now");
			BaseDialogue test_dialogue = GD.Load<BaseDialogue>("res://Scripts/tresses/test_dialogue.tres");
			PackedScene dialogue_scene = GD.Load<PackedScene>("res://Scenes/dialogue_scene.tscn");
			DialogueScene dialogue_test = dialogue_scene.Instantiate<DialogueScene>();

			dialogue_test.dialogue = test_dialogue;
			AddChild(dialogue_test);
			dialogue_test.GlobalPosition = player_reference.GlobalPosition + position_offset;
			GD.Print(dialogue_test.GlobalPosition);
		}
	}

	
	public void OnArea2DBodyEntered(Node2D body)
	{
		
		//checks for the player specifically
		if(body.IsInGroup("Player") && body is Player player)
		{
			GD.Print("yes");
			player_in_range = true;
			GD.Print("Player in Range?", player_in_range);
			player.Presstalk.Visible = true;
			player_reference = player;
		}
		//GD.Print("Body Entered Type: ", body.GetType());
	}

	public void OnArea2DBodyExited(Node2D body)
	{
		
		if(body.IsInGroup("Player") && body is Player player)
		{
			GD.Print("no");
			player_in_range = false;
			player.Presstalk.Visible = false;
			dialoguebox.Visible = false;
		}
		//GD.Print("Body Entered Type: ", body.GetType());
	}

	public void UpdateAnimations(){
			if(talking == true){
				char_anim.Play("talk");
			}
			else{
				char_anim.Play("idle");
			}
	}

	public void Talk()
	{
		
		GD.Print("Please god help me.");
		talking = true;
		dialoguebox.Visible = !dialoguebox.Visible;
		//Presstalk.Visible = !Presstalk.Visible;
		glbl.door = true;
		GD.Print("Enter.");
			

	}
	public void OnDialogueOptionPressed(){
		if (glbl.isBattling == false){
		player_reference.EnableMovement = true;
		player_reference.Presstalk.Visible = true;
		}
	}
}
