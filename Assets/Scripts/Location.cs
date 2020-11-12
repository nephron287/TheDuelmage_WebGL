using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Location : MonoBehaviour
{
	public static Location instance;
	public static Side curSide;		// the current active player

	public Transform slotTower1;	// slot for player models towers
	public Transform slotTower2;	// slot for models enemy tower
	public Transform slotWall1;		// slot for player models wall
	public Transform slotWall2;		// slot for the enemy wall model

	[HideInInspector]
	public Player player1;		// player to the left (the user)
	[HideInInspector]
	public Player player2;		// the player on the right (the enemy)

	public GameObject explosion; // Prefab explosion
	
	private bool playAgain;

	void Awake()
	{
		instance = this;
		EventDispatcher.Add(EventName.LocationCardGet, LocationCardGet);	// event for taking cards from the card
		EventDispatcher.Add(EventName.LocationCardPlay, LocationCardPlay);	// event card in the game
	}

	public void StartGame(Player player, Player enemy)
	{
		// Initialization players right and left sides
		player1 = player; 	// left
		player2 = enemy;	// right

		player1.Init(Side.Your);
		player2.Init(Side.Enemy);

		// adding to the stage towers and walls of player models
		Lib.RemoveObjects(slotTower1);
		Lib.RemoveObjects(slotTower2);
		Lib.RemoveObjects(slotWall1);
		Lib.RemoveObjects(slotWall2);

		player1.data.tower = Lib.AddObject(player1.data.tower, slotTower1);
		player1.data.wall = Lib.AddObject(player1.data.wall, slotWall1);
		player2.data.tower = Lib.AddObject(player2.data.tower, slotTower2);
		player2.data.wall = Lib.AddObject(player2.data.wall, slotWall2);

		// load interface
		UIRoot.CloseAll();
		UIRoot.Load(WindowName.Win_Location, this);
		UIRoot.Load(WindowName.Win_BattleStart);

		// begins to the left side
		curSide = Side.Your;
	}

	// Processing events taking cards from the card
	void LocationCardGet(object[] args)
	{
		int slotID = (int)args [0];

		Card card;
		if(slotID < 0)
			card = player1.data.GetCard();
		else
			card = player2.data.GetCard();

		// add a map to a field in the specified slot
		EventDispatcher.SendEvent(EventName.LocationCardAdd, card, slotID);
	}

	// Event processing "card in game"
	void LocationCardPlay(object[] args)
	{
		SoundManager.SoundPlay(SoundName.CardPlay);

		if (args.Length > 0)
		{
			// there are arguments, the map of the game, rather than the "Discard"
			DC_Card cardData = (DC_Card)args [0];
			PlayCard(cardData);
		}

		// at the end of the game test
		if (CheckEndBattle())
		{
			return;
		}

		if (playAgain)
		{
			playAgain = false;
			EventDispatcher.SendEvent(EventName.CardPlayAgain);

			if (curSide == Side.Enemy)
				// card takes the enemy
				StartCoroutine(GetEnemyCard(3));
			else
				// takes the player card
				EventDispatcher.SendEvent(EventName.LocationFaderOff);

			return;
		}

		// new turn
		StartCoroutine(Turn());
	}

	IEnumerator Turn()
	{
		yield return new WaitForSeconds(1);

		EventDispatcher.SendEvent(EventName.LocationTurnEnd);

		yield return new WaitForSeconds(0.5f);

		// change the side of the current
		if (curSide == Side.Your)
		{
			curSide = Side.Enemy;
			player2.Turn();	// the beginning of the turn to the enemy
		}
		else
		{
			curSide = Side.Your;
			player1.Turn(); // the beginning of the turn for a player
		}
		SoundManager.SoundPlay(SoundName.NewTurn);

		yield return new WaitForSeconds(0.5f);

		// event "start of a new turn"
		EventDispatcher.SendEvent(EventName.LocationTurnNew);

		if (curSide == Side.Enemy)
		{
			// you can write a card selection logic for the enemy (AI)
			// while just taken the first card from the deck
			StartCoroutine(GetEnemyCard(0));
		}
	}

	// procedure for obtaining a card for the enemy
	IEnumerator GetEnemyCard(float time)
	{
		yield return new WaitForSeconds(time);
		EventDispatcher.SendEvent(EventName.LocationCardGet, -1);
	}

	//  card game processing procedure
	void PlayCard(DC_Card cardData)
	{
		playAgain = false;

		// determine the type of card building
		DC_Build buildData;
		if (curSide == Side.Your)
			buildData = player1.data.GetBuild(cardData.buildTypeCost);
		else
			buildData = player2.data.GetBuild(cardData.buildTypeCost);

		if (buildData != null)
		{
			if(buildData.count >= cardData.cost)
			{
				buildData.count -= cardData.cost;
			}
			else
			{
				EventDispatcher.SendEvent(EventName.CardDiscard);
				return;
			}
		}

		DC_PlayerData playerData;

		foreach (DC_CardAction action in cardData.actions)
		{
			if(action.arg == ActionArg.PlayAgain)
			{
				playAgain = true;
				return;
			}

			if(curSide == Side.Your)
			{
				if(action.side == Side.Enemy)
					playerData = player2.data;
				else
					playerData = player1.data;
			}
			else
			{
				if(action.side == Side.Enemy)
					playerData = player1.data;
				else
					playerData = player2.data;
			}

			// Tower Action
			if(action.target == ActionTarget.Tower)
			{
				playerData.tower.data.SetValue(action.value);
			}

			// Wall Action
			if(action.target == ActionTarget.Wall)
			{
				playerData.wall.data.SetValue(action.value);
			}

			// Damage
			if(action.target == ActionTarget.Damage)
			{
				int delta = playerData.wall.data.SetValue(-action.value);
				playerData.tower.data.SetValue(delta);
			}

			// Builds
			if(action.target == ActionTarget.Gold ||
			   action.target == ActionTarget.Magic ||
			   action.target == ActionTarget.Barracks)
			{
				bool colorize = false;
				if(action.arg == ActionArg.Prod)
				{
					colorize = true;
					playerData.GetBuild(action.target.ToString()).SetProd(action.value);
				}
				if(action.arg == ActionArg.Count)
				{
					playerData.GetBuild(action.target.ToString()).SetCount(action.value);
				}
			}
		}
	}

	bool CheckEndBattle()
	{
		if (player1.data.tower.data.health == player1.data.tower.data.healthMax ||
		    player2.data.tower.data.health == 0)
		{
			// Victory
			PlayerPrefs.SetInt("unlock_point" + (Game.gameID + 1), 1);
			SoundManager.SoundPlay(SoundName.BattleVictory, 2);
			UIRoot.Load(WindowName.Win_Victory, 1.5f);
			return true;
		}

		if (player2.data.tower.data.health == player2.data.tower.data.healthMax ||
		    player1.data.tower.data.health == 0)
		{
			// Defeat
			SoundManager.SoundPlay(SoundName.BattleDefeat, 2);
			UIRoot.Load(WindowName.Win_Defeat, 1.5f);
			return true;
		}

		return false;
	}

}
