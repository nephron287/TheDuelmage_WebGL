using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundName
{
	Intro,
	ButtonClick1,
	ButtonClick2,
	MapSelect,
	BattleStart,
	BattleDefeat,
	BattleVictory,
	Build1,
	Build2,
	Build3,
	Health,
	Damage,
	CardPlay,
	NewTurn,
}

[System.Serializable]
public class DC_Sound
{
	public SoundName soundName;
	public AudioClip soundClip;
}