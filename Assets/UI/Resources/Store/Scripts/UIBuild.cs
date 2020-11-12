using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
	public Text labelBuildName;	
	public Text labelBuildProd;
	public Text labelBuildCount;
	public Transform pivotInfo;

	public DC_Build buildData;


	private int prod;
	private int count;

	void Awake()
	{
	}

	public void SetData(DC_Build data)
	{
		buildData = data;
		prod = buildData.prod;
		count = buildData.count;
		labelBuildName.text = data.name;
		labelBuildProd.text = "+" + buildData.prod;
		labelBuildCount.text = buildData.count.ToString();
	}

	void LateUpdate()
	{
		if (buildData == null)
			return;

		if (prod != buildData.prod)
		{
			int delta = buildData.prod - prod;
			prod = buildData.prod;
			labelBuildProd.text = "+" + buildData.prod;
			if (buildData.action)
			{
				EventDispatcher.SendEvent(EventName.TextFly, pivotInfo.position, delta, labelBuildProd.color);
				if(delta > 0)
					BuildSound(buildData);
			}
			buildData.action = false;
		}

		if (count != buildData.count)
		{
			int delta = buildData.count - count;
			count = buildData.count;
			labelBuildCount.text = buildData.count.ToString();
			if (buildData.action)
			{
				EventDispatcher.SendEvent(EventName.TextFly, pivotInfo.position, delta, labelBuildCount.color);
				if(delta > 0)
					BuildSound(buildData);
			}
			buildData.action = false;
		}
	}

	void BuildSound(DC_Build buildData)
	{
		switch (buildData.type)
		{
			case BuildType.Gold:
				SoundManager.SoundPlay(SoundName.Build1);
				break;
			case BuildType.Magic:
				SoundManager.SoundPlay(SoundName.Build2);
				break;
			case BuildType.Barracks:
				SoundManager.SoundPlay(SoundName.Build3);
				break;
		}
	}
}

