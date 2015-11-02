using UnityEngine;
using System.Collections;

public class BuildingPlacement : MonoBehaviour {

	//This is the building to be constructed on grid system
	//Using polymorphism for easier management
	private Building buildingConstructed;
	private int tileOccupied;
	Material invalidLoc;
	Material template;
	Material original;
	MeshRenderer originalRender;
	Grid grid; 

	// When the script is loaded there is no building to be created so it is set to null
	void Awake () {
		template = Resources.Load ("Transparent",typeof(Material)) as Material;
		invalidLoc = Resources.Load ("Invalid",typeof(Material)) as Material;
		original = Resources.Load ("Normal",typeof(Material)) as Material;
		grid = GetComponentInParent<Grid>();
		buildingConstructed = null;
	}

	//TODO
	//Place the building to the nearest avaliable tile
	//If avaliable, show transparent version instead of real
	//When avaliable clicked, put the real building at place and initialize
	//If not dont show transparent and don nothing
	//Then work on selection of the building and start soldier creation

	
	// Update function is used for providing the player a transparent sprite to
	//show the avaliablity of the location on grid system
	void FixedUpdate () {
	
		//If player is currently constructing a building, the cursor position is followed 
		//in order to show the transparent consturction template (sized 1x1,2x2,3x3 depending on building to be constructed)
		if (buildingConstructed != null) {
			Camera cam = transform.Find("Camera").GetComponent<Camera>();
			Vector3 pos = Input.mousePosition;

			//Consider the distance from the camera as Screen to world position needs it
			//If its left as 0, the point will be on camera axis and reflected on
			//screen accordingly
			pos.z = 200;
			pos = cam.ScreenToWorldPoint(pos);

			//Show the transparent sprite only when the tiles below are avaliable
			//In other cases, don't show the building template
			if(!grid.CheckTile(pos,tileOccupied))
			{
				originalRender.material = template;
				//User places on a place (avaliable or not)
				buildingConstructed.transform.position = 
					new Vector2(((grid.ConvertLocation(pos).x - 1)*grid.tileDiameter + grid.tileRadius),
					            ((grid.ConvertLocation(pos).y - 1)*grid.tileDiameter + grid.tileRadius));
				buildingConstructed.transform.position = buildingConstructed.transform.position + Vector3.back * 5f;
				if(Input.GetMouseButtonDown(0))
				{
					//Place the building
					originalRender.material = original;
					//Convert the position of the building to the nearest avaliable grid tile
					buildingConstructed.gameObject.layer = LayerMask.NameToLayer( "obstacle" );
					buildingConstructed.location = buildingConstructed.transform.position;
					buildingConstructed = null;
				}

			}
			else{
				originalRender.material = invalidLoc;
			}

			//buildingConstructed.location = pos;

			//User cancels the construction
			if (Input.GetMouseButtonDown(1))
			{
				Destroy(buildingConstructed.gameObject);
			}


		}
	}

	//Puts the building to be created to the building property of the building placement class
	public void CreateBuilding(Building g,string buildingName)
	{
		switch (buildingName) {
		case "Barracks":
				tileOccupied = 3;
				buildingConstructed = ((Barracks)Instantiate (g, transform.position,transform.rotation));
				//Put the transparent template by changing the material
				originalRender = buildingConstructed.GetComponent<MeshRenderer>();
				originalRender.material = template;
				original = Resources.Load ("Barracks",typeof(Material)) as Material;

				//Change the layer mask to default as it is not a obstacle yet!
				buildingConstructed.gameObject.layer = 0;
				break;
		default:
			break;
		}



	}
}
