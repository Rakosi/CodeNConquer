using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	SpriteRenderer arrow;
	Rigidbody2D arrowB;
	int speed;
	public Vector2 targetLoc;
	string targetTag;
	float dmg;
	Vector2 spawnLoc;

	// Use this for initialization
	void Awake () {
		speed = 10;
		arrow = GetComponent<SpriteRenderer>();
		arrowB = GetComponent<Rigidbody2D>();
		targetLoc = Vector2.zero;
		targetTag = "Enemy"; //Default target is enemy
		spawnLoc = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if ((Vector2)transform.localPosition != targetLoc) {
			arrowB.velocity = targetLoc - spawnLoc;
		}
		if (((Vector2)transform.localPosition - spawnLoc).magnitude > 
		    (targetLoc - spawnLoc).magnitude + 10 ){
			Destroy(transform.gameObject);
		}

	}

	//If collide with anything, considers its side and get destroyed
	void OnTriggerEnter2D (Collider2D collider)
	{

		if (collider.gameObject.tag == targetTag) {

			//If the target is dead, destroy it
			if(collider.gameObject.GetComponentInChildren<Health>().getDamaged(dmg))
			{
				Destroy(collider.gameObject);
			}

			Destroy(transform.gameObject);
		}
	}

	//Define the last location of target and its tag together with damage to be inflected
	public void setTarget (Vector2 position, string tag,float damage)
	{
		targetLoc = position;
		targetTag = tag;
		dmg = damage;
	}
}
