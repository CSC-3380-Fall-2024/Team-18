using Godot;
using System;


public partial class Npc: CharacterBody2D{
	public AnimatedSprite2D char_anim ;
	
	[Export] public Boolean talking = false;
	
	bool player_in_range = false;
	[Export] public CanvasLayer dialoguebox;

	//[Export] public CanvasLayer Presstalk;
	
	public override void _Ready(){
			char_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			dialoguebox = GetNode<CanvasLayer>("dialoguebox");
			//Presstalk = GetNode<CanvasLayer>("Presstalk");
			char_anim.Play("idle");
			dialoguebox.Visible = false;
			
			//Presstalk.Visible = false;

	}
	/* 
	THIS IS JUST TO TEST. PLEASE REMOVE IF YOU GET WORKING
	*/
	public override void _Process(double delta){
		if (player_in_range && Input.IsActionJustPressed("ui_add"))
		{
			GD.Print("hello for now");
		}
	}

	
	public void OnArea2DBodyEntered(Player body)
	{
		//checks for the player specifically
		if(body.IsInGroup("Player"))
		{
			GD.Print("yes");
			player_in_range = true;
			body.Presstalk.Visible = true;
		}
	}

	public void OnArea2DBodyExited(Player body)
	{
		if(body.IsInGroup("Player"))
		{
			GD.Print("no");
			player_in_range = false;
			body.Presstalk.Visible = false;
		}
	}

	public void UpdateAnimations(){
			if(talking == true){
				char_anim.Play("talk");
			}
			else{
				char_anim.Play("idle");
			}
	}

	public void Talk(InputEvent @event)
	{
		if(@event.IsActionPressed("talk") && player_in_range == true)
		{
			GD.Print("Please god help me.");
			talking = true;
			dialoguebox.Visible = !dialoguebox.Visible;
			//Presstalk.Visible = !Presstalk.Visible;
			GD.Print("Enter.");
			
		}

	}
}
