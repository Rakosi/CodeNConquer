using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Soldier : MonoBehaviour {
	
	protected int tier;
	public Vector2 location;
	public int Tier {
		get {
			return tier;
		}
		set {
			if(tier < value && tier != 3)
				tier = value;
		}
	}


	protected SelectionManager Selection;
	private Grid grid;
	private Camera cam;
	private SpriteRenderer render;
	Vector2 point;
	Rigidbody2D body;
	int ordered; //-1 = no order, 0 = loc,1 = enemy
	GameObject target; //Currently no target
	Vector2 targetSighted; //Used for checking if target has moved to another location
	Vector2 targetLoc;
	Stack pathOrdered;



	//Class dependent properties
	protected float range;
	float awareness; //The range where soldier checks for enemies automatically

	// Initialize the components of the infantry
	void Awake () {
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		render = GetComponent<SpriteRenderer>();
		Selection = grid.GetComponent<SelectionManager> ();
		cam = grid.transform.Find("Camera").GetComponent<Camera>();
		body = GetComponent<Rigidbody2D>();

		point = transform.position;
		ordered = -1; //No order has ben given
		pathOrdered = new Stack ();
		target = null;
		targetLoc = new Vector2 (0, 0);
		targetSighted = new Vector2 (0, 0);
		counter = 100;
		awareness = 100;
	}

	public void GetMultiSelected()
	{
		//Selection is being made and unit is not selected
		//As multi soldier movement doesn't work perfectly with high number of soldiers, we kept limit of maximum soldiers selected to 4
		if (Selection.startLoc != -Vector3.one && render.color != Color.yellow && Selection.SelectedS.Count < 4) {
			
			//Convert the world coord to camera without dealing with width,height
			Vector3 pos = cam.WorldToScreenPoint(transform.position);
			pos.y = (Screen.height - pos.y);
			if (Selection.selectionRect.Contains(pos,true)){
				render.color=Color.yellow;
				Selection.addSelected(this);
			}
		}
	}
	

	public void Deselect()
	{
		render.color = Color.white;
	}

	//Clicked on this soldier, send the info about getting clicked to selection manager
	void OnMouseDown()
	{
		Selection.cancel (true);
		render.color = Color.yellow;
		Selection.addSelected(this);
	}
	
	int counter;

	void CheckSurroundings ()
	{
		//Get the objects in the area
		Collider2D[] objects =  Physics2D.OverlapAreaAll (new Vector2(point.x - awareness,point.y +  awareness),
		                                                  new Vector2(point.x + awareness,point.y -  awareness),
		                                                  1 << LayerMask.NameToLayer("obstacle"));
		//If there are objects
		if (objects.Length != 0) {

			//Find the closest enemy
			GameObject closest = null;
			foreach (Collider2D c in objects )
			{
				//No enemy found until now, assign new closest if enemy
				if(closest == null)
				{
					if(c.gameObject.tag == "Enemy")
						closest = c.gameObject;
				}
				//This object is closer that closest and its enemy
				else if(((closest.transform.position - transform.position).magnitude > 
				         (c.transform.position - transform.position).magnitude)
				         && c.gameObject.tag == "Enemy")
				{
					closest = c.gameObject;
				}

			}
			//Assign it if there is currently no order
			if(ordered == -1 && closest != null)
			{
				Debug.Log ("HERE HE IS");
				AssignEnemy(closest);
			}
		}

	}

	//Assign the given gameobject as target to the soldier
	void AssignEnemy(GameObject target)
	{
		this.target = target;
		ordered = 1;
		//As if the target locationis put as enemy loc, code will assume no path exists, so 
		//need to get the nearest avaliable location (neighbours in range)
		targetSighted = new Vector2(target.transform.position.x,target.transform.position.y);
		Tile newTarget = grid.ConvertToTile (target.transform.position);
		ArrayList alternatives = grid.getNeighbours (newTarget.X, newTarget.Y);
		
		if (alternatives.Count != 0)
		{
			do{
				targetLoc = ((Tile)alternatives [Random.Range(0,alternatives.Count-1)]).WorldPosition;
				newTarget = grid.ConvertToTile (targetLoc);
			}
			while(grid.checkSingleTile(newTarget.X, newTarget.Y));
		}
		pathOrdered = CalculatePath(targetLoc);
		counter = 100;
	}

	// Update is called once per frame
	void FixedUpdate () {
		
		GetMultiSelected ();

		//Only check surroundings for enemy and when idle
		if(this.tag != "Enemy" && ordered == -1)
			CheckSurroundings ();

		if (render.color == Color.yellow) {
			//Get right clicked position
			if (Input.GetMouseButton (1)) {
				point = Input.mousePosition;
				point = cam.ScreenToWorldPoint (point);

				//Get the objects in the area if there is any enemy presence

				Collider2D[] objects =  Physics2D.OverlapAreaAll (new Vector2(point.x - 0.5f,point.y +  0.5f),
				                                                  new Vector2(point.x + 0.5f,point.y -  0.5f),
				                                                  1 << LayerMask.NameToLayer("obstacle"));

				//No one at clicked location so apply normal movement order
				if(objects.Length == 0)
				{
					ordered = 0;
					pathOrdered = CalculatePath(point);
				}
				else{
					//If the clicked location is enemy, issue an attack order
					if(objects[0].tag == "Enemy")
					{
						AssignEnemy(objects [0].gameObject);
					}
					//Else , the location is ally so don't do anything
				}



			}
		}


		//Move order, location is clear with no enemy
		if (ordered == 0) {
			if (pathOrdered.Count != 0) {
				//Check if path has new occupations (a unit moving or building created)
				Stack checkPath = new Stack ();
				//Dont consider the first position as obstacle
				checkPath.Push (pathOrdered.Pop ());
				bool changed = false;
				while (pathOrdered.Count > 0) {

					checkPath.Push (pathOrdered.Pop ());
					//If any point on path is occupied, recalc
					if (grid.checkSingleTile (((Tile)checkPath.Peek ()).X, ((Tile)checkPath.Peek ()).Y)) {

						//If the target is occupied by someone else, go to the nearest unoccupied neighbour
						if (grid.checkSingleTile(grid.ConvertToTile(point).X,grid.ConvertToTile(point).Y)) {
							Tile newTarget = grid.ConvertToTile (point);
							ArrayList alternatives = grid.getNeighbours (newTarget.X, newTarget.Y);

							if (alternatives.Count != 0)
							{
								do{
									point = ((Tile)alternatives [Random.Range(0,alternatives.Count-1)]).WorldPosition;
									newTarget = grid.ConvertToTile (point);
								}
								while(grid.checkSingleTile(newTarget.X, newTarget.Y));
							}
						

						}

						pathOrdered = CalculatePath (point);
						changed = true;
						break;
					}

				}
				while (checkPath.Count != 0 && !changed) {
					pathOrdered.Push (checkPath.Pop ());
				}

				try {
					followPath();
				} catch (System.InvalidOperationException) {
				}
			}

			//The target has been arrived, order is completed
			if (pathOrdered.Count == 0)
				ordered = -1;


		}
		//Soldier is assigned to an enemy, target acquired
		else if (ordered == 1) {

			//If enemy has changed loc, refresh location
			//Use counter to check new location of target
			if((Vector2)target.transform.position != targetSighted && counter == 0)
			{
				targetSighted = new Vector2(target.transform.position.x,target.transform.position.y);
				Tile newTarget = grid.ConvertToTile (target.transform.position);
				ArrayList alternatives = grid.getNeighbours (newTarget.X, newTarget.Y);
				
				if (alternatives.Count != 0)
				{
					do{
						targetLoc = ((Tile)alternatives [Random.Range(0,alternatives.Count-1)]).WorldPosition;
						newTarget = grid.ConvertToTile (targetLoc);
					}
					while(grid.checkSingleTile(newTarget.X, newTarget.Y));
				}
				pathOrdered = CalculatePath(targetLoc);
				counter = 50;

			}
			
			//Still not enough near the enemy, move further
			if((transform.position - target.transform.position).magnitude > range)
			{
				if (pathOrdered.Count != 0) {
				try {
					followPath();
				} catch (System.InvalidOperationException) {
					
				}
				}
			}
			//The enemy is in range, attack !
			else 
			{
				//Debug.LogError("HEHEUHEUHE");
				ordered = -1;
				Attack(target);
			}


		}
		counter--;
		if (counter < 0)
			counter = 100;


	}

	//If collide with anything, recalculate path
	void OnTriggerEnter2D (Collider2D collider)
	{
		pathOrdered = CalculatePath(point);
	}

	//Follows through path given
	void followPath()
	{
		Tile tile = (Tile)pathOrdered.Peek ();
		
		Vector2 movement = tile.WorldPosition - (Vector2)transform.position;
		//If the soldier is very close to target, don't normalize as it will cause swinging around target point
		if (movement.magnitude > 1)
			movement.Normalize ();
		//Find the angle for rotation
		float rot_z = Mathf.Atan2 (movement.y, movement.x) * Mathf.Rad2Deg;
		//Rotate using the angle of rotation
		transform.rotation = Quaternion.Euler (0f, 0f, rot_z - 90);
		
		//If the next tile is arrived, pop it. Else continiue going to it
		if ((Vector2)transform.position + movement != tile.WorldPosition)
			body.MovePosition ((Vector2)transform.position + movement);
		else
			pathOrdered.Pop ();
	}

	//The pathfinding part of soldiers
	//Using A* algorithm, soldiers needs to find the shortest path avaliable and follow it
	//grid by grid. (Depending on tile's location, a force will be applied)
	//If the soldier collides with anything, it stops and recalculates the A* shortest path
	//and moves on
	//If enemy changes position , the path is calculated again

	//Thanks to SebastianLague - http://pastebin.com/rj9jZnBN
	Stack CalculatePath(Vector2 point)
	{
		//Get the tiles of start and target
		Tile startTile = grid.ConvertToTile (transform.position);
		Tile targetTile = grid.ConvertToTile (point);
		startTile.GCost = 0;
		startTile.HCost = 0;
		List<Tile> openTiles = new List<Tile>();
		List<Tile> closedTiles = new List<Tile>();

		//Add the start tile to be managed by puttin to open tiles list
		openTiles.Add (startTile);

		//While there are tiles to consider
		while (openTiles.Count > 0) {

			Tile currentTile = openTiles[0];
			//Find the tile in open nodes with lowest total cost
			foreach(Tile t in openTiles)
			{
				if(t.FCost < currentTile.FCost || (t.FCost == currentTile.FCost && t.HCost < currentTile.HCost))
				   currentTile = t;

			}

			//Put that tile as considered 
			openTiles.Remove(currentTile);
			closedTiles.Add(currentTile);

			//If we arrived at destination
			if(currentTile == targetTile)
			{
				//Calculate and return path
				return TraversePath(startTile,targetTile,closedTiles);

			}

			//Get neighbours
			ArrayList neighbours = grid.getNeighbours(currentTile.X,currentTile.Y);

			foreach(Tile n in neighbours)
			{
				//The neighbour is not occupied and not considered
				if(!(grid.checkSingleTile(n.X,n.Y) || closedTiles.Contains(n)))
				{
					//Add neighbour to be considered list and update its neighbours
					int neighbourCost = currentTile.GCost + grid.tileDistance(currentTile, n);
					if(neighbourCost < n.FCost || !openTiles.Contains(n));
					{
						n.GCost = neighbourCost;
						n.HCost = grid.tileDistance(n, targetTile);

						if(!openTiles.Contains(n))
							openTiles.Add(n);
					}

				}

			}



		}
		//return empty if no path can be found
		return new Stack ();
	}


	Stack TraversePath (Tile startTile,Tile targetTile,List<Tile> closedTiles)
	{
		Tile current = targetTile;
		Stack path = new Stack ();
		//Put the target tile
		path.Push (current);
		closedTiles.Remove (current);
		while (current != startTile) {

			//Find the neighbour in closed tiles with smallest distance to target tile
			ArrayList neighbours = grid.getNeighbours(current.X,current.Y);
			foreach(Tile t in neighbours)
			{
				if(closedTiles.Contains(t) && 
				   (t.FCost < current.FCost || (t.FCost == current.FCost && t.GCost < current.GCost)))
				{
					current = t;
					path.Push (current);
					closedTiles.Remove (current);
				}
				
			}
		}

		return path;
	}


	protected abstract void Attack(GameObject target);

	/*

	using UnityEngine;
	using System.Collections;
	
	public class PlayerHealth : MonoBehaviour
	{	
		public float health = 100f;					// The player's health.
		public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
		public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
		public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
		public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player
		
		private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
		private float lastHitTime;					// The time at which the player was last hit.
		private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
		private PlayerControl playerControl;		// Reference to the PlayerControl script.
		private Animator anim;						// Reference to the Animator on the player
		
		
		void Awake ()
		{
			// Setting up references.
			playerControl = GetComponent<PlayerControl>();
			healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
			anim = GetComponent<Animator>();
			
			// Getting the intial scale of the healthbar (whilst the player has full health).
			healthScale = healthBar.transform.localScale;
		}
		
		
		void OnCollisionEnter2D (Collision2D col)
		{
			// If the colliding gameobject is an Enemy...
			if(col.gameObject.tag == "Enemy")
			{
				// ... and if the time exceeds the time of the last hit plus the time between hits...
				if (Time.time > lastHitTime + repeatDamagePeriod) 
				{
					// ... and if the player still has health...
					if(health > 0f)
					{
						// ... take damage and reset the lastHitTime.
						TakeDamage(col.transform); 
						lastHitTime = Time.time; 
					}
					// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
					else
					{
						// Find all of the colliders on the gameobject and set them all to be triggers.
						Collider2D[] cols = GetComponents<Collider2D>();
						foreach(Collider2D c in cols)
						{
							c.isTrigger = true;
						}
						
						// Move all sprite parts of the player to the front
						SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
						foreach(SpriteRenderer s in spr)
						{
							s.sortingLayerName = "UI";
						}
						
						// ... disable user Player Control script
						GetComponent<PlayerControl>().enabled = false;
						
						// ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
						GetComponentInChildren<Gun>().enabled = false;
						
						// ... Trigger the 'Die' animation state
						anim.SetTrigger("Die");
					}
				}
			}
		}
		
		
		void TakeDamage (Transform enemy)
		{
			// Make sure the player can't jump.
			playerControl.jump = false;
			
			// Create a vector that's from the enemy to the player with an upwards boost.
			Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;
			
			// Add a force to the player in the direction of the vector and multiply by the hurtForce.
			GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
			
			// Reduce the player's health by 10.
			health -= damageAmount;
			
			// Update what the health bar looks like.
			UpdateHealthBar();
			
			// Play a random clip of the player getting hurt.
			int i = Random.Range (0, ouchClips.Length);
			AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
		}
		
		
		public void UpdateHealthBar ()
		{
			// Set the health bar's colour to proportion of the way between green and red based on the player's health.
			healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
			
			// Set the scale of the health bar to be proportional to the player's health.
			healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
		}
	}
*/
}
