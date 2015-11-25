using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OutputQScript : MonoBehaviour {


	public Button sel1, sel2, sel3, sel4;
	public Button[] buttons;
	public Text resultText;
	//public bool answered = false;

	int correctSelection = 3;
	int userSelection;

	// Use this for initialization
	void Start () {

		sel1.onClick.AddListener (() => buttonClicked (0));
		sel2.onClick.AddListener (() => buttonClicked (1));
		sel3.onClick.AddListener (() => buttonClicked (2));
		sel4.onClick.AddListener (() => buttonClicked (3));

		buttons = new Button[4] {sel1,sel2,sel3,sel4};
		//sel1.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void buttonClicked(int index)
	{
//		switch (index) {
//		case(1): sel1.image.color = Color.green; break;
//		case(2): sel2.image.color = Color.green; break;
//		default: break;
//		}

  
		userSelection = index;

		for(int i = 0; i<4; i++)
		{
			if(i == userSelection)
				buttons[i].image.color = Color.red;
			if(i == correctSelection)
				buttons[i].image.color = Color.green;

			buttons[i].enabled = false;
		}

		if (userSelection == correctSelection) {
			resultText.color = Color.green;
			resultText.text = "Correct Answer!";
		}

		else
		{
			resultText.color = Color.red;
			resultText.text = "Incorrect Answer!";
		}







	}
}
