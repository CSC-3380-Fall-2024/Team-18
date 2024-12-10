using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class BaseDialogue : Resource
{
	[Export]
    public string dialogue  { get; set; }
    [Export]
    public BaseOptions[] options = new BaseOptions[4];
}
