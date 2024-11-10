using Godot;
using System;

public partial class Player : Node2D

{
	[Export] public int Speed = 200;
	private NPC _currentNPC;
	private bool _inDialogue = false; // To track if player is currently in dialogue

	private string[] _playerDialogChoices = new string[] {
		"Yes, I will gladly help you defeat the evil baseball king!",
		"No, I can't help you. I have more important things to tend to."
	};

	private int _currentChoice = 0; // Tracks the currently selected choice

	public override void _PhysicsProcess(float delta)
	{
		Vector2 velocity = new Vector2();

		if (Input.IsActionPressed("ui_right"))
			velocity.x += 1;
		if (Input.IsActionPressed("ui_left"))
			velocity.x -= 1;
		if (Input.IsActionPressed("ui_down"))
			velocity.y += 1;
		if (Input.IsActionPressed("ui_up"))
			velocity.y -= 1;

		MoveAndSlide(velocity.Normalized() * Speed);
	}

	public override void _Input(InputEvent @event)
	{
		// Check if player is interacting with the NPC
		if (_inDialogue && @event.IsActionPressed("ui_up"))
		{
			_currentChoice = (_currentChoice - 1 + _playerDialogChoices.Length) % _playerDialogChoices.Length; // Cycle up through choices
			GD.Print("Choice: " + _playerDialogChoices[_currentChoice]);
		}
		else if (_inDialogue && @event.IsActionPressed("ui_down"))
		{
			_currentChoice = (_currentChoice + 1) % _playerDialogChoices.Length; // Cycle down through choices
			GD.Print("Choice: " + _playerDialogChoices[_currentChoice]);
		}

		if (_inDialogue && @event.IsActionPressed("ui_accept"))
		{
			// Respond based on the selected choice
			if (_currentChoice == 0) // "Yes, I will help!"
			{
				_currentNPC.Interact(true); // Player agrees to help
			}
			else if (_currentChoice == 1) // "No, I can't help."
			{
				_currentNPC.Interact(false); // Player declines to help
			}

			_inDialogue = false; // End dialogue after the player makes a choice
		}

		if (@event.IsActionPressed("interact") && _currentNPC != null && !_inDialogue)
		{
			// Start dialogue with the NPC
			_inDialogue = true;
			GD.Print("Gerald: " + _currentNPC.GetCurrentDialogue()); // Show NPC's dialogue
			GD.Print("Select:");
			foreach (var option in _playerDialogChoices)
			{
				GD.Print(option);
			}
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body is NPC npc)
		{
			_currentNPC = npc;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body == _currentNPC)
		{
			_currentNPC = null;
		}
	}
}
