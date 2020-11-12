using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextFlyManager : MonoBehaviour
{
	public UITextFly pref;
	public List<Color> colors;

	void Awake()
	{
		pref.gameObject.SetActive(false);

		EventDispatcher.Add(EventName.TextFly, TextFly);
	}

	void TextFly(object[] args)
	{
		// Position
		Vector3 pos = (Vector3)args [0];
		pos.z = 0;

		// Text
		string text = "";
		if (args [1].GetType() == typeof(string))
		{
			text = (string)args [1];
		}
		else
		{
			int value = (int)args[1];
			if(value > 0)
				text = "+" + value;
			else
				text = value.ToString();
		}

		// Color
		Color color = Color.white;
		if (args [2].GetType() == typeof(int))
		{
			int indexColor = (int)args [2];
			if (indexColor > 0 && indexColor < colors.Count)
				color = colors [indexColor];
		}
		else
		{
			color = (Color)args [2];
		}

		Lib.AddObject<UITextFly>(pref, transform, true).SetData(pos, text, color);
	}
}
