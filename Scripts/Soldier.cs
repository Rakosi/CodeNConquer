using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () {
	
	}
	
	protected abstract void Attack();
}
