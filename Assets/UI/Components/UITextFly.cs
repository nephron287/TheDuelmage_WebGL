using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextFly : MonoBehaviour
{
	public Text labelText;

	public void SetData(Vector3 pos, string text, Color color)
	{
		transform.position = pos;
		labelText.text = text;
		labelText.color = color;
	}

	public void Remove()
	{
		Destroy(gameObject);
	}
}
