using UnityEngine;
using System.Collections;

public class Cannon_g : MonoBehaviour
{
	public Transform Range;
	void Start()
	{
		Range.localScale = Vector3.one * GetComponent<Cannon>().range / 15;
	}

	void Update()
	{
		
	}
}