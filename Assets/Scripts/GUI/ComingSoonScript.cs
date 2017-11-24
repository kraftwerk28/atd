using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComingSoonScript : MonoBehaviour
{
	
	void Start()
	{
		GetComponent<Text>().enabled = false;
	}

	public void ShowHide()
	{
		StartCoroutine(Sh());
	}

	IEnumerator Sh()
	{
		GetComponent<Text>().enabled = true;
		yield return new WaitForSeconds(1);
		GetComponent<Text>().enabled = false;
	}
}