using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	public Transform spawner;
	public GameObject[] enemy;

	private float value;
	public float period = 2f;
	
	void Start()
	{
		InvokeRepeating("Spawn", 0f, period);
	}

	void Spawn()
	{
		StartCoroutine(Sp());
	}

	IEnumerator Sp()
	{
		TrigDoor(true);
		Instantiate(enemy[Random.Range(0, enemy.Length)], spawner);
		yield return new WaitForSeconds(1);
		TrigDoor(false);
	}

	void TrigDoor(bool open)
	{
		if (open)
			GetComponent<Animator>().SetTrigger("open");
		else
			GetComponent<Animator>().SetTrigger("close");
	}
}
