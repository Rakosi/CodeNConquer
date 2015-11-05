using UnityEngine;
using System.Collections;

public class Archery : Building {

	Queue soldiers;
	
	//Time required for each soldier type
	int timer;
	int archerTime;
	//	Health health;
	SpriteRenderer render;
	public Archer archer;
	Soldier soldier;
	Grid grid;
	SelectionManager selection;
	
	// Use this for initialization
	void Start () {
		//This will change as tier increases later
		tier = 1;
		archerTime = 120;
		
		//Initialize the soldier queue
		soldiers = new Queue ();
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		render = GetComponent<SpriteRenderer>();
		selection = grid.GetComponent<SelectionManager> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		//If there are soldiers to be summoned in the queue
		if(soldiers.Count != 0)
		{
			//Create the soldier
			if(timer == 0)
			{
				
				//Get the archer position as x and y positions of building
				Vector2 spawnLoc = new Vector2(this.X,this.Y); 
				
				for(int r = (int)spawnLoc.x - 2;r <= (int)spawnLoc.x + 2;r++){
					for(int c = (int)spawnLoc.y - 2;c <= (int)spawnLoc.y + 2;c++){
						//We will check the immediate surrounding of the barracks for summoning the new soldier
						
						//The place is not occupied
						if(!grid.checkSingleTile(r,c))
						{
							spawnLoc.x = r;
							spawnLoc.y = c;
							soldiers.Dequeue ();
							//Create the soldier at the closest not occupied location
							spawnLoc = new Vector2 ((r * grid.getTileRadius () * 2 + grid.getTileRadius ()),
							                        (c * grid.getTileRadius () * 2 + grid.getTileRadius ()));
							//Create archer
							soldier = ((Archer)Instantiate (archer, spawnLoc, transform.rotation));
							r = (int)spawnLoc.x + 3;
							break;
	
						}
						
					}
				}
				
				//If there are still soldiers to be created, reset the timer accordingly
				if (soldiers.Count != 0)
					timer = archerTime;
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
	public void createSoldier()
	{
		//Add the soldier type to the queue to be summoned when timer finishes
		//Reset the timer for update to summon the soldier
		if (soldiers.Count == 0)
			timer = archerTime;
		soldiers.Enqueue(true);
	}
	
	//Increases the tier of the building which 
	//	-Decreases the time required for creation for each soldier type
	//	-Increases the health of the building (Restores it)
	//	-Changes the sprite of the building for indication of level
	#region implemented abstract members of Building
	public override void upgradeBuilding ()
	{
		//Increse tier
		tier++;
		
		//Decrease time required for archers
		archerTime -= 30;
		
		//health = GetComponent<Health>();
		//TODO:UPDATE THE HEALTH HERE
		
		//According to tier, change the sprite of barracks
		Sprite sprite = Resources.Load ("archery" + tier,typeof(Sprite)) as Sprite;


		render.sprite = sprite;
	}
	#endregion

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
