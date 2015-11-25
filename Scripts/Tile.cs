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
	
	public Tile(bool isObstacle,Vector2 worldPosition){
		this.isObstacle = isObstacle;
		this.worldPosition = worldPosition;
	}
	
}
