using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class DialogueOption : Control
{
	public Player player;  // Reference to the player object
	public Global glbl;    // Reference to global game state
	public Label option_text;  // Label to display dialogue option text
	public BaseEnemy battle_enemy;  // Reference to the enemy for battle (if required)
	public int karma;  // Karma change from this dialogue option
	public ItemCreator item_creator;  // Reference to item creator (if you want to add items)

	// Karma change values for each dialogue option (Positive, Neutral, Negative)
	public int[] karmaEffects = new int[3] { 10, 0, -10 };  // Positive, Neutral, Negative karma effects
	public string[] dialogueTexts = new string[3] {
		"You're a great person, I appreciate you!", // Positive 
		"I don't have much to say right now.", // Neutral 
		"You really shouldn't be here, leave!" // Negative
	};
	
	Vector2 position_offset = new Vector2(20, -100);
	public string[] effects = new string[3];  // Effects (battle, karma, item)
	
	public override void _Ready()
	{
		glbl = GetNode<Global>("/root/Global");
		glbl.custom_signals = GetNode<CustomSignals>("/root/CustomSignals");
		player = GetTree().Root.GetNode<Player>("Main/Player");
		option_text = GetNode<Label>("Option_Border/Option_Background/Option_Text");
	}

	public void ShowDialogueOptions()
	{
		// Display the three dialogue options (using buttons)
		for (int i = 0; i < 3; i++)
		{
			option_text.Text = dialogueTexts[i];  // Set the option text
			
			// Create buttons for each dialogue option
			var button = new Button();
			button.Text = dialogueTexts[i];
			button.RectPosition = new Vector2(20, 100 + i * 40);  // Arrange buttons vertically
			button.Connect("pressed", this, nameof(OnOptionButtonPressed), new Godot.Collections.Array() { i });
			this.AddChild(button);  // Add the button to the scene
		}
	}

	public void OnOptionButtonPressed(int optionIndex)
	{
		// Handle the selected option and apply the corresponding effects
		for (int i = 0; i < effects.Length; i++)
		{
			if (effects[i] == "battle")
			{
				StartBattle();  // Start battle if the option has a "battle" effect
			}
			if (effects[i] == "karma")
			{
				ApplyKarma(karmaEffects[optionIndex]);  // Apply karma change based on the selected option
			}
			if (effects[i] == "item")
			{
				Dictionary<string, dynamic> item = new Dictionary<string, dynamic>
				{
					{ "quantity", item_creator.quantity },
					{ "item_type", item_creator.item_type },
					{ "item_name", item_creator.item_name },
					{ "item_texture", item_creator.item_texture },
					{ "item_effect", item_creator.item_effect },
					{ "item_price", item_creator.item_price },
					{ "equip_effect", item_creator.equip_effect }
				};
				GiveItem(item);  // Give item if the option has an "item" effect
			}
		}

		// Remove the buttons after the option is selected
		foreach (Node child in GetChildren())
		{
			if (child is Button)
			{
				child.QueueFree();  // Remove the button from the scene
			}
		}

		glbl.custom_signals.EmitSignal(nameof(CustomSignals.OnDialogueOptionPressed));  // Trigger custom signal for dialogue
	}

	public void ApplyKarma(int karmaChange)
	{
		// Update the karma value based on the selected option
		glbl.karma += karmaChange;
		GD.Print("Karma updated: " + glbl.karma);
	}

	public void StartBattle()
	{
		// Start a battle scene (if necessary)
		PackedScene battleScene = GD.Load<PackedScene>("res://Scenes/battle.tscn");
		Battle battleTest = battleScene.Instantiate<Battle>();
		battleTest.enemy = battle_enemy;
		battleTest.player = player;

		glbl.isBattling = true;
		player.AddChild(battleTest);
		battleTest.GlobalPosition = player.GlobalPosition;
		battleTest.ZIndex = 5;
		GD.Print(battleTest.GlobalPosition);
	}

	public void GiveItem(Dictionary<string, dynamic> item)
	{
		// Add an item to the player's inventory (if necessary)
		glbl.AddItem(item);
	}

	public override void _Process(double delta)
	{
		// Optional: Update logic if needed
	}
}
