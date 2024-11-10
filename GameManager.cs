using Godot;
using System;

public partial class GameManager : Node2D
{
	private int positiveChoices = 0;
	private int negativeChoices = 0;

	public void RecordChoice(bool positive)
	{
		if (positive)
			positiveChoices++;
		else
			negativeChoices++;
	}

	public void DetermineEnding()
	{
		if (positiveChoices >= 3)
		{
			GD.Print("Good Ending: You helped everyone. Everyone is grateful!");
		}
		else if (negativeChoices >= 3)
		{
			GD.Print("Bad Ending: You ignored everyone. They are disappointed.");
		}
		else
		{
			GD.Print("Neutral Ending: Some people appreciate your help, some do not.");
		}
	}
}
