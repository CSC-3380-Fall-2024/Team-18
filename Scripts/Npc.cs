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
			//dialoguebox.Visible = false;
			
			//Presstalk.Visible = false;

	}
	/* 
	THIS IS JUST TO TEST. PLEASE REMOVE IF YOU GET WORKING
	*/
	public override void _Process(double delta){
		if (player_in_range && Input.IsActionJustPressed("ui_add"))
		{
			GD.Print("hello for now");
			Talk();
		}
	}

	
	public void OnArea2DBodyEntered(Player body)
	{
		GD.Print("Body Entered Type: ", body.GetType());
		//checks for the player specifically
		if(body is Player player && body.IsInGroup("Player"))
		{
			GD.Print("yes");
			player_in_range = true;
			GD.Print("Player in Range?", player_in_range);
			body.Presstalk.Visible = true;
		}
	}

	public void OnArea2DBodyExited(Player body)
	{
		GD.Print("Body Entered Type: ", body.GetType());
		if(body is Player player && body.IsInGroup("Player"))
		{
			GD.Print("no");
			player_in_range = false;
			body.Presstalk.Visible = false;
			dialoguebox.Visible = false;
			player.EnableMovement = false;
			
			//GetTree().Paused = !GetTree().Paused;
			
			BaseEnemy test_knight = GD.Load<BaseEnemy>("res://Scripts/test_knight.tres");
			
			PackedScene battleScene = GD.Load<PackedScene>("res://Scenes/battle.tscn");
			Battle BattleTest = battleScene.Instantiate<Battle>();
			
			BattleTest.enemy = test_knight;
			BattleTest.player = player;
			
			
			//GD.Print("yay");
			AddChild(BattleTest);
			
			
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

	public void Talk()
	{
		
		GD.Print("Please god help me.");
		talking = true;
		dialoguebox.Visible = !dialoguebox.Visible;
		//Presstalk.Visible = !Presstalk.Visible;
		GD.Print("Enter.");
			

	}
}
