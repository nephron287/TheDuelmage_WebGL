using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;
	public static List<AudioSource> list;

	public List<DC_Sound> listSounds;

	void Awake()
	{
		instance = this;
	}

	public static void SoundPlay(SoundName soundName, float delay = 0)
	{
		AudioClip clip = GetSound(soundName);
		if (clip == null)
			return;

		AudioSource audio = instance.gameObject.AddComponent<AudioSource>();
		audio.clip = clip;
		delay = delay * clip.frequency;
		audio.Play ((ulong)delay);
		
		if (list == null)
			list = new List<AudioSource>();
		
		list.Add(audio);
	}

	static AudioClip GetSound(SoundName soundName)
	{
		foreach (DC_Sound item in instance.listSounds)
		{
			if (item.soundName == soundName)
				return item.soundClip;
		}
		return null;
	}

	void LateUpdate()
	{
		if (list == null)
			return;

		list.RemoveAll(item => item == null);

		foreach (AudioSource item in list)
		{
			if(!item.isPlaying)
			{
				list.Remove(item);
				Destroy(item);
				break;
			}
		}
	}

}
