﻿using UnityEngine;
using System.Collections;

public class QuestionBuilding : Building {

	#region implemented abstract members of Building
	public override void upgradeBuilding ()
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
	public override void Deselect()
	{
//		render.color = Color.white;
	}
}
