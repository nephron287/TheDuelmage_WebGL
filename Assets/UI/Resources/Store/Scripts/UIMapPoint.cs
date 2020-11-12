using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMapPoint : MonoBehaviour
{
	public GameObject objLock;
	public int pointID;

	private bool unlock;

	void Start()
	{
		unlock = PlayerPrefs.GetInt("unlock_point" + pointID, 0) == 1;
		if(objLock != null)
		   objLock.SetActive(!unlock);
	}

	public void SelectGame()
	{
		SoundManager.SoundPlay(SoundName.ButtonClick2);

		if (unlock)
			EventDispatcher.SendEvent(EventName.LocationStartGame, pointID);
		else
			UIRoot.Load(WindowName.Win_Message, "We must pass the previous level");
	}

}
