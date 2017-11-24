using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine.UI;

public class BTR_AI : MonoBehaviour, IEnemy
{

	public float Health = 30;
	public float MoneyAdd = 10;
	public float speed = .5f;
	
	public HealthBar hb;
	public Renderer FreezeCube;
	public ParticleSystem Smoker;
	public GameObject Exploder;

	private bool isFreezed = false;
	private float MaxHealth;
	private float _health
	{
		get
		{
			return Health;
		}
		set
		{
			Health = value;
			StartCoroutine(ShowBar());
		}
	}
	// Use this for initialization
	void Start()
	{
		MaxHealth = Health;
		GetComponent<NavMeshAgent>().speed = speed;
		GetComponent<NavMeshAgent>().SetDestination(GameObject.Find("Destination").transform.position);
		hb.GetComponent<Canvas>().enabled = false;
		hb.Value = _health;
		hb.MaxValue = MaxHealth;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void GiveDamage(float dmg)
	{
		_health -= dmg;
		if (_health <= 0)
			StartCoroutine(Destroy());
	}//IEnemy

	public IEnumerator Freeze(float t)
	{
		if (!isFreezed)
		{
			isFreezed = true;
			float _speed = GetComponent<NavMeshAgent>().speed;
			GetComponent<NavMeshAgent>().speed *= 0.2f;
			FreezeCube.enabled = true;

			yield return new WaitForSeconds(t);

			isFreezed = false;
			GetComponent<NavMeshAgent>().speed = _speed;
			FreezeCube.enabled = false;
		}

	}//IEnemy

	IEnumerator ShowBar()
	{
		hb.Value = _health;
		hb.MaxValue = MaxHealth;
		hb.GetComponent<Canvas>().enabled = true;
		yield return new WaitForSeconds(3);
		hb.GetComponent<Canvas>().enabled = false;
	}

	IEnumerator Destroy()
	{
		GameObject.FindGameObjectWithTag("MainCamera").SendMessage("AddMoney", MoneyAdd);
		hb.GetComponent<Canvas>().enabled = false;
		FreezeCube.enabled = false;
		tag = "Untagged";
		GetComponent<NavMeshAgent>().enabled = false;
		Smoker.Play();
		foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
		{
			m.enabled = false;
		}
		Instantiate(Exploder, transform);
		yield return new WaitForSeconds(5);
		Destroy(gameObject);
	}
}
