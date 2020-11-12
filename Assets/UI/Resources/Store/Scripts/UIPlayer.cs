using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
	public Text playerName;
	public List<UIBuild> builds;
	public Text labelTower;
	public Text labelWall;

	public DC_PlayerData playerData;

	private Camera cam;
	private DC_Health towerData;
	private DC_Health wallData;
	private int towerHealth;
	private int wallHealth;

	void Awake()
	{
		cam = Camera.main;
	}

	public void SetData(DC_PlayerData data)
	{
		playerData = data;
		playerName.text = data.playerName;

		towerData = playerData.tower.data;
		towerHealth = towerData.health;
		wallData = playerData.wall.data;
		wallHealth = wallData.health;

		for (int i=0; i<playerData.listBuilds.Count; i++)
		{
			builds[i].SetData(playerData.listBuilds[i].data);
		}

		labelTower.text = towerData.health + " / " + towerData.healthMax;
		labelWall.text = wallData.health + " / " + wallData.healthMax;
	}

	void LateUpdate()
	{
		if (playerData == null || playerData.tower == null || playerData.wall == null)
			return;

		SetPositionComponent(playerData.tower.transform.position, labelTower.transform);
		SetPositionComponent(playerData.wall.transform.position, labelWall.transform);

		if (towerHealth != towerData.health)
		{
			int delta = towerData.health - towerHealth;
			towerHealth = towerData.health;
			labelTower.text = towerData.health + " / " + towerData.healthMax;

			// Playing effects
			StartCoroutine(playerData.tower.PlayFX(delta));
		}

		if (wallHealth != wallData.health)
		{
			int delta = wallData.health - wallHealth;
			wallHealth = wallData.health;
			labelWall.text = wallData.health + " / " + wallData.healthMax;
			
			// Playing effects
			StartCoroutine(playerData.wall.PlayFX(delta));
		}
	}

	void SetPositionComponent(Vector3 target, Transform component)
	{
		Vector3 pos = cam.WorldToScreenPoint(target);
		pos.x = (int)pos.x;
		pos.y = component.position.y;
		pos.z = component.position.z;
		component.position = pos;
	}
}
