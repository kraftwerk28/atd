using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour {
	public void Press()
	{
		GetComponent<AudioSource>().Play();
	}
}
