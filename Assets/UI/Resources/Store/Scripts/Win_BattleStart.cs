using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Win_BattleStart : MonoBehaviour
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
		UIRoot.Close(gameObject);
	}

}
