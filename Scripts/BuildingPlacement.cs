using UnityEngine;
using System.Collections;

public class BuildingPlacement : MonoBehaviour {

	//This is the building to be constructed on grid system
	//Using polymorphism for easier management
	private Building buildingConstructed;
	Sprite invalidLoc;
	Sprite template;
	Sprite original;
	SpriteRenderer originalRender;
	Grid grid; 
	SelectionManager selection;

	// When the script is loaded there is no building to be created so it is set to null
	void Awake () {
		grid = GetComponentInParent<Grid>();
		buildingConstructed = null;
		selection = GetComponentInParent<SelectionManager>();
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
			if(!grid.CheckTile(pos))
			{
				originalRender.color = Color.gray;
				//User places on a place (avaliable or not)
				buildingConstructed.transform.position = 
					new Vector2(((grid.ConvertLocation(pos).x)*grid.getTileRadius() * 2+ grid.getTileRadius()),
					            ((grid.ConvertLocation(pos).y)*grid.getTileRadius() * 2+ grid.getTileRadius()));
				if(Input.GetMouseButtonDown(0))
				{
					//Place the building
					originalRender.color = Color.white;
					//Convert the position of the building to the nearest avaliable grid tile
					buildingConstructed.gameObject.layer = LayerMask.NameToLayer( "obstacle" );

					//Put the grid indexof created buildings center
					buildingConstructed.X = (int)grid.ConvertLocation(buildingConstructed.transform.position).x;
					buildingConstructed.Y = (int)grid.ConvertLocation(buildingConstructed.transform.position).y;

					Debug.Log(buildingConstructed.X + " " +  buildingConstructed.Y);

					buildingConstructed.gameObject.GetComponent<BoxCollider2D>().enabled = true;
					buildingConstructed = null;
				}

			}
			else{
				originalRender.color = Color.red;
				buildingConstructed.transform.position = 
					new Vector2(((grid.ConvertLocation(pos).x)*grid.getTileRadius() * 2+ grid.getTileRadius()),
					            ((grid.ConvertLocation(pos).y)*grid.getTileRadius() * 2+ grid.getTileRadius()));
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

		//Cancel the selection of building
		selection.cancel();
		switch (buildingName) {
		case "Barracks":

			buildingConstructed = ((Barracks)Instantiate (g, transform.position,transform.rotation));
			break;
		case "Question":

			buildingConstructed = ((QuestionBuilding)Instantiate (g, transform.position,transform.rotation));
			break;
		case "Archery":

			buildingConstructed = ((Archery)Instantiate (g, transform.position,transform.rotation));
			break;
		case "Research":

			buildingConstructed = ((ResearchBuilding)Instantiate (g, transform.position,transform.rotation));
			break;
		case "Tower":

			buildingConstructed = ((Tower)Instantiate (g, transform.position,transform.rotation));
			break;
		default:
			break;

		}

		//We need to assign the transparent sprite which will be displayed as during the construction process
		//together with the original sprite and invalid sprite
		original = Resources.Load (buildingName,typeof(Sprite)) as Sprite;

		//Put the transparent template by changing the material
		originalRender = buildingConstructed.GetComponent<SpriteRenderer>();
		originalRender.sprite = original;
		originalRender.color = Color.gray;

		//Disable collider to have full transparent construction
		buildingConstructed.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		//Change the layer mask to default as it is not a obstacle yet!
		buildingConstructed.gameObject.layer = 0;
		
		
		
	}
}
