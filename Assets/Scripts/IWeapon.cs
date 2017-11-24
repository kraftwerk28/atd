using UnityEngine;
using System.Collections;

public interface IWeapon
{
	bool isActive { set; }
	int Level { get; set; }
	string Name { get; set; }
	void Upgrade();
	void Sell();
	void ToggleSelect(bool show);
}