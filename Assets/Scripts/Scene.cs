using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
	public Location scene;
	public Transform camPivot;

	void Awake()
	{
		Lib.RemoveObjects(transform);

		EventDispatcher.Add(EventName.LocationStartGame, LocationStartGame);
		EventDispatcher.Add(EventName.LocationEndGame, LocationEndGame);
	}

	void LocationStartGame(object[] args)
	{
		SoundManager.SoundPlay(SoundName.BattleStart);

		// We obtain the ID loaded location
		Game.gameID = (int)args [0];
		if (Game.gameID == 0)
			return;

		// We are looking for a point on the map and load the scene with the parameters of this point
		MapPoint[] mapPoints = GameObject.FindObjectsOfType<MapPoint>();

		foreach (MapPoint mapPoint in mapPoints)
		{
			if(mapPoint.ID != Game.gameID)
				continue;

			// We found the desired point
			scene = Lib.AddObject<Location>(mapPoint.location, transform);

			// We set the parameters of the player
			Player player = Instantiate<Player>(Game.player);
			player.SetParams(Game.playerConfig);

			// We set the parameters of the enemy
			Player enemy = Instantiate<Player>(mapPoint.enemy);
			enemy.RandomParams();

			// we start a fight
			scene.StartGame(player, enemy);
			break;
		}

	}

	void LocationEndGame()
	{
		Lib.RemoveObjects(transform);
	}

	void LateUpdate()
	{
		// turning the camera according to the current of side
		if(Location.curSide == Side.Your)
			camPivot.localRotation = Quaternion.Lerp(camPivot.localRotation, Quaternion.Euler(8, 15, 0), Time.deltaTime * 2);
		else
			camPivot.localRotation = Quaternion.Lerp(camPivot.localRotation, Quaternion.Euler(8, -15, 0), Time.deltaTime * 2);
	}

}
