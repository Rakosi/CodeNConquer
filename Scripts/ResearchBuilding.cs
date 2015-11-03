using UnityEngine;
using System.Collections;

public class ResearchBuilding : Building {

	#region implemented abstract members of Building
	protected override void upgradeBuilding ()
	{
		throw new System.NotImplementedException ();
	}
	#endregion
	
	// Use this for initialization
	void Start () {
		tier = 1; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
