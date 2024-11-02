using Godot;
using System;


public partial class Npc: CharacterBody2D{
	public AnimatedSprite2D char_anim ;
	
	[Export] public Boolean talking = false;
	
	bool player_in_range = false;
	[Export] public CanvasLayer dialoguebox;

	[Export] public CanvasLayer Presstalk;
	
	public override void _Ready(){
			char_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			dialoguebox = GetNode<CanvasLayer>("dialoguebox");
			Presstalk = GetNode<CanvasLayer>("Presstalk");
			char_anim.Play("idle");

	}

	
	public void OnArea2DBodyEntered(Player body)
	{
		//checks for the player specifically
		if(body.IsInGroup("Player"))
		{
			player_in_range = true;
			body.interact_ui.Visible = true;
		}
	}

	public void OnArea2DBodyExited(Player body)
	{
		if(body.IsInGroup("Player"))
		{
			player_in_range = false;
			body.interact_ui.Visible = false;
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
			talking = true;
			dialoguebox.Visible = !dialoguebox.Visible;
			Presstalk.Visible = !Presstalk.Visible;
			GD.Print("Enter.");
			
		}

	}
}
