using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	public Image hlth;

	public float Value;
	public float MaxValue;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Canvas>().enabled)
		{
			Colorize();
			transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
		}

	}

	void Colorize()
	{
		float half = MaxValue / 2;
		float R;
		float G;
		if (Value < half)
		{
			G = Value / half;
			R = 1;
		}
		else
		{
			R = (2 * half - Value) / half;
			G = 255;
		}
		hlth.color = new Color(R, G, 0);
		hlth.fillAmount = Value / MaxValue;
	}
	
}
