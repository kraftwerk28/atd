using UnityEngine;
using System.Collections;

public class Laser_g : MonoBehaviour
{
	public Transform Range;
	void Start()
	{
		Range.localScale = Vector3.one * GetComponent<Laser>().range / 15;
	}

	void Update()
	{
		
	}
}