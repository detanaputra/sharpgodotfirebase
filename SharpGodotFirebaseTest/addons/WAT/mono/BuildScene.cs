using Godot;
using System;

public partial class BuildScene : Node
{
	public override void _Ready() 
	{ 
		GetTree().Quit(); 
	}
}
