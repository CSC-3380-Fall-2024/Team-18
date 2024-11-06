using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
	public class DialogueNode
	{
		public string text { get; set; }
		public List<string> options { get; set; }
		public List<DialogueNode> next_nodes { get; set; }

		public DialogueNode(string input_text, List<string> input_options, List<DialogueNode> input_next_nodes)
		{
			text = input_text;
			options = input_options;
			next_nodes = input_next_nodes;
		}
	}

	private List<DialogueNode> dialogue_tree = new List<DialogueNode>();
	private int current_node_index = 0;
	private const float interaction_range = 50.0f; 
	private bool is_interacting = false; 

	public override void _Ready()
	{
		dialogue_tree.Add(new DialogueNode("Wassup g", 
			new List<string> { "Ask about the quest", "Bye idiot, I don't have time to talk" }, 
			new List<DialogueNode>
			{
				new DialogueNode("The quest is to find my long lost basketball", new List<string>(), new List<DialogueNode>()),
				new DialogueNode("You will regret that", new List<string>(), new List<DialogueNode>())
			}));
	}

	public override void _PhysicsProcess(double delta)
	{
		var npc_position = GetPosition();
		var player_position = GetNode<Player>("Player").GetPosition(); 

	   
		if (player_position.DistanceTo(npc_position) <= interaction_range && !is_interacting)
		{
			AskToInteract();
		}
	}

	private void AskToInteract()
	{
		is_interacting = true;
		GetNode<Label>("DialogueLabel").Text = "Approach? Yes/No";

		var buttons = GetNode<VBoxContainer>("OptionsContainer");
		ClearChildren(buttons);

	   
		var yes_button = new Button { Text = "Yes" };
		yes_button.Connect("pressed", new Callable(this, nameof(OnYesSelected)));
		buttons.AddChild(yes_button);

	 
		var no_button = new Button { Text = "No" };
		no_button.Connect("pressed", new Callable(this, nameof(OnNoSelected)));
		buttons.AddChild(no_button);
	}

	private void OnYesSelected()
	{
		current_node_index = 0; 
		ShowDialogue(current_node_index); 
	}

	private void OnNoSelected()
	{
		is_interacting = false; 
		ClearChildren(GetNode<VBoxContainer>("OptionsContainer")); 
		GetNode<Label>("DialogueLabel").Text = ""; 
	}

	private void ShowDialogue(int index)
	{
		if (index >= dialogue_tree.Count) return;

		var current_node = dialogue_tree[index];
		GetNode<Label>("DialogueLabel").Text = current_node.text;

		var buttons = GetNode<VBoxContainer>("OptionsContainer");
		ClearChildren(buttons);

		for (int i = 0; i < current_node.options.Count; i++)
		{
			var button = new Button { Text = current_node.options[i] };
			int option_index = i;
			button.Connect("pressed", new Callable(this, nameof(OnOptionSelected)), (uint)option_index);
			buttons.AddChild(button);
		}
	}

	private void OnOptionSelected(int option_index)
	{
		var current_node = dialogue_tree[current_node_index];
		if (option_index < current_node.next_nodes.Count)
		{
			current_node_index = dialogue_tree.IndexOf(current_node.next_nodes[option_index]);
			ShowDialogue(current_node_index);
		}
	}
	
	private void ClearChildren(Node node){
		foreach (Node child in node.GetChildren())
		{
			child.QueueFree();
		}
	}
}
