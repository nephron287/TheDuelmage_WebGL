using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
	public float timer = 5;

	void Start ()
	{
		Destroy(gameObject, timer);
	}
}
