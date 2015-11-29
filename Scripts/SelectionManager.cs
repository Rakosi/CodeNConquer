using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This script manages the selection of buildings together with selection of
//single AND multiple soldiers.
//Normal selection will use raycast to identify friendly selectable units
//
public class SelectionManager : MonoBehaviour {

	//Start location is used for selection rectangle
	public Vector3 startLoc;
	public Texture2D selectionText; // Used for texture of rectangle
	public Rect selectionRect; //Rectangle to be defined 
	//Holds the selected soldiers that are currently under command of user 
	ArrayList selectedS;

	public ArrayList SelectedS {
		get {
			return selectedS;
		}
	}

	Building selectedB;
	Canvas canvas;
	//Used for management of active panel for selected object
	GameObject [] panels;
	int panelIndex; //Determines which panel is currently on
	//Number of different panels
	const int NUM_OF_PANELS = 6;
	Vector3 lastPos;

	Camera cam;
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
		panels[4] = GameObject.Find ("SelectedSoldiers");
		panels[4].SetActive(false);
	//	panels[5] = GameObject.Find ("QuestionPanel");
	//	panels[5].SetActive(false);
		panelIndex = -1; //None of the panels are on

		startLoc = -Vector3.one;
		selectionRect = new Rect(0, 0, 0, 0);
		selectedB = null;
		selectedS = new ArrayList ();

		lastPos = -Vector3.one;
		cam = transform.Find("Camera").GetComponent<Camera>();
	
	}
	
	// In fixed update, selection manager checks if player is doing a multi soldier selection 
	// where user clicks on a location and draws a rectangle.
	void FixedUpdate () {

		if (Input.GetMouseButtonDown (0)) {//User stars drawing

			startLoc = Input.mousePosition;


		} else if (Input.GetMouseButtonUp (0)) {// User has stopped dragging the rectangle so finalize selection
			startLoc = -Vector3.one;
			lastPos = -Vector3.one;


		} else if (Input.GetMouseButton (0)) {//User is currently dragging the rectangle for selection

			lastPos = Input.mousePosition;

			selectionRect = new Rect(startLoc.x, Screen.height - startLoc.y, lastPos.x - startLoc.x,
			                     (Screen.height - lastPos.y) - (Screen.height - startLoc.y));

		}

	}

	//Draw the selection rectangle for multi selection
	void OnGUI()
	{
		if (lastPos != -Vector3.one && startLoc != -Vector3.one) {
			GUI.color = new Color(1,1,1,0.5f);
			GUI.DrawTexture(selectionRect,selectionText);
		}
	}


	//Add selected add soldiers to current selected army as multiple soldiers can be selected
	//But only one building can be active at a time
	public void addSelected(Soldier soldier)
	{
		//Add the soldiers to the list where it waits to for commands

		//If the list already contains the soldier, don't add it second time
		if (!selectedS.Contains (soldier)) {
			cancel (false); //Cancel the current selected building and close its panels together with question panel

			//Add the soldier to the list
			selectedS.Add(soldier);
			panels [4].SetActive (true);

			Text solNumber = null;

			//Modify panel texts
			if(soldier.GetType().ToString () == "Infantry")
				solNumber = panels[4].transform.GetChild(4).GetComponent<Text>();
			else if (soldier.GetType().ToString () == "Archer")
				solNumber = panels[4].transform.GetChild(6).GetComponent<Text>();
			else
				solNumber = panels[4].transform.GetChild(5).GetComponent<Text>();

			solNumber.text = (int.Parse(solNumber.text) + 1).ToString();

		}
	}

	//Used for barracks
	public void updatePanelDynamic(int panelNum, int panelElement, int value)
	{
		if (panelElement != 1) //Not the progress bar
			panels [panelNum].transform.GetChild (panelElement).GetComponent<Text> ().text = value.ToString ();
		else { //Update the progress bar

			if(((Barracks)selectedB).Soldiers.Count != 0){
			panels [panelNum].transform.GetChild (panelElement)
				.GetChild(0).transform.localScale = new Vector3(
						((float)value/(
					(bool)((Barracks)selectedB).Soldiers.Peek() 
					? ((Barracks)selectedB).CavalryTime : ((Barracks)selectedB).InfantryTime)
				 ),1, 1);
			}
			else
				panels [panelNum].transform.GetChild (panelElement)
					.GetChild(0).transform.localScale = new Vector3(0,1,1);
		}

	}

	public void setSelected(Building selectedB)
	{
		//If building is already selected, don't change selection
		if (selectedB != this.selectedB) {
			cancel (true);
			//The menus are already created but we need to activate them in order to use.
			//We do this by acsessing them in the beginning of the selection to hide
		
			//Activate the required panel

			//Update its prices time,queue and avaliablity accordingly according to type

			//Testing the barracks first
			if (selectedB.GetType ().ToString () == "Barracks"){
				panelIndex = 0;
				panels [panelIndex].SetActive (true);

				//Might be necesseary
				panels [panelIndex].transform.GetChild (6).GetComponent<Button>().interactable = true;
				panels [panelIndex].transform.GetChild (7).GetComponent<Button>().interactable = true;
				panels [panelIndex].transform.GetChild (2).GetComponent<Button>().interactable = true;

				//Arrange queue number
				panels [panelIndex].transform.GetChild (5).GetComponent<Text>().text = ((Barracks)selectedB).Soldiers.Count.ToString();

				//The progress will be arranged by barracks itself

				//Arrange armor,attack and upgrade options
				if(!((Barracks)selectedB).ArmorAval)
					panels [panelIndex].transform.GetChild (6).GetComponent<Button>().interactable = false;
				if(!((Barracks)selectedB).AttackAval)
					panels [panelIndex].transform.GetChild (7).GetComponent<Button>().interactable = false;
				if(!((Barracks)selectedB).UpgradeAval)
					panels [panelIndex].transform.GetChild (2).GetComponent<Button>().interactable = false;

			}
			else if (selectedB.GetType ().ToString () == "Archery")
				panelIndex = 1;
			else if (selectedB.GetType ().ToString () == "ResearchBuilding")
				panelIndex = 2;
			else if (selectedB.GetType ().ToString () == "Tower")
				panelIndex = 3;
			//else if (selectedB.GetType ().ToString () == "QuestionBuilding")
			//	panelIndex = 5;
			panels [panelIndex].SetActive (true);
		




		
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
	public void cancel (bool all)
	{
		//If all soldiers are also to be deselected
		if(all)
		{
			foreach(Soldier s in selectedS)
			{
				s.Deselect();
			}
			selectedS.Clear();


			Text solNumber = null;

			solNumber = panels[4].transform.GetChild(4).GetComponent<Text>();
			
			solNumber.text = (0).ToString();

			solNumber = panels[4].transform.GetChild(5).GetComponent<Text>();
			
			solNumber.text = (0).ToString();

			solNumber = panels[4].transform.GetChild(6).GetComponent<Text>();
			
			solNumber.text = (0).ToString();

			panels[4].SetActive(false);
			//also reset the Soldier countings
		}

		if (panelIndex != -1) {
			selectedB.Deselect();
			panels[panelIndex].SetActive(false);
			panelIndex = -1;
			selectedB = null;
		}
	}

}
