using Godot;
using System;

/*
Summary:
Player script. Contains methods for movement, animations and input registration for opening the inventory.
*/
public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	//The parts of the sprite sheet used for this character.
	public AnimatedSprite2D char_anim;
	//CanvasLayer for the notification to pick up an item.
	[Export]
	public CanvasLayer interact_ui;
	//CanvasLayer for the screen that appears when you open the inventory.
	[Export]
	public CanvasLayer inventory_ui;
	[Export]
	public Label interact_text;

	[Export]
	public CanvasLayer shop_ui;
	[Export]
	public CanvasLayer karma_ui;
	[Export]
	public Label karma_label;
	[Export]
	public Label inventory_money;
	[Export]
	public Label inventory_health;

	public CanvasLayer Presstalk;
	//Pause Alternative
	public bool EnableMovement = true;
	public bool shop_openable = false;
	public Camera2D camera;
	
	//Global Reference
	//Global Reference
	public Global glbl;
	
	
	/*
	Summary:
	Called when the node enters the scene tree for the first time. 
	Assigns the character animations, Player reference, and both UIs.
	*/
	public override void _Ready()
	{
		glbl = GetNode<Global>("/root/Global");
		camera = GetNode<Camera2D>("Camera2D");
		char_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		glbl.SetPlayerReference(this, this);
		interact_ui = GetNode<CanvasLayer>("InteractUI");
		interact_text = GetNode<Label>("InteractUI/ColorRect/Label");
		inventory_ui = GetNode<CanvasLayer>("InventoryUI");
		shop_ui = GetNode<CanvasLayer>("ShopUI");
		karma_ui = GetNode<CanvasLayer>("KarmaUI");
		karma_label = GetNode<Label>("KarmaUI/ColorRect/Label");
		inventory_money = GetNode<Label>("InventoryUI/ColorRect/Money");
		inventory_health = GetNode<Label>("InventoryUI/ColorRect/Health");
		Presstalk = GetNode<CanvasLayer>("Presstalk");

	}
	
	/*
	Summary:
	Movement script. Moves the character based on input. No acceleration or deceleration.
	Updates the animations according to the speed of the character (see UpdateAnimations())
	Params:
	delta: the amount of time since the last update.
	*/
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		//Moves at constant speed based on the vector of the input. If no movement, decelerates to 0. 
		//Note: Character currently decelerates in one frame, effectively stopping instantly.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (EnableMovement == true){
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
				velocity.Y = direction.Y * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			}

			Velocity = velocity;
			MoveAndSlide();
			UpdateAnimations();
			}
		
	}
	/*
	Summary:
	Updates the character's animations to reflect their movement.
	*/
	public void UpdateAnimations()
	{
		Vector2 velocity = Velocity;
		if(velocity == Vector2.Zero)
		{
			char_anim.Play("idle");
		} else if (velocity.X > 0)
		{
			char_anim.Play("walk_right");
			char_anim.FlipH = false;
		} else
		{
			char_anim.Play("walk_left");
			char_anim.FlipH = true;
		}
	}
	/*
	Summary:
	Godot signal for when the player enters range. Allows the player to pick up the item, and displays a notification that the item can be picked up.
	Params:
	@event: Godot event variable, used for things like button presses.
	*/
	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("ui_inventory"))
		{
			EnableMovement = !EnableMovement;
			inventory_ui.Visible = !inventory_ui.Visible;
			inventory_money.Text = "Money = " + glbl.money.ToString();
			inventory_health.Text = "Health = " + glbl.health.ToString();
		}
		
		if(@event.IsActionPressed("ui_interact") && shop_openable == true)
		{
			EnableMovement = !EnableMovement;
			shop_ui.Visible = !shop_ui.Visible;
			glbl.custom_signals.EmitSignal(nameof(CustomSignals.OnShopOpened));
		}
		if(@event.IsActionPressed("ui_karma"))
		{
			EnableMovement = !EnableMovement;
			karma_ui.Visible = !karma_ui.Visible;
			karma_label.Text = "Karma = " + glbl.karma.ToString() + "\n";
		}
	}
}
