using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoard : MonoBehaviour
{
	public List<Transform> slots;
	public UICard prefCard;
	public GameObject fader;

	public DC_PlayerData playerData;

	private Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
		prefCard.transform.SetParent(transform);
		prefCard.gameObject.SetActive(false);

		EventDispatcher.Add(EventName.LocationCardAdd, LocationCardAdd);
		EventDispatcher.Add(EventName.LocationTurnEnd, LocationTurnEnd);
		EventDispatcher.Add(EventName.LocationTurnNew, LocationTurnNew);
		EventDispatcher.Add(EventName.LocationFaderOn, FaderOn);
		EventDispatcher.Add(EventName.LocationFaderOff, FaderOff);
		EventDispatcher.Add(EventName.ResolutionChanged, ResolutionChanged);
	}

	public void SetData(DC_PlayerData data)
	{
		playerData = data;

		for(int i=0; i<6; i++)
			EventDispatcher.SendEvent(EventName.LocationCardGet, i);
	}

	void LocationCardAdd(object[] args)
	{
		Card card = (Card)args [0];
		int slotID = (int)args [1];

		UICard uiCard;
		if(slotID < 0)
			uiCard = Lib.AddObject<UICard>(prefCard, transform, true);
		else
			uiCard = Lib.AddObject<UICard>(prefCard, slots [slotID], true);

		uiCard.slotID = slotID;
		uiCard.SetData(card.data, playerData);
	}

	void LocationTurnEnd()
	{
		if (Location.curSide == Side.Your)
			anim.SetTrigger("your off");
		else
			anim.SetTrigger("enemy off");
	}

	void LocationTurnNew()
	{
		if (Location.curSide == Side.Your)
			anim.SetTrigger("your on");
		else
			anim.SetTrigger("enemy on");
	}

	public void FaderOff()
	{
		fader.SetActive(false);
	}

	public void FaderOn()
	{
		fader.SetActive(true);
	}

	void ResolutionChanged()
	{
		StartCoroutine(ResChanged());
	}

	IEnumerator ResChanged()
	{
		yield return new WaitForSeconds(0.1f);

		foreach (Transform slot in slots)
		{
			slot.GetComponentInChildren<UICard>().SetSize();
		}
	}
}

