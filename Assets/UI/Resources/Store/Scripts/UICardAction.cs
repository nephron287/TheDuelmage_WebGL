using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardAction : MonoBehaviour
{
	public Transform rootTower;
	public Transform rootWall;
	public Transform rootAll;

	void Awake()
	{
		foreach (Transform child in rootTower)
			child.gameObject.SetActive(false);
		foreach (Transform child in rootWall)
			child.gameObject.SetActive(false);
		foreach (Transform child in rootAll)
			child.gameObject.SetActive(false);
	}

	public void SetAction(Side side, DC_Card data)
	{

		int value = data.GetDamage(side, ActionTarget.Damage);

		if (value != 0)
		{
			SetValue(rootAll, "Less 0", -value);
			return;
		}

		value = data.GetDamage(side, ActionTarget.Tower);
		if (value != 0)
		{
			if(value > 0)
				SetValue(rootTower, "Greater 0", value);
			else
				SetValue(rootTower, "Less 0", value);
		}
		else
		{
			SetValue(rootTower, "Equals 0", value);
		}

		value = data.GetDamage(side, ActionTarget.Wall);
		if (value != 0)
		{
			if(value > 0)
				SetValue(rootWall, "Greater 0", value);
			else
				SetValue(rootWall, "Less 0", value);
		}
		else
		{
			SetValue(rootWall, "Equals 0", value);
		}
	}

	void SetValue(Transform root, string name, int value)
	{
		GameObject objInfo = root.Find(name).gameObject;
		objInfo.SetActive(true);
		if (value != 0)
		{
			if (value > 0)
				objInfo.GetComponentInChildren<Text>().text = "+" + value;
			else
				objInfo.GetComponentInChildren<Text>().text = value.ToString();
		}
		else
		{
			objInfo.GetComponentInChildren<Text>().text = "0";
		}
	}
}
