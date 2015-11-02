using UnityEngine;
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
		//buildings = new Building[MAX_NUM_OF_BUILDINGS];
	}

	//TODO
	//Manage the positioning of the building buttons
	void OnGUI()
	{
		Grid grid = GetComponent<Grid> ();

		//Create a button for each type of building that can be placed
		for (int index = 0; index < buildingsTypes.Length; index++) {
			if(GUI.Button (new Rect(grid.GridWorldSize.x-25,
			                        grid.GridWorldSize.y + (-1 * index * 35),100,40),buildingsTypes[index].name))
			{
				if(buildingPlacement != null)
					buildingPlacement.CreateBuilding(buildingsTypes[index],buildingsTypes[index].name);

			}
		}
	}
}
