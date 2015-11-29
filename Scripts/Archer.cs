using UnityEngine;
using System.Collections;

public class Archer : Soldier {

	
	Health healthSys;
	public float attack;
	public Arrow arrow;
	
	void Start()
	{
		range = 100f;
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
		//Create arrow to kill the target
		//Arrow travels very fast so it won't miss the enemy
		Arrow arrowShot = ((Arrow)Instantiate(arrow, transform.position,transform.localRotation));
		arrowShot.setTarget (target.transform.position,"Enemy",attack); //True is to hit enemy, false to hit user units and buildings

		//The target is not destroyed by archer but arrow, so it is enough for archer to check
		//if target is dead in soldier script
		return false;
		
	}
	

}
