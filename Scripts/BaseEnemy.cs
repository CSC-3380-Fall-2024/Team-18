using Godot;
using System;

public partial class BaseEnemy : Resource
{
	[Export]
	public string name = "Enemy";
	[Export]
	public Texture texture = null;
	[Export]
	public int health = 30;
	[Export]
	public int damage = 20;
}
