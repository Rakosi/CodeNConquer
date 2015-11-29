using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	//Health system is used by all soldiers and buildings in the game (controlled or not)
	//It consists of an update system that is only called when under attacked

	//Also it includes sprite for displayement of health. Updates here
	//Attach this as component to health prefab to be included by all soldiers and buildings

	SpriteRenderer healthBarFrame;
	SpriteRenderer healthBar;

	//Defined by soldier and building upon spawn
	//Also destruction will be called from here too
	float health;

	public float HealthProp {
		get {
			return health;
		}
		set {
			health = value;
		}
	}

	float maxHealth;

	public float MaxHealth {
		get {
			return maxHealth;
		}
		set {
			maxHealth = value;
		}
	}

	float armor; //Soldiers and buildings

	public float Armor {
		get {
			return armor;
		}
		set {
			armor = value;
		}
	}

	bool underAttack;
	public bool UnderAttack {
		get {
			return underAttack;
		}
		set {
			underAttack = value;
		}
	}

	float barScale;

	//Defines the healing frequency

	void Awake () {
		healthBarFrame = transform.GetChild(0).GetComponent<SpriteRenderer>();
		healthBar = transform.GetChild(1).GetComponent<SpriteRenderer>();

		//This will be changed by soldier/building
		health = 100;
		maxHealth = 100;
		armor = 0;

		barScale = healthBar.transform.localScale.x;
	}

	//Constantly check the timer for auto healing
	void FixedUpdate () {
		transform.position = (Vector2)transform.parent.GetComponent<Transform>().position + new Vector2 (0, 10.0f);
		//Apply the inverse of the soldier,building rotation in order to place the bar properly
		transform.localRotation = Quaternion.Inverse(transform.parent.GetComponent<Transform>().localRotation);

	}

	//Update the health bar so it is syncronized
	public void updateHealthBar ()
	{	
		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3( barScale * (health/(maxHealth)), 
													healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}

	//Applies the damage to unit or building, called by attacker. If health goes down
	//equal or below 0, returns true. And the unit starts death procedure.
	public bool getDamaged(float dmg)
	{

		underAttack = true;
		if (dmg - armor > 0) { //Don't change health if armor absorbs all
			health -= dmg- armor;
			if(health <= 0)
			{
				maxHealth = 1; //For safety, no ressurection
				health = 0;
				updateHealthBar();
				return true;
			}
			updateHealthBar();
			return false;	
		}
		return false;

	}

	void OnDestroy() {
		//Death sound

	}


}
