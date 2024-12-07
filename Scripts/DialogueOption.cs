using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class DialogueOption : Control
{
	public Player player;
	public Global glbl;
	public Label option_text;
	public BaseEnemy battle_enemy;
	public int karma;

	Vector2 position_offset = new Vector2(20, -100);
	public string[] effects = new string[3];
	public ItemCreator item_creator;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		glbl = GetNode<Global>("/root/Global");
		glbl.custom_signals = GetNode<CustomSignals>("/root/CustomSignals");
		player = GetTree().Root.GetNode<Player>("Main/Player");
		option_text = GetNode<Label>("Option_Border/Option_Background/Option_Text");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void KarmaUpdate(int karma){
		glbl.karma += karma;
	}
	
	public void GiveItem( Dictionary<string, dynamic> item){
		glbl.AddItem(item);
	}
	
	public void StartBattle(){
			PackedScene battleScene = GD.Load<PackedScene>("res://Scenes/battle.tscn");

			Battle BattleTest = battleScene.Instantiate<Battle>();
			BattleTest.enemy = battle_enemy;
			BattleTest.player = player;

			glbl.isBattling = true;
			player.AddChild(BattleTest);
			BattleTest.GlobalPosition = player.GlobalPosition;
			BattleTest.ZIndex = 5;
			GD.Print(BattleTest.GlobalPosition);
	}
	public void OnOptionButtonPressed(){
		for(int i = 0; i < effects.Length; i++) { 
			if(effects[i] == "battle"){
				StartBattle();
			}
			if(effects[i] == "karma"){
				KarmaUpdate(karma);
			}
			if(effects[i] == "item"){
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
			GiveItem(item);
			}
		}
		glbl.custom_signals.EmitSignal(nameof(CustomSignals.OnDialogueOptionPressed));
	}

}
