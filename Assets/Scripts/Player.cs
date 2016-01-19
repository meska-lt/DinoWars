using UnityEngine;
using System.Collections;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
	private const string AnimatorTriggerIdle = "PlayerIdle";
	private const string AnimatorTriggerMoveUp = "DinoUp";
	private const string AnimatorTriggerMoveRight = "DinoRight";
	private const string AnimatorTriggerMoveDown = "DinoDown";
	private const string AnimatorTriggerMoveLeft = "DinoLeft";

//	private int lastHitWallId = -1;

//	public float restartLevelDelay = 1f; //Delay time in seconds to restart level.
//	public int pointsPerFood = 10; //Number of points to add to player food points when picking up a food object.
//	public int pointsPerSoda = 20; //Number of points to add to player food points when picking up a soda object.
//	public int wallDamage = 1; //How much damage a player does to a wall when chopping it.

	private Animator animator; //Used to store a reference to the Player's animator component.
//	private int food; //Used to store player food points total during level.

	//Start overrides the Start function of MovingObject
	protected override void Start()
	{
		animator = GetComponent<Animator>(); //Get a component reference to the Player's animator component
//		food = GameManager.instance.playerFoodPoints; //Get the current food point total stored in GameManager.instance between levels.		
		base.Start(); //Call the Start function of the MovingObject base class.
	}

	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable()
	{
		//When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
//		GameManager.instance.playerFoodPoints = food;
	}
	
	
	private void Update()
	{
		//If it's not the player's turn, exit the function.
//		if(!GameManager.instance.playersTurn)
//			return;

		int horizontal = 0; //Used to store the horizontal move direction.
		int vertical = 0; //Used to store the vertical move direction.

		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		
		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int) (Input.GetAxisRaw ("Vertical"));
		
		//Check if moving horizontally, if so set vertical to zero.
		if(horizontal != 0)
		{
			vertical = 0;
		}
		
		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0) {
			//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
			//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			AttemptMove<Wall>(horizontal, vertical);
		}
	}
	
	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
//		food--; //Every time player moves, subtract from food points total.
	
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove <T> (xDir, yDir);

		RaycastHit2D hit; //Hit allows us to reference the result of the Linecast done in Move.
		
		//If Move returns true, meaning Player was able to move into an empty space.
		if (Move (xDir, yDir, out hit)) 
		{
//			lastHitWallId = -1;

			if ((xDir == 0) && (yDir == 1) && !animator.GetBool(AnimatorTriggerMoveUp))
				animator.SetBool(AnimatorTriggerMoveUp, true);
			else if ((xDir == 1) && (yDir == 0) && !animator.GetBool(AnimatorTriggerMoveRight))
				animator.SetBool(AnimatorTriggerMoveRight, true);
			else if ((xDir == 0) && (yDir == -1) && !animator.GetBool(AnimatorTriggerMoveDown))
				animator.SetBool(AnimatorTriggerMoveDown, true);
			else if ((xDir == -1) && (yDir == 0) && !animator.GetBool(AnimatorTriggerMoveLeft))
				animator.SetBool(AnimatorTriggerMoveLeft, true);
		}

//		CheckIfGameOver (); //Since the player has moved and lost food points, check if the game has ended.
		
		//Set the playersTurn boolean of GameManager to false now that players turn is over.
//		GameManager.instance.playersTurn = false;
	}

	protected override void OnDoneMoving ()
	{
		animator.SetBool(AnimatorTriggerMoveUp, false);
		animator.SetBool(AnimatorTriggerMoveRight, false);
		animator.SetBool(AnimatorTriggerMoveDown, false);
		animator.SetBool(AnimatorTriggerMoveLeft, false);
		animator.SetBool(AnimatorTriggerIdle, true);
	}
	
	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove <T> (T component)
	{
		if (component == null)
			return;
		print ("Wall hit.");
		Wall wall = component as Wall;
		wall.DestroyWall();
	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		//Check if the tag of the trigger collided with is Exit.
//		if(other.tag == "Exit")
//		{
//			Invoke ("Restart", restartLevelDelay); //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
//			enabled = false; //Disable the player object since level is over.
//		}
		//Check if the tag of the trigger collided with is Food.
//		else if(other.tag == "Food")
//		{			
//			food += pointsPerFood; //Add pointsPerFood to the players current food total.						
//			other.gameObject.SetActive(false); //Disable the food object the player collided with.
//		}		
		//Check if the tag of the trigger collided with is Soda.
//		else if(other.tag == "Soda")
//		{
//			food += pointsPerSoda; //Add pointsPerSoda to players food points total
//			other.gameObject.SetActive(false); //Disable the soda object the player collided with.
//		}
	}
	
	
	//Restart reloads the scene when called.
	private void Restart ()
	{
		Application.LoadLevel(Application.loadedLevel); //Load the last scene loaded, in this case Main, the only scene in the game.
	}
	
	//LoseFood is called when an enemy attacks the player.
	//It takes a parameter loss which specifies how many points to lose.
	public void LoseFood(int loss)
	{
//		animator.SetTrigger ("playerHit"); //Set the trigger for the player animator to transition to the playerHit animation.		
//		food -= loss; //Subtract lost food points from the players total.
//		CheckIfGameOver (); //Check to see if game has ended.
	}	
	
	//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
	private void CheckIfGameOver ()
	{
		//Check if food point total is less than or equal to zero.
//		if (food <= 0) 
//		{
//			GameManager.instance.GameOver (); //Call the GameOver function of GameManager.
//		}
	}
}