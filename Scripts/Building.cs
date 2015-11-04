﻿using UnityEngine;
using System.Collections;

public abstract class Building : MonoBehaviour {

	protected int tier;
	//Variables holding the location indices in grid
	private int x;
	private int y;
	public int X {
		get {
			return x;
		}
		set {
			x = value;
		}
	}

	public int Y {
		get {
			return y;
		}
		set {
			y = value;
		}
	}

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

	protected abstract void upgradeBuilding();
}
