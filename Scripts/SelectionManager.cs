using UnityEngine;
using System.Collections;

//This script manages the selection of buildings together with selection of
//single AND multiple soldiers.
//Normal selection will use raycast to identify friendly selectable units
//
public class SelectionManager : MonoBehaviour {


	//Holds the selected soldiers that are currently under command of user 
	//	ArrayList <Soldier> selectedS;
	Building selectedB;
	Canvas canvas;
	//Used for management of active panel for selected object
	GameObject [] panels;
	int panelIndex; //Determines which panel is currently on
	//Number of different panels
	const int NUM_OF_PANELS = 6;
	// Use this for initialization
	void Start () {
		//Each panel holds reference to gui panel and activates,updates it whenever necesseary
		panels = new GameObject[NUM_OF_PANELS];
		panels[0] = GameObject.Find ("BarracksPanel");
		panels[0].SetActive(false);
		panels[1] = GameObject.Find ("ArcheryPanel");
		panels[1].SetActive(false);
		panels[2] = GameObject.Find ("ResearchPanel");
		panels[2].SetActive(false);
		panels[3] = GameObject.Find ("TowerPanel");
		panels[3].SetActive(false);
		panelIndex = -1; //None of the panels are on

		selectedB = null;
	}
	
	// Update is called once per frame
	void Update () {

		//selectedB.Deselect ();
	}
	
	void commandSoldier()
	{
		
	}
	
	//	ArrayList<Soldier> getSelected()
	//	{
	//
	//	}


	public void setSelected(Building selectedB)
	{
		//If building is already selected, don't change selection
		if (selectedB != this.selectedB) {
			cancel ();
			//The menus are already created but we need to activate them in order to use.
			//We do this by acsessing them in the beginning of the selection to hide
		
			//Activate the required panel
			if (selectedB.GetType ().ToString () == "Barracks")
				panelIndex = 0;
			else if (selectedB.GetType ().ToString () == "Archery")
				panelIndex = 1;
			else if (selectedB.GetType ().ToString () == "ResearchBuilding")
				panelIndex = 2;
			else if (selectedB.GetType ().ToString () == "Tower")
				panelIndex = 3;
			panels [panelIndex].SetActive (true);
		
			//TODO:Update its prices and time,queue according to type
		
			this.selectedB = selectedB;
		}
	}

	public void createInfantry()
	{
		((Barracks)selectedB).createInfantry();
	}

	public void createCavalry()
	{
		((Barracks)selectedB).createCavalry();
	}

	public void createArcher()
	{
		((Archery)selectedB).createSoldier();
	}
	
	public void upgradeBarracks()
	{
		((Barracks)selectedB).upgradeBuilding();
	}

	public void upgradeArchery()
	{
		((Archery)selectedB).upgradeBuilding();
	}

	//Cancel the latest selection by returning everything to default
	public void cancel ()
	{
		if (panelIndex != -1) {
			//emptySelectedSoldiers();
			selectedB.Deselect();
			panels[panelIndex].SetActive(false);
			panelIndex = -1;
			selectedB = null;
		}
	}

}
