using UnityEngine;
using System.Collections;

public class Freezer_g : MonoBehaviour
{
	public Transform Range;
	void Start()
	{
		Range.localScale = Vector3.one * GetComponent<Freezer>().range / 15;
	}

	void Update()
	{
		
	}
}