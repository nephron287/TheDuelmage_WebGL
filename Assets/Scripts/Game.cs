using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public static int gameID;
	public static Player player;
	public static DC_PlayerConfig playerConfig;

	void Awake()
	{
		// Unlock the first point on the map
		PlayerPrefs.SetInt("unlock_point" + 1, 1);

		// Form the details of the player
		player = Instantiate<Player>(Resources.Load<Player>("Players/Player"));
		player.data.playerName = PlayerPrefs.GetString("playerName", "Player 1");

		playerConfig = DC_PlayerConfig.GetNewConfig();
	}

	void Start ()
	{
//		Screen.SetResolution(1280, 720, false);
//		Screen.SetResolution(1024, 768, false);

		UIRoot.Load (WindowName.Win_MainMenu);
		SoundManager.SoundPlay(SoundName.Intro);
	}
}

[System.Serializable]
public class DC_PlayerConfig
{
	public List<int> buildProds;	// changes in production buildings
	public List<int> buildCounts;	// changing the initial amount of resources
	public int freePoints;			// the remaining number of points for the distribution of

	public static DC_PlayerConfig GetNewConfig()
	{
		DC_PlayerConfig newConfig = new DC_PlayerConfig();
		newConfig.buildProds = new List<int>(){0,0,0};
		newConfig.buildCounts = new List<int>(){0,0,0};
		newConfig.freePoints = 5;
		return newConfig;
	}

	// Change settings in the production of buildings
	public void Prod(int buildID, int value)
	{
		ChangePoints(buildProds, buildID, value);
	}
	
	// Changing the settings of the initial amount of resources
	public void Count(int buildID, int value)
	{
		ChangePoints(buildCounts, buildID, value);
	}
	
	void ChangePoints(List<int> list, int index, int value)
	{
		if (value > 0 && freePoints > 0)
		{
			freePoints--;
			list [index] += value;
		}
		
		if (value < 0 && list [index] > 0)
		{
			freePoints++;
			list [index] += value;
		}
	}
}