using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Win_Location : MonoBehaviour
{
	public UIPlayer player1;
	public UIPlayer player2;
	public UIBoard uiBoard;
	public Transform pivotInfo;

	void Awake()
	{
		EventDispatcher.Add(EventName.CardDiscard, CardDiscard);
		EventDispatcher.Add(EventName.CardPlayAgain, CardPlayAgain);
	}

	void Start()
	{
		UIRoot.Show(gameObject);
	}

	void CardDiscard()
	{
		EventDispatcher.SendEvent(EventName.TextFly, pivotInfo.position, "Discard", 5);
	}

	void CardPlayAgain()
	{
		EventDispatcher.SendEvent(EventName.TextFly, pivotInfo.position, "Play Again", 4);
	}

	void SetData(object[] args)
	{
		Location board = (Location)args [0];
		player1.SetData(board.player1.data);
		player2.SetData(board.player2.data);
		StartCoroutine(SetLocation(board.player1.data));
	}

	IEnumerator SetLocation(DC_PlayerData data)
	{
		yield return new WaitForSeconds(0.5f);
		uiBoard.SetData(data);
	}

	public void MainMenu()
	{
		EventDispatcher.SendEvent(EventName.LocationEndGame);
		UIRoot.CloseAll();
		UIRoot.Load(WindowName.Win_MainMenu);
	}
}
