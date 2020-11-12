using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
	public DC_PlayerData data;	// player data

	public void Init(Side side)
	{
		data.side = side;

		// forming a deck of cards
		data.listCards = GetListCards();
	}

	// the formation of a deck of cards
	List<Card> GetListCards()
	{
		List<Card> list = Resources.LoadAll<Card>("Cards").ToList();
		
		for (int i=0; i<list.Count; i++)
		{
			int rnd = UnityEngine.Random.Range(0, list.Count);
			Card tmp = list[i];
			list[i] = list[rnd];
			list[rnd] = tmp;
		}

		return list;
	}

	public void Turn()
	{
		foreach (Build build in data.listBuilds)
		{
			// update the parameters of buildings (update resources)
			build.Turn();
		}
	}

	// setting initial parameters
	public void SetParams(DC_PlayerConfig config)
	{
		for (int i=0; i<3; i++)
		{
			data.listBuilds[i].data.prod += config.buildProds[i];
			data.listBuilds[i].data.count += config.buildCounts[i];
		}
	}

	// a randomly setting the initial parameters for bots
	public void RandomParams()
	{
		for (int i=0; i<5; i++)
		{
			int buildID = UnityEngine.Random.Range(0, 3);

			if(UnityEngine.Random.Range(0, 2) == 0)
				data.listBuilds[buildID].data.prod += 1;
			else
				data.listBuilds[buildID].data.count += 3;
		}
	}
	
}

[System.Serializable]
public class DC_PlayerData
{
	public Side side;
	public string playerName;
	public Tower tower;
	public Wall wall;
	public List<Build> listBuilds;
	public List<Card> listCards;

	// take the first card from the deck
	public Card GetCard()
	{
		Card card = listCards [0];
		listCards.RemoveAt(0);
		listCards.Add(card);
		return card;
	}

	public DC_Build GetBuild(string buildType)
	{
		BuildType bt = (BuildType)Enum.Parse(typeof(BuildType), buildType);
		return GetBuild(bt);
	}

	public DC_Build GetBuild(BuildType buildType)
	{
		foreach (Build build in listBuilds)
		{
			if(build.data.type == buildType)
				return build.data;
		}
		return null;
	}
	
	public bool CheckAvailable(BuildType buildType, int value)
	{
		foreach (Build build in listBuilds)
		{
			if(build.data.type == buildType)
				return build.data.count >= value;
		}
		return true;
	}
}
