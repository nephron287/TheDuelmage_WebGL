using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Win_MainMenu : MonoBehaviour
{
	public int stepProd = 1;
	public int stepCount = 3;

	public InputField labelName;
	public Text labelFreePoints;
	public List<UIMainMenuBuild> listBuilds;

	void Awake()
	{
	}

	void Start()
	{
		UIRoot.Show(gameObject);

		UpdateBuildsData();
	}

	public void ChangeName()
	{
		Game.player.data.playerName = labelName.text;
		PlayerPrefs.SetString("playerName", labelName.text);
	}

	public void ChangeParams(string value)
	{
		string[] arrays = Lib.ParseStr(value, "|");
		int buildID = Lib.ConvertToInt(arrays [0]) - 1;

		if (arrays [1] == "+")
		{
			if (arrays [2] == "prod")
				Game.playerConfig.Prod(buildID, stepProd);
			else
				Game.playerConfig.Count(buildID, stepCount);
		}
		else
		{
			if (arrays [2] == "prod")
				Game.playerConfig.Prod(buildID, -stepProd);
			else
				Game.playerConfig.Count(buildID, -stepCount);
		}

		SoundManager.SoundPlay(SoundName.ButtonClick1);
		UpdateBuildsData();
	}

	void UpdateBuildsData()
	{
		if (Game.player == null)
			return;

		labelName.text = Game.player.data.playerName;
		labelFreePoints.text = Game.playerConfig.freePoints.ToString();

		for (int i=0; i<3; i++)
		{
			listBuilds [i].SetData(Game.player.data.listBuilds [i].data,
			                       Game.playerConfig.buildProds[i],
			                       Game.playerConfig.buildCounts[i]);
		}
	}
}


