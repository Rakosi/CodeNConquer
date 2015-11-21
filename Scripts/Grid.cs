using UnityEngine;
using System.Collections;

//This is the script for creation of Grid system of the game (Prone to changes)
//Written by Yalım Doğan


public class Grid : MonoBehaviour {

	//public Transform player;
	private Vector2 gridWorldSize;

	public Vector2 GridWorldSize {
		get {
			return gridWorldSize;
		}
	}

	private float tileRadius;
	Tile[,] grid;

	private float tileDiameter;
	int gridSizeX,gridSizeY;

	void Awake()
	{
		tileRadius = 6f;
		gridWorldSize = new Vector2 (480, 270);
		tileDiameter = tileRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / tileDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / tileDiameter);
		InstantiateGrid ();
	}


	public float getTileRadius()
	{
		return tileRadius;
	}

	void InstantiateGrid()
	{
		grid = new Tile[gridSizeX, gridSizeY];
		Vector2 worldCorner = new Vector2 (0, 0);
		for (int x = 0;x < gridSizeX;x++){
			for (int y = 0;y < gridSizeY;y++){
				Vector2 worldPoint =  worldCorner + Vector2.right * (x * tileDiameter + tileRadius) + Vector2.up * (y * tileDiameter + tileRadius);
				//Physics part is used to check for colliders in the already position, useful for placement of buildings and pathfinding
				//RaycastHit obstacle;
				//Ray check = new Ray(worldPoint,Vector3.back);
//				if(Physics.Raycast (check,out obstacle,10,layer))
//				{
//					isObstacle = true;
//				}
//				else
//					isObstacle = false;
				//Check if there are any obstacles on the tile currently
				//Update: I have added a very small amount of distance to shrink the area to be checked, as
				//it can falsely think there is an obstacle on it while actually its neighbour has it.
				//Area leaks to neighbour tiles.
				bool isObstacle = Physics2D.OverlapArea (new Vector2(worldPoint.x - tileRadius + 0.01f,worldPoint.y + tileRadius - 0.01f),
				                                         new Vector2(worldPoint.x + tileRadius - 0.01f,worldPoint.y - tileRadius + 0.01f),
				                                    1 << LayerMask.NameToLayer("obstacle"));
				
				grid[x,y] = new Tile(isObstacle,worldPoint);
				if(isObstacle)
					Debug.Log(x + " " + y);
			}
		}

	}
	
	//Find the position of tile in the grid form the vector coordinates
	//Useful and be used for positioning for further game objects
	public Vector2 ConvertLocation(Vector2 worldPosition)
	{
		float percentX = (worldPosition.x + tileRadius) / gridWorldSize.x;
		float percentY = (worldPosition.y + tileRadius) / gridWorldSize.y;

		int x = Mathf.RoundToInt((gridSizeX) * percentX);
		int y = Mathf.RoundToInt((gridSizeY) * percentY);
		return new Vector2 (x-1, y-1);
	}

	//Constantly updates the whole grid each frame so building construction A* works as accurate as possible
	//As the game scene is constant, it is not a performance problem 
	void FixedUpdate()
	{
		foreach (Tile t in grid) {
		
			//Check if there are any obstacles on the tile currently
			//Update: I have added a very small amount of distance to shrink the area to be checked, as
			//it can falsely think there is an obstacle on it while actually its neighbour has it.
			//Area leaks to neighbour tiles.
			t.IsObstacle = Physics2D.OverlapArea (new Vector2(t.WorldPosition.x - tileRadius + 0.01f,t.WorldPosition.y + tileRadius - 0.01f),
			                                      new Vector2(t.WorldPosition.x + tileRadius - 0.01f,t.WorldPosition.y - tileRadius + 0.01f),
			                                      1 << LayerMask.NameToLayer("obstacle"));
		//	if(t.IsObstacle)
		//		Debug.Log(t.WorldPosition);
		}
	}

	//Checks all the close tiles if they are occupied, buildings take 5x5 even if they are 3x3
	//as we should leave room for soldier creation
	public bool CheckTile (Vector2 pos)
	{
		Vector2 pos2 = ConvertLocation(pos);
		int x = Mathf.RoundToInt (pos2.x);
		int y = Mathf.RoundToInt (pos2.y);
		
		//Debug.Log(x + " " + y);
		if (x < 2 || x > gridSizeX - 3 || y < 2 || y > gridSizeY - 3)
			return true;
		else {
			for(int r = x - 2;r <= x + 2;r++){
				for(int c = y - 2;c <= y + 2;c++)
				{
					if(grid[r,c].IsObstacle)
						return true;
				}
			}
		}

		return false;

	}

	public bool checkSingleTile(int x,int y)
	{
		return grid[x,y].IsObstacle;
	}
//	Gizmos debugging part
//	void OnDrawGizmos()
//	{
//		//Gizmos are used for visual debugging which can be efficiently used for seeing the building 
//		//transparently before its placement (green if avaliable, red if not)
//		Gizmos.DrawWireCube (transform.position, new Vector2 (gridWorldSize.x, gridWorldSize.y));
//		if (grid != null) {
//
//			//Conversion of point (need to use that convert position part again)
//			Vector2 buildingPos = ConvertLocation(Input.mousePosition);
//			//Checking all the tiles that are part of the building
//			Tile [] buildingTile = new Tile[4]; //Depends on building sizes
//			int x = Mathf.RoundToInt(buildingx);
//			int y = Mathf.RoundToInt(buildingy);
//			buildingTile[0] = grid[x,y];
//			buildingTile[1] = grid[x+1,y-1];
//			buildingTile[2] = grid[x+1,y];
//			buildingTile[3] = grid[x,y-1];
//
//			//Debug.Log(Mathf.RoundToInt(buildingx) + " " + Mathf.RoundToInt(buildingy));
//			foreach(Tile t in buildingTile)
//			{
//				//Determine the tile status and color accordingly
//				//Gizmos.color = (t.isObstacle) ? Color.white: Color.red;
////				if (buildingTile == t)
////				{
//					Gizmos.color = Color.cyan;
//					Gizmos.DrawCube(t.WorldPosition,Vector2.one * (tileDiameter-.1f));
//					//break;
////				}
//
//			}
//		}
//	}
	
}
