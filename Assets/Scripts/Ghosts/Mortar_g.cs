using UnityEngine;
using System.Collections;

public class Mortar_g : MonoBehaviour
{
	public Transform Range;
	void Start()
	{
		Range.localScale = Vector3.one * GetComponent<Mortar>().range / 15;
	}

	void Update()
	{
		
	}
}