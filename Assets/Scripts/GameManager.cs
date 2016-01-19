using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour {
	public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script.

	private BoardManager boardScript; //Store a reference to our BoardManager which will set up the level.
	public int level = 1; //Current level number, expressed in game as "Day 1".
	

//	[HideInInspector] public bool playersTurn = true;
	
	//Awake is always called before any Start functions
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject); //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.

		DontDestroyOnLoad(gameObject); //Sets this to not be destroyed when reloading scene
		boardScript = GetComponent<BoardManager>(); //Get a component reference to the attached BoardManager script
		//Call the InitGame function to initialize the first level 
		InitGame();
	}
	
	//Initializes the game for each level.
	public void InitGame()
	{
		boardScript.SetupScene(level); //Call the SetupScene function of the BoardManager script, pass it current level number.
	}


	public void GameOver() 
	{
		if (!GameOverManager.instance.timeToFinish)
		{
			GameOverManager.instance.timeToFinish = true;
		}
	}
	
	//Update is called every frame.
	void Update()
	{
//		playersTurn = true;
	}

}