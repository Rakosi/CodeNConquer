using UnityEngine;
using System.Collections;

public class Cavalry : Soldier {
	
	void Start()
	{
		range = 20f;
	}

	protected override void Attack(GameObject target)
	{
		Debug.LogError("DIE");
	}
	
	

}
