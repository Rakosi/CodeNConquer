﻿using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour {

	BuildingManager buildingManager;
    BuildingPlacement buildingPlacement;
	public Building[] buildingsTypes;
	int buildingsIndex;

	void Start()
	{
		//Singleton
		buildingManager = this;
		buildingsIndex = 0;
		buildingPlacement = GetComponent<BuildingPlacement> ();
	}

	//TODO
	//Manage the positioning of the building buttons
	void OnGUI()
	{
		Grid grid = GetComponent<Grid> ();

		//Create a button for each type of building that can be placed
		for (int index = 0; index < buildingsTypes.Length; index++) {
			if(GUI.Button (new Rect(grid.GridWorldSize.x-5,
			                        grid.GridWorldSize.y + (-1 * index * 40),100,40),buildingsTypes[index].name))
			{
				if(buildingPlacement != null)
					buildingPlacement.CreateBuilding(buildingsTypes[index],buildingsTypes[index].name);

			}
		}
		if (GUI.Button (new Rect (250,250, 100, 40), "Create infantry"))
			GameObject.FindGameObjectWithTag ("Building").GetComponent<Barracks>().createSoldier (false);
		if (GUI.Button (new Rect (250,290, 100, 40), "Create Cavalry"))
			GameObject.FindGameObjectWithTag ("Building").GetComponent<Barracks>().createSoldier (true);
		if (GUI.Button (new Rect (250,330, 100, 40), "Create Archer"))
			GameObject.FindGameObjectWithTag ("Building").GetComponent<Archery>().createSoldier();
	}
}
