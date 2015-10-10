using UnityEngine;
using System.Collections;

public class Tile {

	public bool isObstacle {
		get {
			return isObstacle;
		}
		set {
			isObstacle = value;
		}
	}

	public Vector2 worldPosition;
	
	public Tile(bool isObstacle,Vector2 worldPosition){
		this.isObstacle = isObstacle;
		this.worldPosition = worldPosition;
	}
	
}
