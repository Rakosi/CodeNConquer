using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//This is the main class of the game that manages everything. It keeps the list of all enemies,
//buildings and units and use them for saving,loading the game

//It also has the wave where it sends the enemies in random directions on arbitrary numbers
//depending on the difficulty

public class GameEngine : MonoBehaviour {

	int maxTier;
	public int MaxTier {
		get{
			return maxTier;
		}
	}

	GameEngine engine;

	//Hold arraylists for soldiers,enemy,buildings and special for question buildings
	List <Soldier> soldiers;
	List <Soldier> enemies;
	List<Building> buildings;
	List<Building> Qbuildings;

	//Hold the current tier of the base
	int baseTier;

	public int BaseTier {
		get {
			return baseTier;
		}
		set {
			baseTier = value;
		}
	}

	int wave;

	public int Wave {
		get {
			return wave;
		}
		set {
			wave = value;
		}
	}

	int difficulty; //Difficulty numbers 0 = none, 1 = easy, 2 =medium, 3= hard

	int userpoints;

	public int Userpoints {
		get {
			return Userpoints;
		}
	}

	//Poitns can be negative to deduct
	public void addPoints(int points)
	{
		userpoints -= points;
	}

	void Awake () {
	
		engine = this;
		soldiers = new List <Soldier> ();
		enemies = new List <Soldier> ();
		buildings = new List <Building> ();
		Qbuildings = new List<Building>();

		wave = 1;
		difficulty = 0;

		userpoints = 0;
		baseTier = 1; //Might change
		maxTier = 3;

	}
	
	void defineDiff(int difficulty)
	{
		this.difficulty = difficulty;
	}

	void FixedUpdate () {
	
	}
}
