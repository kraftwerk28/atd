using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	public float GamageDist = 6;
	public float damage;

	public ParticleSystem fire_p;
	public ParticleSystem explo_p;

	private Quaternion lookat;
	private Transform _target;
	// Use this for initialization
	void Start ()
	{
		GetComponent<Rigidbody>().maxAngularVelocity = 1;
		Invoke("StartAI", 2f);
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().AddForce(transform.forward * 100);
	}

	void StartAI()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		fire_p.Stop();
		transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
		InvokeRepeating("Aim", 0, 0.5f);
	}

	void Aim()
	{
		if (_target)
			transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
	}

	public void SelectTarget(Transform tg)
	{
		_target = tg;
	}

	private void OnCollisionEnter(Collision collision)
	{
		StartCoroutine("StartDestroy");
	}

	private IEnumerator StartDestroy()
	{
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Collider>().enabled = false;
		foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
			r.enabled = false;
		GetComponent<AudioSource>().Play();
		GameObject[] en = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject g in en)
		{
			if (Vector3.Distance(g.transform.position, transform.position) <= GamageDist)
				g.SendMessage("GiveDamage", damage);
		}

		explo_p.Play();
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);

	}
}
