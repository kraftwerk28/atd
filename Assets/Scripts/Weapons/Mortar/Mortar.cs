using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour, IWeapon
{

	public float range;
	public int damage = 20;
	public static int price = 200;
	public float rotation_speed;

	public Transform platform;
	public Transform muzzle;
	public Transform[] shooters;
	public GameObject rocket;
	public Transform RangeSprite;

	private Transform Target = null;
	private Quaternion lookat;

	private GameObject[] enemies;

	public string Name { get; set; }
	public int Level { get; set; }
	public bool isActive { set { GetComponent<Mortar>().enabled = value; } }

	// Use this for initialization
	void Start()
	{
		Name = name;

		InvokeRepeating("CheckEnemy", 1f, 3f);
		RangeSprite.localScale = Vector3.one * range / 15;
	}

	// Update is called once per frame
	void Update()
	{
		if (Target)
		{
			lookat = Quaternion.LookRotation(Target.position - muzzle.transform.position + Vector3.up);
			Vector3 pl_dir = Quaternion.Lerp(platform.rotation, lookat, Time.deltaTime * rotation_speed).eulerAngles;
			Vector3 c_dir = Quaternion.Lerp(muzzle.rotation, lookat, Time.deltaTime * rotation_speed).eulerAngles;
			muzzle.rotation = Quaternion.Euler(c_dir.x, c_dir.y, 0f);
			platform.rotation = Quaternion.Euler(0, muzzle.eulerAngles.y, 0);
		}
	}

	void CheckEnemy()
	{
		List<GameObject> _enemies = new List<GameObject>();

		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject en in enemies)
		{
			if (Vector3.Distance(en.transform.position, transform.position) <= range)
			{
				_enemies.Add(en);
			}
		}
		if (_enemies.Count != 0)
		{
			Target = _enemies[_enemies.Count / 2].transform;
			Invoke("Shoot", 1);
		}

	}

	void Shoot()
	{
		StartCoroutine(_Shoot());
	}

	IEnumerator _Shoot()
	{
		int si = Random.Range(0, shooters.Length);
		//particle_s.Play();
		//light.enabled = true;
		GetComponents<AudioSource>()[Random.Range(0, 2)].Play();
		GameObject bl = Instantiate(rocket, shooters[si].transform.position, shooters[si].rotation);
		bl.GetComponent<Rocket>().SelectTarget(Target);
		bl.GetComponent<Rocket>().damage = damage;

		yield return new WaitForSeconds(1f);

		//light.enabled = false;
		//particle_s.Stop();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
		RangeSprite.localScale = Vector3.one * range / 15;
	}

	public void ToggleSelect(bool show)
	{
		if (show)
		{
			RangeSprite.GetComponent<Animator>().enabled = true;
			RangeSprite.GetComponent<SpriteRenderer>().enabled = true;
		}

		else
		{
			RangeSprite.GetComponent<Animator>().enabled = false;
			RangeSprite.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	
	public void Sell()
	{
		Destroy(gameObject);
		GameObject.FindGameObjectWithTag("MainCamera").SendMessage("AddMoney", price * 0.5f);
	}
	public void Upgrade()
	{

	}
}
