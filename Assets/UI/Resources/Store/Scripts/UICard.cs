using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
	public static Vector2 sizeCard;

	public int slotID;
	public Text labelTitle;
	public Image buildTypeImage;
	public Text labelCost;
	public Image imageCard;

	public UICardAction actionMy;
	public UICardAction actionEnemy;

	public Text labelText;

	public Transform back;
	public Transform over;
	public Transform pointCardPlay;
	public Transform pointCardDrop;
	public Transform pointCardEnemy;

	public GameObject fader;

	public List<Sprite> buildTypeImages;
	public List<string> buildTypeColors;

	public DC_PlayerData playerData;
	public DC_Card data;

	private int indexBuild;

	private bool down;
	private Vector3 mousePos;
	private Vector3 targetPos;
	private bool played;
	private bool droped;

	

	void Awake()
	{
		EventDispatcher.Add(EventName.LocationTurnNew, LocationTurnNew);
	}

	void Start()
	{
		SetSize();
	}

	public void SetSize()
	{
		if (slotID >= 0)
		{
			RectTransform rtParent = transform.parent.GetComponent<RectTransform>();
			sizeCard = new Vector2((int)rtParent.sizeDelta.x * 0.77f, (int)rtParent.sizeDelta.y);
		}
		
		RectTransform rt = GetComponent<RectTransform>();
		rt.anchorMin = new Vector2(0.5f, 0.5f);
		rt.anchorMax = new Vector2(0.5f, 0.5f);
		rt.sizeDelta = sizeCard;
		rt.anchoredPosition = Vector2.zero;

		if (slotID < 0)
			transform.position = pointCardEnemy.position;
		
		targetPos = transform.position;
	}

	public void SetData(DC_Card data, DC_PlayerData playerData)
	{
		GetComponent<CanvasGroup>().alpha = 0;

		this.data = data;
		this.playerData = playerData;

		indexBuild = data.buildTypeCost.GetHashCode();

		labelTitle.text = "<color=" + buildTypeColors[indexBuild] + ">" + data.title + "</color>";
	
		//labelTitle.text =  data.title;
		buildTypeImage.sprite = buildTypeImages [indexBuild];
		labelCost.text = data.cost.ToString();

//		imageCard.sprite = Resources.Load<Sprite>("Cards/Images/" + data.image);
		imageCard.sprite = data.image;
		imageCard.SetNativeSize();

		actionMy.SetAction(Side.Your, data);
		actionEnemy.SetAction(Side.Enemy, data);

		labelText.text = ParsingActionText();

		if(slotID < 0)
			fader.SetActive(!Location.instance.player2.data.CheckAvailable(data.buildTypeCost, data.cost));
		else
			fader.SetActive(!playerData.CheckAvailable(data.buildTypeCost, data.cost));

		GetComponent<Animator>().SetTrigger("add");

		if (slotID < 0)
			StartCoroutine(PlayEnemyCard());
	}

	int SetDamageValue(Side side, ActionTarget target, Transform root)
	{
		int value = data.GetDamage(side, target);
		if (target == ActionTarget.Damage)
		{
			if(value == 0)
				return 0;
			else
				value = -value;
		}

		string strValue = value.ToString();
		if (value > 0)
			strValue = "+" + strValue;

		GameObject obj_E_0 = root.Find("Equals 0").gameObject;
		GameObject obj_G_0 = root.Find("Greater 0").gameObject;
		GameObject obj_L_0 = root.Find("Less 0").gameObject;

		if(value == 0)
		{
			obj_E_0.GetComponentInChildren<Text>().text = strValue;
			obj_G_0.SetActive(false);
			obj_L_0.SetActive(false);
		}

		if(value > 0)
		{
			obj_E_0.SetActive(false);
			obj_G_0.GetComponentInChildren<Text>().text = strValue;
			obj_L_0.SetActive(false);
		}
		
		if(value < 0)
		{
			obj_E_0.SetActive(false);
			obj_G_0.SetActive(false);
			obj_L_0.GetComponentInChildren<Text>().text = strValue;
		}

		return value;
	}

	string ParsingActionText()
	{
		string result = "";

		foreach (DC_CardAction action in data.actions)
		{
			if(action.target == ActionTarget.Gold ||
			   action.target == ActionTarget.Magic ||
			   action.target == ActionTarget.Barracks)
			{
				string str = action.value.ToString();

				if(action.value > 0)
					str = "+" + str;

				str = "<color=" + buildTypeColors[action.target.GetHashCode()] + ">" + str + "</color>";

				if(action.side != Side.Your)
					str += " " + action.side.ToString();

				switch(action.target)
				{
					case ActionTarget.Gold:
						if(action.arg == ActionArg.Prod)
							str += " Mine Gold";
						else
							str += " Money";
						break;
					case ActionTarget.Magic:
						if(action.arg == ActionArg.Prod)
							str += " Magic Tower";
						else
							str += " Mana";
						break;
					case ActionTarget.Barracks:
						if(action.arg == ActionArg.Prod)
							str += " Barracks";
						else
							str += " Units";
						break;
				}

				if(result != "")
					result += "\n";
				result += str;
				continue;
			}

			if(action.arg == ActionArg.PlayAgain)
			{
				string str = "PlayAgain";

				if(result != "")
					result += "\n";
				result += str;
				continue;
			}
		}

		return result;
	}

	public void CardDown()
	{
		if (Location.curSide == Side.Enemy || played || droped)
			return;

		down = true;
		mousePos = Input.mousePosition;
	}

	public void CardUp()
	{
		down = false;

		if ((transform.position.y - targetPos.y) > 150)
		{
			played = true;
			targetPos = pointCardPlay.position;
		}
		else if ((transform.position.y - targetPos.y) < -70)
		{
			droped = true;
			targetPos = pointCardDrop.position;
		}

		if (played || droped)
			EventDispatcher.SendEvent(EventName.LocationFaderOn);
	}

	void LateUpdate()
	{
		if (!down)
		{
			transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10);
			if((over.position - targetPos).magnitude < 10)
			{
				if(played)
				{
					played = false;
					GetComponent<Animator>().SetTrigger("play");
					EventDispatcher.SendEvent(EventName.LocationCardPlay, data);
					if(slotID >= 0)
						EventDispatcher.SendEvent(EventName.LocationCardGet, slotID);
				}

				if(droped)
				{
					droped = false;
					EventDispatcher.SendEvent(EventName.LocationCardPlay);
					if(slotID >= 0)
						EventDispatcher.SendEvent(EventName.LocationCardGet, slotID);
					CardRemove();
				}
			}
			return;
		}

		Vector2 delta = Input.mousePosition - mousePos;
		transform.Translate(delta);
		mousePos = Input.mousePosition;
	}

	void LocationTurnNew()
	{
		fader.SetActive(!playerData.CheckAvailable(data.buildTypeCost, data.cost));
	}

	public void CardRemove()
	{
		Destroy(gameObject);
	}

	IEnumerator PlayEnemyCard()
	{
		yield return new WaitForSeconds(1);

		played = true;
		targetPos = pointCardPlay.position;
	}

}
