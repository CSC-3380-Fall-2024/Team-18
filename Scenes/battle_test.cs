using Godot;
using System;

public partial class battle_test : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void on_body_entered(CharacterBody2D body)
	{
		if (body.IsInGroup("Player")) 
		{
			BaseEnemy eyeMan = GD.Load<BaseEnemy>("res://Scripts/eyeman.tres");
			
			PackedScene battleScene = GD.Load<PackedScene>("res://Scenes/battle.tscn");
			Battle battleEye = battleScene.Instantiate<Battle>();
			
			battleEye.enemy = eyeMan;
			
			GD.Print("yay");
			AddChild(battleEye);
			
		}
	}

}
