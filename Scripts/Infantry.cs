using UnityEngine;
using System.Collections;

public class Infantry : Soldier {

	void Start()
	{
		range = 15f;
	}

	protected override void Attack(GameObject target)
	{
		Debug.LogError("DIE");
	}
	
}
