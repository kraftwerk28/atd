using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMoney : MonoBehaviour {


	public void Change(int m)
	{
		GetComponent<Text>().text = "Money: " + m.ToString();
	}
}
