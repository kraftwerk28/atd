using UnityEngine;
using System.Collections;

public class Minigun_g : MonoBehaviour
{
	public Transform Range;
	void Start()
	{
		Range.localScale = Vector3.one * GetComponent<Minigun>().range / 15;
	}

	void Update()
	{
		
	}
}