using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, IWeapon
{

	public float range;
	public float damage = 20;
	public static int price = 50;
	public float rotationspeed;

	public Transform platform;
	public Transform muzzle;
	public Transform shooter;
	public ParticleSystem particle_s;
	public new Light light;
	public Transform RangeSprite;

	public GameObject bullet;

	private Transform Target = null;
	private Quaternion lookat;

	private GameObject[] enemies;

	public string Name { get; set; }
	public int Level { get; set; }
	public bool isActive { set { GetComponent<Cannon>().enabled = value; } }

	// Use this for initialization
	void Start()
	{
		Name = name;
		Level = 1;
		InvokeRepeating("CheckEnemy", 1f, .5f);
		RangeSprite.localScale = Vector3.one * range / 15;
	}

	// Update is called once per frame
	void Update()
	{
		if (Target)
		{
			lookat = Quaternion.LookRotation(Target.position - shooter.transform.position + Vector3.up);
			Vector3 pl_dir = Quaternion.Lerp(platform.rotation, lookat, Time.deltaTime * rotationspeed).eulerAngles;
			Vector3 c_dir = Quaternion.Lerp(muzzle.rotation, lookat, Time.deltaTime * rotationspeed).eulerAngles;
			//platform.rotation = Quaternion.Euler(0f, pl_dir.y, 0f);
			muzzle.rotation = Quaternion.Euler(c_dir.x, c_dir.y, 0f);
			platform.rotation = Quaternion.Euler(0, muzzle.eulerAngles.y, 0);
		}
		else
			lookat = Quaternion.LookRotation(shooter.transform.forward);
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
			//GetComponent<AudioSource>().Play();
			GameObject bl = Instantiate(bullet, shooter.transform.position, shooter.rotation);
			bl.GetComponent<Rigidbody>().AddForce(shooter.forward * 5000, ForceMode.Impulse);
			StartCoroutine(ShootSmoke());
		}
	}

	IEnumerator ShootSmoke()
	{
		particle_s.Play();
		light.enabled = true;
		yield return new WaitForSeconds(0.1f);
		particle_s.Stop();
		light.enabled = false;
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

	void ScriptActivate()
	{
		GetComponent<Cannon>().enabled = true;
		RangeSprite.GetComponent<SpriteRenderer>().enabled = false;
		RangeSprite.GetComponent<Animator>().enabled = false;
	}

	public void Sell()
	{
		Destroy(gameObject);
		GameObject.FindGameObjectWithTag("MainCamera").SendMessage("AddMoney", price * 0.5f);
	}

	public void Upgrade()
	{

		if(Level<=2)
		{
			damage *= 1.5f;
			transform.localScale *= 1.5f;
		}
		Level = (Level + 1) % 3;
	}
}
