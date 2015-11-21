using UnityEngine;
using System.Collections;

public class ResearchBuilding : Building {

	//Time required for each soldier type
	int timer;
	//	Health health;
	SpriteRenderer render;
	Grid grid;
	SelectionManager selection;
	
	#region implemented abstract members of Building
	public override void upgradeBuilding ()
	{
		throw new System.NotImplementedException ();
	}
	#endregion
	
	// Use this for initialization
	void Start () {
		tier = 1;
		
		grid = GameObject.FindGameObjectWithTag ("Grid").GetComponent<Grid> ();
		render = GetComponent<SpriteRenderer>();
		selection = grid.GetComponent<SelectionManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//Clicked on this barracks, send the info about getting clicked to selection manager
	void OnMouseDown()
	{
		render.color = Color.yellow;
		selection.setSelected (this);
	}
	
	public override void Deselect()
	{
		render.color = Color.white;
	}
}
