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
	}

	//TODO
	//Manage the positioning of the building buttons
	void OnGUI()
	{
		Grid grid = GetComponent<Grid> ();

		//Create a button for each type of building that can be placed
		for (int index = 0; index < buildingsTypes.Length; index++) {
			if(GUI.Button (new Rect(Screen.width - 100,
			                        Screen.height - 100 + (-1 * index * 40),100,40),buildingsTypes[index].name))
			{
				if(buildingPlacement != null)
					buildingPlacement.CreateBuilding(buildingsTypes[index],buildingsTypes[index].name);

			}
		}

		if(GUI.Button (new Rect(Screen.width - 100,
		                        Screen.height - 100 + (-1 * 6 * 40),100,40),"New Game"))
		{
			//Load new game (or scene to be modified) depending on difficulty
			Application.LoadLevel("GameScene");
			
		}

		if(GUI.Button (new Rect(Screen.width - 100,
		                        Screen.height - 100 + (-1 * 7 * 40),100,40),"Pause"))
		{
			if(Time.timeScale == 0)
				Time.timeScale = 1;
			else
				Time.timeScale = 0;
			
		}
	}
}
