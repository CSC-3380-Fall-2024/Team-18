using Godot;
using System;

public partial class Npc : Area2D
{
	public bool TaskCompleted = false;
	public bool PlayerAgreed = false;

	private string[] dialogOptions = {
		"Will you help me with a task?",
		"Thank you for agreeing to help!",
		"Oh, I see. Maybe next time..."
	};

	private int dialogState = 0;  // 0: Ask, 1: Positive Response, 2: Negative Response

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(OnPlayerEnter));
	}

	private void OnPlayerEnter(Node body)
	{
		if (body is Player)
		{
			GD.Print("Press 'E' to interact with me.");
		}
	}

	public string GetCurrentDialogue()
	{
		// Return the current dialogue based on the NPC's state
		if (dialogState == 0)
			return dialogOptions[0]; // Asking for help
		else if (dialogState == 1)
			return dialogOptions[1]; // Positive response
		else
			return dialogOptions[2]; // Negative response
	}

	public void Interact(bool agree)
	{
		if (dialogState == 0)  // Initial question
		{
			PlayerAgreed = agree;
			if (agree)
			{
				dialogState = 1;
				TaskCompleted = true;
				GD.Print(dialogOptions[1]);
				GetNode<GameManager>("/root/GameManager").RecordChoice(true);
			}
			else
			{
				dialogState = 2;
				GD.Print(dialogOptions[2]);
				GetNode<GameManager>("/root/GameManager").RecordChoice(false);
			}
		}
	}
}
