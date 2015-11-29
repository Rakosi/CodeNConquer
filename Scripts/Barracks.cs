using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Barracks :  Building {

	Queue soldiers;

	public Queue Soldiers {
		get {
			return soldiers;
		}
	}

	//Time required for each soldier type
	int timer;
	int infantryTime;

	public int InfantryTime {
		get {
			return infantryTime;
		}
	}

	int cavalryTime;

	public int CavalryTime {
		get {
			return cavalryTime;
		}
	}

	Health health;
	SpriteRenderer render;
	public Infantry infantry;
	public Cavalry cavalry;
	Soldier soldier;
	Grid grid;
	SelectionManager selection;

	//Unit specs
	int cavalryDmg,cavalryArmor,cavalryHealth;
	int infantryDmg,infantryArmor,infantryHealth;

	//Used for determining if a upgrade can be bought
	bool armorAval;

	public bool ArmorAval {
		get {
			return armorAval;
		}
	}

	bool attackAval;

	public bool AttackAval {
		get {
			return attackAval;
		}
	}

	bool upgradeAval;

	public bool UpgradeAval {
		get {
			return upgradeAval;
		}
	}

	int armorLevel,attackLevel;

	// Use this for initialization
	void Start () {
		//This will change as tier increases later
		tier = 1;
		infantryTime = 100;
		cavalryTime = 200;

		//Initialize the soldier queue
		soldiers = new Queue ();
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		render = GetComponent<SpriteRenderer>();
		selection = grid.GetComponent<SelectionManager> ();

		health = GetComponentInChildren<Health> ();

		cavalryDmg = 15; cavalryArmor = 5; cavalryHealth = 125;
		infantryDmg = 10; infantryArmor = 0; infantryHealth = 100;

		armorAval = false;attackAval = false;upgradeAval = false;
		armorLevel = 1;attackLevel = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		//If there are soldiers to be summoned in the queue
		if(soldiers.Count != 0)
		{
			//Create the soldier
			if(timer == 0)
			{

				//Get the barracks position as x and y positions of building
				Vector2 spawnLoc = new Vector2(this.X,this.Y); 

				for(int r = (int)spawnLoc.x - 2;r <= (int)spawnLoc.x + 2;r++){
					for(int c = (int)spawnLoc.y - 2;c <= (int)spawnLoc.y + 2;c++){
						//We will check the immediate surrounding of the barracks for summoning the new soldier

						//The place is not occupied
						if(!grid.checkSingleTile(r,c))
						{
							spawnLoc.x = r;
							spawnLoc.y = c;
							//Create the soldier at the closest not occupied location
							spawnLoc = new Vector2 ((r * grid.getTileRadius () * 2 + grid.getTileRadius ()),
							                       (c * grid.getTileRadius () * 2 + grid.getTileRadius ()));
							//Create cavalry
							if ((bool)soldiers.Dequeue ()) {
								soldier = ((Cavalry)Instantiate (cavalry, spawnLoc, transform.rotation));
								((Cavalry)soldier).stats(cavalryDmg,cavalryArmor,cavalryHealth);
								r = (int)spawnLoc.x + 3;
								break;

							}
							//Create soldier
							else{
								soldier = ((Infantry)Instantiate (infantry, spawnLoc, transform.rotation));
								((Infantry)soldier).stats(infantryDmg,infantryArmor,infantryHealth);
								r = (int)spawnLoc.x + 3;
								break;
							}
						}
							
					}
				}

				//If there are still soldiers to be created, reset the timer accordingly
				if (soldiers.Count != 0)
					timer = ((bool)soldiers.Peek()) ? cavalryTime : infantryTime;
			}
			else{
				timer--;
			}

		}

		//Used for ui buttons
		if (tier != 1 && tier > armorLevel)
			armorAval = true;
		if (tier != 1 && tier > attackLevel)
			attackAval = true;
		if (tier < grid.GetComponent<GameEngine>().BaseTier)
			upgradeAval = true;
		//If selected, update the queue number and timer display
		if (render.color == Color.yellow) {
			selection.updatePanelDynamic(0,5,soldiers.Count);
			selection.updatePanelDynamic(0,1,timer);
		}
	}


	//This function works on the creation of soldiers depending on the current points of the
	//user that is obtained from questions. It starts the creation of soldier by adding it to 
	//queue while holding their number.
	//A timer is used to show how much time left for soldier to be created.
	public void createInfantry()
	{
		//Add the soldier type to the queue to be summoned when timer finishes
		//Reset the timer for update to summon the soldier
		if (soldiers.Count == 0)
			timer = infantryTime;
		soldiers.Enqueue(false);
	}

	public void createCavalry()
	{
		//Add the soldier type to the queue to be summoned when timer finishes
		//Reset the timer for update to summon the soldier
		if (soldiers.Count == 0)
			timer = cavalryTime;
		soldiers.Enqueue(true);
	}

	//Increases the tier of the building which 
	//	-Decreases the time required for creation for each soldier type
	//	-Increases the health of the building (Restores it)
	//	-Changes the sprite of the building for indication of level
	#region implemented abstract members of Building
	public override void upgradeBuilding ()
	{
		if(tier != grid.GetComponent<GameEngine>().MaxTier && tier+1 == grid.GetComponent<GameEngine>().BaseTier){
			//Increse tier
			tier++;
		
			//Decrease time required for each soldier type
			cavalryTime -= 30;
			infantryTime -= 25;

			//Only increase health of units
			cavalryHealth += 50;
			infantryHealth += 25;

			//Repair the building and increase maximum health
			health.MaxHealth += 25;
			health.HealthProp = health.MaxHealth;
		
			//According to tier, change the sprite of barracks
			Sprite sprite = Resources.Load ("barracks" + tier,typeof(Sprite)) as Sprite;

			render.sprite = sprite;
			upgradeAval = false;
		}

	}
	#endregion

	//Increase the armor of units
	public void upgradeArmor()
	{
		cavalryArmor += 5; 
		infantryArmor += 5; 
		armorLevel++;
		armorAval = false;
	}

	//Increase the attack of units
	public void upgradeAttack()
	{
		infantryDmg += 10; 
		cavalryDmg += 10; 
		attackLevel++;
		attackAval = false;
	}


	//Clicked on this barracks, send the info about getting clicked to selection manager
	void OnMouseDown()
	{
		render.color = Color.yellow;
		selection.setSelected (this);
	}

	public override void Deselect()
	{
		render.color = Color.white;
	}
}
