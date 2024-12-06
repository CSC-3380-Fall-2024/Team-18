using Godot;
using System;
using System.Threading.Tasks;


[Signal]
public delegate void TextClosed();




public partial class Enemy : Node
{
	[Export]
	public Enemy enemyResource = null; // Exported Resource variable
}

public partial class Battle : Control
{
	
	[Export]
	public BaseEnemy enemy;
	[Export] 
	public Player player;
	[Export]
	public CanvasLayer interact_ui;
	//CanvasLayer for the screen that appears when you open the inventory.
	[Export]
	public CanvasLayer InventoryUI;
	
	//Declare health variables at class level
	private int current_enemy_health = 0;
	private int max_player_health = 0; //won't use until potions
	private int MAX_ENEMY_HEALTH; //likely won't use until enemies get >1 attack
	private bool is_defending = false;
	public Global glbl;
	
	
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		//enemy.defeated = false;
		glbl = GetNode<Global>("/root/Global");
		glbl.custom_signals = GetNode<CustomSignals>("/root/CustomSignals");
		glbl.custom_signals.OnItemUsed += OnItemUsed;
		//state = new State();
		setHealth(GetNode<ProgressBar>("EnemyContainer/ProgressBar"), enemy.health, enemy.health);
		setHealth(GetNode<ProgressBar>("PlayerPanel/PlayerData/ProgressBar"), glbl.health, glbl.max_health);
		GetNode<TextureRect>("EnemyContainer/Enemy").Texture = (Texture2D)enemy.texture;
		
		current_enemy_health = enemy.health;
		max_player_health = glbl.max_health;
		MAX_ENEMY_HEALTH = enemy.health;
		
		AddUserSignal(nameof(TextClosed));
		
		
		GetNode<Control>("Textbox").Hide();
		GetNode<Panel>("Actions").Hide();
		
		
		display_text($"A wild {enemy.name} appears.");
		await ToSignal(this, nameof(TextClosed));
		//GD.Print("Fuck1");
		GetNode<Panel>("Actions").Show();
	}
	public async void OnItemUsed(string ItemEffect){
		GD.Print("Item Used.");
		GD.Print($"{ItemEffect}");
		player.inventory_ui.Visible = !player.inventory_ui.Visible;
		
		if (ItemEffect == "firebomb")
		{
			display_text("You throw a firebomb, dealing 10 damage.");
			await ToSignal(this, nameof(TextClosed));
			current_enemy_health -= 10;
		}
		
		setHealth(GetNode<ProgressBar>("PlayerPanel/PlayerData/ProgressBar"), glbl.health, max_player_health);
		setHealth(GetNode<ProgressBar>("EnemyContainer/ProgressBar"), current_enemy_health, MAX_ENEMY_HEALTH);
		
		EnemyTurn();
		
	}
	
	public void setHealth(ProgressBar progressBar, int current_health, int max_health)
	{
		progressBar.Value = current_health;
		progressBar.MaxValue = max_health;
		Label healthLabel = progressBar.GetNode<Label>("Label");
		healthLabel.Text = $"HP: {current_health} / {max_health}";
	}
	public override void _Input(InputEvent @event)
	{
		if ( Input.IsMouseButtonPressed(MouseButton.Left) && GetNode<Control>("Textbox").Visible == true)
		{
			//GetTree().Paused = false;
			GD.Print("Text Closed");
			GetNode<Control>("Textbox").Hide();
			EmitSignal(nameof(TextClosed));		
		}
		
		if(@event.IsActionPressed("ui_inventory"))
		{
			GetNode<CanvasLayer>("InventoryUI").Visible = false;
		}
	}
	public void display_text(String text)
	{
		//GetTree().Paused = false;
		GetNode<Control>("Textbox").Show();
		GetNode<Label>("Textbox/Label").Text = text;
	}
	private async void EnemyTurn()
	{
		if (is_defending == true)
		{
			display_text($"{enemy.name} attacks, dealing 0 damage.");
			await ToSignal(this, nameof(TextClosed));
			is_defending = false;
		}
		else if (glbl.trapped == true)
		{
			display_text($"{enemy.name} attacks, falling for you trap, taking {enemy.damage} damage.");
			await ToSignal(this, nameof(TextClosed));
			current_enemy_health -= enemy.damage;
			glbl.trapped = false;
			setHealth(GetNode<ProgressBar>("EnemyContainer/ProgressBar"), current_enemy_health, MAX_ENEMY_HEALTH);
		}
		else
		{
			display_text($"{enemy.name} attacks, dealing {enemy.damage}"); //this can be edited for multiple attacks. rand + assign rand name and dmg then pass to this and the function continues like normal
			await ToSignal(this, nameof(TextClosed));
			glbl.health = Math.Max(0, glbl.health - enemy.damage);
			setHealth(GetNode<ProgressBar>("PlayerPanel/PlayerData/ProgressBar"), glbl.health, max_player_health);
		
			if (glbl.health == 0) 
			{
				display_text("You Lose.");
				player.EnableMovement = true;
				glbl.isBattling = false; 
				glbl.custom_signals.OnItemUsed -= OnItemUsed;
				QueueFree();
			}
		}
		
	}

	public async void  On_run_pressed()
	{
		display_text("Escaped Successfully!");
		await ToSignal(this, nameof(TextClosed));
		player.EnableMovement = true;
		glbl.isBattling = false; 
		glbl.custom_signals.OnItemUsed -= OnItemUsed;
		QueueFree();
	}
	public async void On_attack_pressed()
	{
		display_text($"You Attack with your {glbl.weapon}, dealing {glbl.damage} damage."); //Weapon names could change here. DMG can change in the state.cs. Still not 100% sure how. Probably by changing it in the main.cs
		await ToSignal(this, nameof(TextClosed));
		current_enemy_health = Math.Max(0, current_enemy_health - glbl.damage);
		setHealth(GetNode<ProgressBar>("EnemyContainer/ProgressBar"), current_enemy_health, MAX_ENEMY_HEALTH);
		
		if (current_enemy_health == 0) 
		{
			display_text("You Win.");
			enemy.defeated = true;
			await ToSignal(this, nameof(TextClosed));
			GetParent().QueueFree();
			glbl.karma -= 500;
			glbl.isBattling = false; 
			player.EnableMovement = true;
			glbl.custom_signals.OnItemUsed -= OnItemUsed;
			QueueFree();
		}
		EnemyTurn();
		
	}
	public async void On_defend_pressed()
	{
		is_defending = true;
		display_text($"You defend against the incoming attack."); //Weapon names can change here. DMG can change in the state.cs. Still not 100% sure how. Probably by changing it in the main.cs
		await ToSignal(this, nameof(TextClosed));
		EnemyTurn();
	}
	public void On_items_pressed()
	{
		if(glbl.isBattling == true ){
			player.inventory_ui.Visible = !player.inventory_ui.Visible;
			player.inventory_money.Text = "Money = " + glbl.money.ToString();
			player.inventory_health.Text = "Health = " + glbl.health.ToString();
			//GetTree().Paused = !GetTree().Paused;
			//JUST FOR DEMO
			}
		
		
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
}
