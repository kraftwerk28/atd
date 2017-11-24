using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : MonoBehaviour, IWeapon
{

	public float range;
	public float damage = 1;
	public static int price = 10;
	public float rotation_speed;

	public Transform platform;
	public Transform muzzle;
	public Transform shooter;
	public GameObject pipe;
	public ParticleSystem particle_s;
	public new Light light;
	public Transform RangeSprite;

	private Transform Target = null;
	private Quaternion lookat;

	private GameObject[] enemies;
	
	public string Name { get; set; }
	public int Level { get; set; }
	public bool isActive { set { GetComponent<Minigun>().enabled = value; } }

	// Use this for initialization
	void Start()
	{
		Name = name;
		Debug.Log(Name);

		InvokeRepeating("CheckEnemy", 1f, 0.1f);
		RangeSprite.localScale = Vector3.one * range / 15;
	}

	// Update is called once per frame
	void Update()
	{
		if (Target != null)
		{
			ToggleShoot(true);
			lookat = Quaternion.LookRotation(Target.position - shooter.transform.position + Vector3.up);
			Vector3 pl_dir = Quaternion.Lerp(platform.rotation, lookat, Time.deltaTime * rotation_speed).eulerAngles;
			Vector3 c_dir = Quaternion.Lerp(muzzle.rotation, lookat, Time.deltaTime * rotation_speed).eulerAngles;
			muzzle.rotation = Quaternion.Euler(c_dir.x, c_dir.y, 0f);
			platform.rotation = Quaternion.Euler(0, muzzle.eulerAngles.y, 0);
		}
		else
			ToggleShoot(false);
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
		
		if (Physics.Raycast(shooter.position, shooter.forward, out rh, range) && rh.collider.gameObject.tag == "Enemy")
		{
			rh.collider.gameObject.GetComponent<IEnemy>().GiveDamage(damage);
			//rh.collider.gameObject.GetComponent<Rigidbody>().AddForce((shooter.forward - shooter.position), ForceMode.Impulse);
		}
	}

	void ToggleShoot(bool s)
	{
		if (s)
		{
			pipe.GetComponent<Animator>().SetBool("Rotate", true);
			if (!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
			particle_s.Play();
			light.enabled = true;
		}
		else
		{
			pipe.GetComponent<Animator>().SetBool("Rotate", false);
			GetComponent<AudioSource>().Stop();
			particle_s.Stop();
			light.enabled = false;
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
		Level++;
	}
}
