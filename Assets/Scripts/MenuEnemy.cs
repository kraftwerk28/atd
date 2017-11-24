using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnemy : MonoBehaviour {

	private Vector3[] points = { new Vector3(-70, 25, 80), new Vector3(-180, -20 -10), new Vector3(-70, 25, -150) };
	private int i = 0;
	// Use this for initialization
	void Start () {
		InvokeRepeating("Move", 0f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, points[i], 1f);
	}

	void Move()
	{
		i = (i + 1) % 3;
	}
}
