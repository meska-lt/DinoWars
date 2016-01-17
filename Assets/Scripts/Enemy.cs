using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
	private const string AnimatorTriggerIdle = "EnemyIdle";
	private const string AnimatorTriggerMoveUp = "EnemyUp";
	private const string AnimatorTriggerMoveRight = "EnemyRight";
	private const string AnimatorTriggerMoveDown = "EnemyDown";
	private const string AnimatorTriggerMoveLeft = "EnemyLeft";

	private const int TimesStandingStillMax = 60;

	private Animator animator;
	private int lastHorizontalDirection = 0;
	private int lastVerticalDirection = 0;
	private int timeStandingStill = 0;
	bool animationOngoing = false;
	
	protected override void Start()
	{
		animator = GetComponent<Animator>(); //Get a component reference to the Player's animator component
		base.Start(); //Call the Start function of the MovingObject base class.
	}
	
	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable()
	{
	}

	private void Update()
	{
		//If it's not the player's turn, exit the function.
//		if(!GameManager.instance.playersTurn)
//			return;
		
		int horizontal = (int) Random.Range(-1, 2);
		int vertical = 0;
		bool canMove = false;
		RaycastHit2D hit;

		if (horizontal == 0)
			while (vertical == 0)
				vertical = (int)Random.Range (-1, 2);

		canMove = Move (horizontal, vertical, out hit);

		if (!canMove && (timeStandingStill + 1 > TimesStandingStillMax)) {
			vertical = lastVerticalDirection * -1;
			horizontal = lastHorizontalDirection * -1;
		}
		else if (!canMove) {
			timeStandingStill++;
		}

		//Check if we have a non-zero value for horizontal or vertical
		if (horizontal != 0 || vertical != 0) {
			timeStandingStill = 0;

			//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
			//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
			lastHorizontalDirection = horizontal;
			lastVerticalDirection = vertical;
		}
		
		AttemptMove<Wall>(horizontal, vertical);
	}
	
	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove <T> (xDir, yDir);
		
//		RaycastHit2D hit; //Hit allows us to reference the result of the Linecast done in Move.

//		bool canMove = Move (xDir, yDir, out hit);
		if (!base.animationOngoing) {
			animator.SetBool (AnimatorTriggerIdle, false);
			
			if ((xDir == 0) && (yDir == 1) && !animator.GetBool (AnimatorTriggerMoveUp))
				animator.SetBool (AnimatorTriggerMoveUp, true);
			else if ((xDir == 1) && (yDir == 0) && !animator.GetBool (AnimatorTriggerMoveRight))
				animator.SetBool (AnimatorTriggerMoveRight, true);
			else if ((xDir == 0) && (yDir == -1) && !animator.GetBool (AnimatorTriggerMoveDown))
				animator.SetBool (AnimatorTriggerMoveDown, true);
			else if ((xDir == -1) && (yDir == 0) && !animator.GetBool (AnimatorTriggerMoveLeft))
				animator.SetBool (AnimatorTriggerMoveLeft, true);
			else
				animator.SetBool (AnimatorTriggerIdle, true);

			base.animationOngoing = true;
		}

		//If Move returns true, meaning Player was able to move into an empty space.
		//Since the player has moved and lost food points, check if the game has ended.
//		CheckIfGameOver ();
		
		//Set the playersTurn boolean of GameManager to false now that players turn is over.
//		GameManager.instance.playersTurn = false;
	}
	
	protected override void OnDoneMoving ()
	{
		animator.SetBool (AnimatorTriggerMoveUp, false);
		animator.SetBool (AnimatorTriggerMoveRight, false);
		animator.SetBool (AnimatorTriggerMoveDown, false);
		animator.SetBool (AnimatorTriggerMoveLeft, false);
		animator.SetBool (AnimatorTriggerIdle, true);
	}
	
	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove <T> (T component)
	{
		OnDoneMoving ();
//		Wall hitWall = component as Wall; //Set hitWall to equal the component passed in as a parameter.
//		hitWall.DamageWall(wallDamage); //Call the DamageWall function of the Wall we are hitting.
//		animator.SetTrigger("playerChop"); //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
	}
	
	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("Enemy OnTriggerEnter2D");
		//Check if the tag oßf the trigger collided with is Exit.
		if(other.tag == "Player")
		{
//			Invoke ("Restart", restartLevelDelay); //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
//			enabled = false; //Disable the player object since level is over.
			GameManager.instance.GameOver();
		}
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