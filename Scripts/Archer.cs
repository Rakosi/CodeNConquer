using UnityEngine;
using System.Collections;

public class Archer : Soldier {

	void Start()
	{
		range = 100f;
	}

	protected override void Attack(GameObject target)
	{
		Debug.LogError("DIE");
	}

}
