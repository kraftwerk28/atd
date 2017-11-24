using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IWeapon
{

	public float range;
	public float damage = 2;
	public float damage_multiplier = 1.5f;
	public static int price = 100;
	public float rotation_speed;

	public Transform platform;
	public Transform muzzle;
	public Transform crystal;
	public Transform RangeSprite;
	public Transform DmgPos;

	private Transform Target = null;
	private Quaternion lookat;

	private GameObject[] enemies;

	//additonal
	private LineRenderer laser;

	public string Name { get; set; }
	public int Level { get; set; }
	public bool isActive { set { GetComponent<Laser>().enabled = value; } }

	// Use this for initialization
	void Start()
	{
		Name = name;

		laser = crystal.GetComponent<LineRenderer>();
		InvokeRepeating("CheckEnemy", 1f, 0.1f);
		RangeSprite.localScale = Vector3.one * range / 15;
	}

	// Update is called once per frame
	void Update()
	{
		if (Target != null)
		{
			lookat = Quaternion.LookRotation(Target.position - crystal.position + Vector3.up);
			Vector3 pl_dir = Quaternion.Lerp(platform.rotation, lookat, Time.deltaTime * rotation_speed).eulerAngles;
			Vector3 c_dir = Quaternion.Lerp(muzzle.rotation, lookat, Time.deltaTime * rotation_speed).eulerAngles;
			muzzle.rotation = Quaternion.Euler(c_dir.x, c_dir.y, 0f);
			platform.rotation = Quaternion.Euler(0, muzzle.eulerAngles.y, 0);
		}
	}

	void CheckEnemy()
	{
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		Transform NearestEnemy = null;
		float dist = Mathf.Infinity;
		foreach (GameObject en in enemies)
		{
			if (Vector3.Magnitude(en.transform.position - transform.position) < dist)
			{
				dist = Vector3.Distance(en.transform.position, transform.position);
				NearestEnemy = en.transform;
			}
		}

		if (NearestEnemy != null && Vector3.Magnitude(NearestEnemy.position - transform.position) <= range)
			Target = NearestEnemy;
		else
			Target = null;
		RaycastHit rh;

		if (Physics.Raycast(crystal.position, crystal.forward, out rh, range) && rh.collider.gameObject.tag == "Enemy")
		{
			rh.collider.gameObject.SendMessage("GiveDamage", damage);
			laser.SetPosition(0, crystal.position);
			laser.SetPosition(1, rh.point);
			laser.enabled = true;
			crystal.GetComponent<ParticleSystem>().Play();
			DmgPos.GetComponent<ParticleSystem>().Play();
			DmgPos.position = rh.point;
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
		}
		else
		{
			crystal.GetComponent<ParticleSystem>().Stop();
			DmgPos.GetComponent<ParticleSystem>().Stop();
			GetComponent<AudioSource>().Stop();
			laser.enabled = false;
		}
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
			RangeSprite.gameObject.GetComponent<Animator>().enabled = true;
			RangeSprite.gameObject.GetComponent<SpriteRenderer>().enabled = true;
		}

		else
		{
			RangeSprite.gameObject.GetComponent<Animator>().enabled = false;
			RangeSprite.gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
