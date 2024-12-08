using Godot;
using System;

public class DialogueBox : Panel
{
	private int karmaCount = 0;

	private Label dialogueText;
	private Button option1;
	private Button option2;
	private Button option3;

	public override void _Ready()
	{
		dialogueText = GetNode<Label>("DialogueText");
		option1 = GetNode<Button>("Option1");
		option2 = GetNode<Button>("Option2");
		option3 = GetNode<Button>("Option3");

		option1.Connect("pressed", this, nameof(OnOption1Pressed));
		option2.Connect("pressed", this, nameof(OnOption2Pressed));
		option3.Connect("pressed", this, nameof(OnOption3Pressed));

		Hide(); // Hide the dialogue box initially.
	}

	public void ShowDialogue()
	{
		Visible = true;
		dialogueText.Text = "NPC: Hello traveler! Would you like to help me with a task?";
		option1.Text = "1. Sure, I’ll help you with your task.";
		option2.Text = "2. Sorry, I can’t help right now.";
		option3.Text = "3. I don’t have time for this YOU IDIOT.";
	}

	public void HideDialogue()
	{
		Visible = false;
	}

	private void OnOption1Pressed()
	{
		karmaCount += 10;
		dialogueText.Text = "NPC: Thank you so much! I knew I could count on you.";
		EndInteraction();
	}

	private void OnOption2Pressed()
	{
		dialogueText.Text = "NPC: That's okay. Maybe another time.";
		EndInteraction();
	}

	private void OnOption3Pressed()
	{
		karmaCount -= 5;
		dialogueText.Text = "NPC: That was unnecessary...";
		EndInteraction();
	}

	private async void EndInteraction()
	{
		await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
		HideDialogue();
		GD.Print($"Karma Count: {karmaCount}");
	}
}
