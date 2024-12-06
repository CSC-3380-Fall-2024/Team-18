using Godot;
using System;

public partial class EndScreen : Control
{
	
	public Global glbl;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		glbl = GetNode<Global>("/root/Global");
		SetText();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetText(){
		if (glbl.karma == 1000){
			GetNode<Label>("Textbox/Label").Text = "You have contributed nothing. Good Job?";
		}
		else if (glbl.karma < 1000 && glbl.karma > 500){
			GetNode<Label>("Textbox/Label").Text = "You are the town scamp. Game Over";
		}
		else if (glbl.karma <= 500){
			GetNode<Label>("Textbox/Label").Text = "You are a scoundrel and you are hated. Game Over.";
		}
		GetNode<Label>("Textbox/Label").Visible = true;
	}
}
