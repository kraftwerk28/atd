using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSpawn : MonoBehaviour
{

	//public
	[Header("Arrays")]
	public GameObject[] WeaponPrefabs;
	public Toggle[] toggles;
	public int[] prices;

	[Header("GUI")]
	public Text MoneyField;
	public ToggleGroup ToggleGroup;
	public EventSystem EvSystem;

	[Header("Sell Upgrade Group")]
	public GameObject Sellupgrade;
	public Text UpgrageText;
	public Text SellText;

	[Header("LayerMasks")]
	public LayerMask WeaponLayer;
	public LayerMask TerrainLayer;


	//private
	private GameObject ghost;
	private GameObject isSel;
	private Vector3 HideBufferPos = new Vector3(0, -29, 0);


	private int money;
	public int Money
	{
		set
		{
			money = value;
			MoneyField.GetComponent<TextMoney>().Change(money);
		}
		get
		{
			return money;
		}
	}

	private List<GameObject> AllWeapons = new List<GameObject>();
	private int WIndex;

	private RaycastHit rhit;

	void Start()
	{
		Money = 500;
		prices = new int[] { Minigun.price, Cannon.price, Freezer.price, Laser.price, Mortar.price };
	}

	void Update()
	{
		if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out rhit))
		{
			if (!EvSystem.IsPointerOverGameObject() && ghost && rhit.collider.gameObject.tag == "Terrain" && rhit.point.y > 0.9f && rhit.point.y < 1.1f)
			{
				ghost.transform.position = rhit.point;

				if (Input.GetMouseButtonDown(0))
				{
					SpawnWeapon(rhit.point);
				}
			}
			else if (ghost)
			{
				ghost.transform.position = HideBufferPos;
			}

			if (Input.GetMouseButtonDown(0))
			{
				if (rhit.collider.gameObject.tag == "Weapon")
				{
					if (isSel)
					{
						if (isSel == rhit.collider.gameObject)
						{
							isSel.GetComponent<IWeapon>().ToggleSelect(false);
							isSel = null;
							Sellupgrade.GetComponent<Animator>().SetBool("show", false);

							UpgrageText.text = "Upgrade";
						}
						else
						{
							isSel.GetComponent<IWeapon>().ToggleSelect(false);
							isSel = rhit.collider.gameObject;
							isSel.GetComponent<IWeapon>().ToggleSelect(true);
							Sellupgrade.GetComponent<Animator>().SetBool("show", true);

							UpgrageText.text = "Upgrade\n" + isSel.GetComponent<IWeapon>().Name.Split(new char[] { '(' })[0];
						}
					}
					else
					{
						isSel = rhit.collider.gameObject;
						isSel.GetComponent<IWeapon>().ToggleSelect(true);
						Sellupgrade.GetComponent<Animator>().SetBool("show", true);

						UpgrageText.text = "Upgrade\n" + isSel.GetComponent<IWeapon>().Name.Split(new char[] { '(' })[0];
					}
				}
				else if (!EvSystem.IsPointerOverGameObject() && isSel != null)
				{
					isSel.GetComponent<IWeapon>().ToggleSelect(false);
					isSel = null;
					Sellupgrade.GetComponent<Animator>().SetBool("show", false);

					UpgrageText.text = "Upgrade";
				}
			}
		}
	}


	void SpawnWeapon(Vector3 position)
	{
		if (money >= prices[WIndex])
		{
			GameObject w = Instantiate(WeaponPrefabs[WIndex], position, Quaternion.identity);

			Money -= prices[WIndex];
			AllWeapons.Add(WeaponPrefabs[WIndex]);
			foreach (Toggle t in ToggleGroup.ActiveToggles())
				t.isOn = false;
		}
	}

	public void ChangeWeapon()
	{
		int _index = -1;
		for (int i = 0; i < ToggleGroup.GetComponentsInChildren<Toggle>().Length; i++)
		{
			if (ToggleGroup.GetComponentsInChildren<Toggle>()[i].isOn)
			{
				_index = i;
				break;
			}
		}
		WIndex = _index;

		if (WIndex != -1)
		{
			Destroy(ghost);
			ghost = Instantiate(WeaponPrefabs[WIndex], HideBufferPos, Quaternion.identity);
			ghost.GetComponent<Collider>().enabled = false;
			ghost.GetComponent<IWeapon>().ToggleSelect(true);
			ghost.GetComponent<IWeapon>().isActive = false;
		}
		else
		{
			Destroy(ghost);
			ghost = null;
		}
	}

	void MinusMoney(int price)
	{
		money -= price;
	}

	public void AddMoney(int val)
	{
		Money += val;
	}

	public void UpgrageWeapon()
	{
		isSel.GetComponent<IWeapon>().Upgrade();
	}

	public void SellWeapon()
	{
		isSel.GetComponent<IWeapon>().Sell();
		Sellupgrade.GetComponent<Animator>().SetBool("show", false);
	}
}
