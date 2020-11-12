using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public Transform pointTarget;	// point FlyText and Explosion
	public int posMin;				// position when health to 0
	public int posMax;				// position when health to Max
	public DC_Health data;

	private int level;				

	void Start()
	{
		SetLevel();
	}

	void LateUpdate()
	{
		if (data.healthMax == 0)
			return;

		Vector3 pos = new Vector3(transform.localPosition.x, level, transform.localPosition.z);
		transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * 5);
	}

	public void SetLevel()
	{
		level = posMin + (int)((posMax - posMin) * data.health / (float)data.healthMax);
	}

	public IEnumerator PlayFX(int value)
	{
		if (value == 0)
			yield break;

		Vector3 pos = Camera.main.WorldToScreenPoint(pointTarget.position);
		if (value > 0)
		{
			// Healing
			SoundManager.SoundPlay(SoundName.Health);
			EventDispatcher.SendEvent(EventName.TextFly, pos, value, 4);

			SetLevel();
		}
		else
		{
			// Damage
			SoundManager.SoundPlay(SoundName.Damage);
			yield return new WaitForSeconds(0.6f);
			GameObject exp = Instantiate(Resources.Load<GameObject>("FX/Explosion"));
			exp.transform.position = pointTarget.position;
			EventDispatcher.SendEvent(EventName.TextFly, pos, value, 5);

			SetLevel();
		}

	}

}

[System.Serializable]
public class DC_Health
{
	public int health;		// current health
	public int healthMax;	// max health

	public int SetValue(int value)
	{
		int delta = 0;

		health += value;
		if (health < 0)
		{
			delta = health;
			health = 0;
		}
		if (health > healthMax)
			health = healthMax;

		return delta;
	}
}
