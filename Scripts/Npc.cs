using Godot;
using System;


public partial class Npc: CharacterBody2D{
	public AnimatedSprite2D char_anim ;
	
	[Export] public Boolean talking = false;
	
	bool player_in_range = false;
	[Export] public CanvasLayer dialoguebox;
	public Global glbl;

	//[Export] public CanvasLayer Presstalk;
	
	public override void _Ready(){
			char_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			dialoguebox = GetNode<CanvasLayer>("dialoguebox");
			//Presstalk = GetNode<CanvasLayer>("Presstalk");
			char_anim.Play("idle");
			glbl = GetNode<Global>("/root/Global");
			//dialoguebox.Visible = false;
			
			//Presstalk.Visible = false;

	}
	/* 
	THIS IS JUST TO TEST. PLEASE REMOVE IF YOU GET WORKING
	*/
	public override void _Process(double delta){
		if (player_in_range && Input.IsActionJustPressed("talk"))
		{
			GD.Print("hello for now");
			Talk();
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
			player.EnableMovement = false;
			glbl.isBattling = true;
			
			//GetTree().Paused = !GetTree().Paused;
			
			BaseEnemy test_knight = GD.Load<BaseEnemy>("res://Scripts/test_knight.tres");
			
			PackedScene battleScene = GD.Load<PackedScene>("res://Scenes/battle.tscn");
			Battle BattleTest = battleScene.Instantiate<Battle>();
			
			BattleTest.enemy = test_knight;
			BattleTest.player = player;
			
			
			//GD.Print("yay");
			AddChild(BattleTest);
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
}
