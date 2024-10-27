using Godot;
using System;
using System.Collections.Generic;

public partial class Main : CharacterBody2D
{
    public class DialogueNode
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public List<DialogueNode> NextNodes { get; set; }

        public DialogueNode(string text, List<string> options, List<DialogueNode> nextNodes)
        {
            Text = text;
            Options = options;
            NextNodes = nextNodes;
        }
    }

    private List<DialogueNode> dialogueTree = new List<DialogueNode>();
    private int currentNodeIndex = 0;
    private const float interactionRange = 50.0f; 
    private bool isInteracting = false; 

    public override void _Ready()
    {
        dialogueTree.Add(new DialogueNode("Wassup g", 
            new List<string> { "Ask about the quest", "Bye idiot, I don't have time to talk" }, 
            new List<DialogueNode>
            {
                new DialogueNode("The quest is to find my long lost basketball", new List<string>(), new List<DialogueNode>()),
                new DialogueNode("You will regret that", new List<string>(), new List<DialogueNode>())
            }));
    }

    public override void _Process(float delta)
    {
        var npcPosition = GetPosition();
        var playerPosition = GetNode<Player>("Player").GetPosition(); 

       
        if (playerPosition.DistanceTo(npcPosition) <= interactionRange && !isInteracting)
        {
            AskToInteract();
        }
    }

    private void AskToInteract()
    {
        isInteracting = true;
        GetNode<Label>("DialogueLabel").Text = "Approach? Yes/No";

        var buttons = GetNode<VBoxContainer>("OptionsContainer");
        buttons.ClearChildren();

       
        var yesButton = new Button { Text = "Yes" };
        yesButton.Connect("pressed", this, nameof(OnYesSelected));
        buttons.AddChild(yesButton);

     
        var noButton = new Button { Text = "No" };
        noButton.Connect("pressed", this, nameof(OnNoSelected));
        buttons.AddChild(noButton);
    }

    private void OnYesSelected()
    {
        currentNodeIndex = 0; 
        ShowDialogue(currentNodeIndex); 
    }

    private void OnNoSelected()
    {
        isInteracting = false; 
        GetNode<VBoxContainer>("OptionsContainer").ClearChildren(); 
        GetNode<Label>("DialogueLabel").Text = ""; 
    }

    private void ShowDialogue(int index)
    {
        if (index >= dialogueTree.Count) return;

        var currentNode = dialogueTree[index];
        GetNode<Label>("DialogueLabel").Text = currentNode.Text;

        var buttons = GetNode<VBoxContainer>("OptionsContainer");
        buttons.ClearChildren();

        for (int i = 0; i < currentNode.Options.Count; i++)
        {
            var button = new Button { Text = currentNode.Options[i] };
            int optionIndex = i;
            button.Connect("pressed", this, nameof(OnOptionSelected), new Godot.Collections.Array { optionIndex });
            buttons.AddChild(button);
        }
    }

    private void OnOptionSelected(int optionIndex)
    {
        var currentNode = dialogueTree[currentNodeIndex];
        if (optionIndex < currentNode.NextNodes.Count)
        {
            currentNodeIndex = dialogueTree.IndexOf(currentNode.NextNodes[optionIndex]);
            ShowDialogue(currentNodeIndex);
        }
    }
}