using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_bulletcol : MonoBehaviour {

	public int damage = 50;

	// Use this for initialization
	void Start()
	{
		transform.SetParent(null);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Enemy")
			col.gameObject.SendMessage("GiveDamage", damage);
		Destroy(gameObject);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
			other.gameObject.SendMessage("GiveDamage", damage);
		Destroy(gameObject);
	}

}
