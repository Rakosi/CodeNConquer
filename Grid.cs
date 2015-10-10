using UnityEngine;
using System.Collections;

//This is the script for creation of Grid system of the game (Prone to changes)
//Written by Yalım Doğan


public class Grid : MonoBehaviour {

	public Transform player;
	public LayerMask obstacleMask;
	public Vector2 gridWorldSize;
	public float tileRadius;
	Tile[,] grid;

	float tileDiameter;
	int gridSizeX,gridSizeY;

	void Start()
	{
		tileRadius = .5f;
		gridWorldSize = new Vector2 (200, 100);
		tileDiameter = tileRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / tileDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / tileDiameter);
		InstantiateGrid ();
	}

	void InstantiateGrid()
	{
		grid = new Tile[gridSizeX, gridSizeY];
		Vector2 worldBottomLeft = new Vector2(transform.position.x - (float)Vector2.right.x * gridWorldSize.x / 2,
		                                      transform.position.y - (float)Vector2.up.y * gridWorldSize.y / 2);

		for (int x = 0;x < gridSizeX;x++){
			for (int y = 0;y < gridSizeY;y++){
				Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * tileDiameter + tileRadius) + Vector2.up * (y * tileDiameter + tileRadius);
				//Physics part is used to check for colliders in the already position, useful for placement of buildings and pathfinding
				//Actually unnecesseary here but can be useful in future
				bool isObstacle = (Physics.CheckSphere(worldPoint,tileRadius,obstacleMask));
				grid[x,y] = new Tile(isObstacle,worldPoint);
			}
		}

	}

	//Find the position of tile in the grid form the vector coordinates
	//Useful and be used for positioning for further game objects
	public Vector2 ConvertLocation(Vector2 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.y+ gridWorldSize.y / 2) / gridWorldSize.y;

		//Checks if the character in the world edges
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return new Vector2 (x, y);
	}


	//void PlaceBuilding()//Building toConstruct)
	void OnDrawGizmos()
	{
		//Gizmos are used for visual debugging which can be efficiently used for seeing the building 
		//transparently before its placement (green if avaliable, red if not)
		Gizmos.DrawWireCube (transform.position, new Vector2 (gridWorldSize.x, gridWorldSize.y));
		if (grid != null) {

			//Conversion of point (need to use that convert position part again)
			Vector2 buildingPos = ConvertLocation(player.position);
			//Checking all the tiles that are part of the building
			Tile [] buildingTile = new Tile[4]; //Depends on building sizes
			int x = Mathf.RoundToInt(buildingPos.x);
			int y = Mathf.RoundToInt(buildingPos.y);
			buildingTile[0] = grid[x,y];
			buildingTile[1] = grid[x+1,y-1];
			buildingTile[2] = grid[x+1,y];
			buildingTile[3] = grid[x,y-1];

			//Debug.Log(Mathf.RoundToInt(buildingPos.x) + " " + Mathf.RoundToInt(buildingPos.y));
			foreach(Tile t in buildingTile)
			{
				//Determine the tile status and color accordingly
				//Gizmos.color = (t.isObstacle) ? Color.white: Color.red;
//				if (buildingTile == t)
//				{
					Gizmos.color = Color.cyan;
					Gizmos.DrawCube(t.worldPosition,Vector2.one * (tileDiameter-.1f));
					//break;
//				}

			}
		}
	}

	//Constantly updates the whole grid each frame so building construction A* works as accurate as possible
	//As the game scene is constant, it is not a performance problem 
	void FixedUpdate()
	{
		foreach (Tile t in grid) {
			t.isObstacle = Physics.CheckSphere(t.worldPosition,tileRadius,obstacleMask);
		}
	}
}
