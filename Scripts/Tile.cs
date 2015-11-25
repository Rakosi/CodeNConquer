using UnityEngine;
using System.Collections;

public class Tile {

	//Property which is true if there is an obstacle 
	//False if the tile is clear of obstacle
	private bool isObstacle;
	public bool IsObstacle {
		get {
			return isObstacle;
		}
		set {
			isObstacle = value;
		}
	}
	//Property of location of tile on xy coordinates
	private Vector2 worldPosition;

	public Vector2 WorldPosition {
		get {
			return worldPosition;
		}
		set {
			worldPosition = value;
		}
	}

	//These are used for A* algorithm
	//The cost of coming this tile from starting tile
	private int gCost;
	public int GCost {
		get {
			return gCost;
		}
		set {
			gCost = value;
		}
	}
	//The cost of going to target tile from this tile

	private int hCost;
	public int HCost {
		get {
			return hCost;
		}
		set {
			hCost = value;
		}
	}

	private int x;
	public int X {
		get {
			return x;
		}

	}

 	private int y;
	public int Y {
		get {
			return y;
		}
		
	}


	
	public Tile(bool isObstacle,Vector2 worldPosition,int X,int Y){
		this.isObstacle = isObstacle;
		this.worldPosition = worldPosition;
		gCost = int.MaxValue;
		hCost = int.MaxValue;
		x = X;
		y = Y;
	}

	private int fCost;

	public int FCost {
		get {
			return gCost + hCost;
		}
	}
}
