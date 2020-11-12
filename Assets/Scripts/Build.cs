using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
	public DC_Build data;

	// end of turn processing procedure for buildings
	public void Turn()
	{
		data.count += data.prod;
	}
}

[System.Serializable]
public class DC_Build
{
	public Side side;		// It refers to which side of the building
	public BuildType type;	// building type
	public string name;		// name
	public int prod;		// production rate
	public int count;		// the amount of resources in stock

	[HideInInspector]
	public bool action;

	public void SetProd(int value)
	{
		action = true;
		prod += value;
		if(prod < 0)
			prod = 0;
	}

	public void SetCount(int value)
	{
		action = true;
		count += value;
		if(count < 0)
			count = 0;
	}
}

[System.Serializable]
public enum BuildType
{
	None,		//
	Gold,		// gold mine
	Magic,		// magic tower
	Barracks,	// barracks
}

