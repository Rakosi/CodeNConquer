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

	public float tileRadius;
	Tile[,] grid;

	public float tileDiameter;
	int gridSizeX,gridSizeY;

	void Awake()
	{
		tileRadius = 3f;
		gridWorldSize = new Vector2 (356, 200);
		tileDiameter = tileRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / tileDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / tileDiameter);
		InstantiateGrid ();
	}

	void InstantiateGrid()
	{
		grid = new Tile[gridSizeX, gridSizeY];
		Vector2 worldBottomLeft = new Vector2(0,0);
		for (int x = 0;x < gridSizeX;x++){
			for (int y = 0;y < gridSizeY;y++){
				Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * tileDiameter + tileRadius) + Vector2.up * (y * tileDiameter + tileRadius);
				//Physics part is used to check for colliders in the already position, useful for placement of buildings and pathfinding
				bool isObstacle;
				//RaycastHit obstacle;
				//Ray check = new Ray(worldPoint,Vector3.back);
				var layer = LayerMask.NameToLayer("obstacle");
				layer = 1 << layer;
//				if(Physics.Raycast (check,out obstacle,10,layer))
//				{
//					isObstacle = true;
//				}
//				else
//					isObstacle = false;
//				
				isObstacle = Physics2D.OverlapArea (new Vector2(worldPoint.x - tileRadius,worldPoint.y + tileRadius),
				                                    new Vector2(worldPoint.x + tileRadius,worldPoint.y - tileRadius), layer);
				
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
		float percentX = (worldPosition.x + tileDiameter) / gridWorldSize.x;
		float percentY = (worldPosition.y + tileDiameter) / gridWorldSize.y;

		//Checks if the character in the world edges
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((gridSizeX) * percentX);
		int y = Mathf.RoundToInt((gridSizeY) * percentY);
		return new Vector2 (x, y);
	}

	//Constantly updates the whole grid each frame so building construction A* works as accurate as possible
	//As the game scene is constant, it is not a performance problem 
	void FixedUpdate()
	{
		foreach (Tile t in grid) {

			//RaycastHit obstacle;
			//Ray check = new Ray(t.WorldPosition,Vector3.back);
			var layer = LayerMask.NameToLayer("obstacle");
			layer = 1 << layer;

//			if(Physics.Raycast (check,out obstacle,10,layer))
//			{
//				t.IsObstacle = true;
//			}
//			else
//				t.IsObstacle = false;

			t.IsObstacle = Physics2D.OverlapArea (new Vector2(t.WorldPosition.x - tileRadius,t.WorldPosition.y + tileRadius),
			                                      new Vector2(t.WorldPosition.x + tileRadius,t.WorldPosition.y - tileRadius), layer);
			
			//if(t.IsObstacle)
			//	Debug.Log(t.WorldPosition);
		}
	}

	public bool CheckTile (Vector3 pos)
	{
		Vector2 pos2 = ConvertLocation(pos);
		int x = Mathf.RoundToInt (pos2.x);
		int y = Mathf.RoundToInt (pos2.y);

		return grid[x,y].IsObstacle || grid[x-2,y-1].IsObstacle || grid[x-1,y-2].IsObstacle
				|| grid[x-2,y-2].IsObstacle || grid[x,y-1].IsObstacle || grid[x-2,y].IsObstacle
				|| grid[x-1,y].IsObstacle || grid[x,y].IsObstacle || grid[x,y-2].IsObstacle;

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
