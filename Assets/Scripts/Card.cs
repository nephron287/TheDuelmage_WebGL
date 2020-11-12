using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Card : MonoBehaviour
{
	public DC_Card data;

	void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			data.title = name;
		}
	}
}

[System.Serializable]
public class DC_Card
{
	public string title;			// name card
	public Sprite image;			// image
	public BuildType buildTypeCost;	// building type for card payment
	public int cost;				// card cost

	public List<DC_CardAction> actions;	// action when activated

	public int GetDamage(Side side, ActionTarget target)
	{
		foreach (DC_CardAction action in actions)
		{
			if(action.side == side && action.target == target)
				return action.value;
		}
		return 0;
	}
}

[System.Serializable]
public class DC_CardAction
{
	public Side side;			// the player on whom the action is directed
	public ActionTarget target;	// To Do
	public ActionArg arg;
	public int value;
}

public enum Side
{
	Your = 0,
	Enemy = 1,
}

[System.Serializable]
public enum ActionTarget
{
	None,
	Gold,		// gold mine
	Magic,		// magic tower
	Barracks,	// barracks
	Tower,		// tower Player
	Wall,		// Player wall
	Damage,		// applying damage (wall, then the tower)
}

[System.Serializable]
public enum ActionArg
{
	None,		
	Prod,		// resource production rate change
	Count,		// changing the number of resources in a building
	PlayAgain,	
}

