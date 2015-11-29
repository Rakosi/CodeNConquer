using UnityEngine;
using System.Collections;

public class Infantry : Soldier {


	Health healthSys;
	float attack;

	void Start()
	{
		range = 27f;
	}

	public void stats (float attack, float armor,float maxHealth)
	{
		healthSys = GetComponentInChildren<Health> ();
		healthSys.MaxHealth = maxHealth;
		healthSys.HealthProp = maxHealth;
		healthSys.Armor = armor;
		healthSys.updateHealthBar ();
		this.attack = attack;

	}

	protected override bool Attack(GameObject target)
	{
		Debug.Log("ttac");
		if (target.GetComponentInChildren<Health> ().getDamaged (attack)) {
			Destroy(target);
			return true;
		}
		return false;

	}
	
}
