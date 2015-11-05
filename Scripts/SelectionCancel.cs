using UnityEngine;
using System.Collections;

public class SelectionCancel : MonoBehaviour {

	SelectionManager selection;
	// Use this for initialization
	void Start () {
	
		selection = transform.parent.GetComponent<SelectionManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Cancel the selection
	void OnMouseDown()
	{
		//CANCEL THE SELECTION IF NON GUI,EMPTY AREA IS CLICKED
		//selection.cancel();
	}
}
