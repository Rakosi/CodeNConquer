using UnityEngine;
using System.Collections;

public class Barracks :  Building {

	Queue soldiers;

	//Time required for each soldier type
	int timer;
	int infantryTime;
	int cavalryTime;
//	Health health;
	SpriteRenderer render;
	public Infantry infantry;
	public Cavalry cavalry;
	Soldier soldier;
	Grid grid;

	// Use this for initialization
	void Start () {
		//This will change as tier increases later
		tier = 1;
		infantryTime = 100;
		cavalryTime = 200;

		//Initialize the soldier queue
		soldiers = new Queue ();
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
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
								r = (int)spawnLoc.x + 3;
								break;

							}
							//Create soldier
							else{
								soldier = ((Infantry)Instantiate (infantry, spawnLoc, transform.rotation));
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
	}

	//This function works on the creation of soldiers depending on the current points of the
	//user that is obtained from questions. It starts the creation of soldier by adding it to 
	//queue while holding their number.

	//A timer is used to show how much time left for soldier to be created.
	public void createSoldier(bool isCavalary)
	{
		//Add the soldier type to the queue to be summoned when timer finishes
		//Reset the timer for update to summon the soldier
		if (soldiers.Count == 0)
			timer = (isCavalary) ? cavalryTime : infantryTime;
		soldiers.Enqueue(isCavalary);
	}

	//Increases the tier of the building which 
	//	-Decreases the time required for creation for each soldier type
	//	-Increases the health of the building (Restores it)
	//	-Changes the sprite of the building for indication of level
	#region implemented abstract members of Building
	protected override void upgradeBuilding ()
	{
		//Increse tier
		tier++;
		
		//Decrease time required for each soldier type
		cavalryTime -= 30;
		infantryTime -= 25;
		
		//health = GetComponent<Health>();
		//TODO:UPDATE THE HEALTH HERE
		
		//According to tier, change the sprite of barracks
		Sprite sprite = Resources.Load ("barracks" + tier,typeof(Sprite)) as Sprite;
		
		//Put the transparent template by changing the material
		render = GetComponent<SpriteRenderer>();
		render.sprite = sprite;
	}
	#endregion


}
