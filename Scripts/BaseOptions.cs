using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class BaseOptions : Resource
{
    [Export]
    public string text {get; set;}
    [Export]
    public BaseEnemy battle_enemy  { get; set; }
    [Export]
    public int karma  {get; set;}
    [Export]
    public string[] effects {get; set;}
    [Export]
    public ItemCreator item_info {get; set;}
}
