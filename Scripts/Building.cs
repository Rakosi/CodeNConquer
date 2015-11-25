using UnityEngine;
using System.Collections;

public abstract class Building : MonoBehaviour {

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

	//Health can be considered as a component which will be affected by attacks

	void Start()
	{

	}
	protected abstract void upgradeBuilding();
}
