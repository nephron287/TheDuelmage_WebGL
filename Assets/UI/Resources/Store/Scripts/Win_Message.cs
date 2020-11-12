using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Win_Message : MonoBehaviour
{
	public Text labelText;

	void Start()
	{
		UIRoot.Show(gameObject);
	}

	void SetData(object[] args)
	{
		labelText.text = (string)args [0];
	}

	public void Close()
	{
		UIRoot.Close(gameObject);
	}

}
