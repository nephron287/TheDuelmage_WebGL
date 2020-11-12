using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Win_Victory : MonoBehaviour
{
	void Awake()
	{
	}

	void Start()
	{
		UIRoot.Show(gameObject);
	}

	void SetData()
	{

	}

	public void Close()
	{
		EventDispatcher.SendEvent(EventName.LocationEndGame);
		UIRoot.CloseAll();
		UIRoot.Load(WindowName.Win_MainMenu);
	}

}
