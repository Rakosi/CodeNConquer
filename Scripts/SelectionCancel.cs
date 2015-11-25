using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectionCancel : MonoBehaviour {

	SelectionManager selection;

	// Get the selection manager to invoke cancelling
	void Start () {
	
		selection = transform.parent.GetComponent<SelectionManager> ();
	}

	//Cancel the selection when an empty area has been clicked, which is not GUI either
	void OnMouseDown()
	{
		//EventSystem identifies the clicked object as an GUI element
		//Where clicking on it shouldn't cancel the selection. This code doesn't affect 
		//the selection of buildings and soldiers as they have their own MouseDown where it doesn't affect
		if (!EventSystem.current.IsPointerOverGameObject()) {
			selection.cancel(true);
		}
	}
}
