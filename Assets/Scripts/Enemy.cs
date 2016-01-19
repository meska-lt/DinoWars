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
	private Transform target;                           //Transform to attempt to move toward each turn.

	protected override void Start()
	{
		animator = GetComponent<Animator>(); //Get a component reference to the Player's animator component
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start(); //Call the Start function of the MovingObject base class.
	}
	
	//This function is called when the behaviour becomes disabled or inactive.
	private void OnDisable()
	{

	}

	private void Update()
	{
		MoveEnemy();
	}
	
	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove <T> (xDir, yDir);	
		RaycastHit2D hit; //Hit allows us to reference the result of the Linecast done in Move.
		if (!Move (xDir, yDir, out hit))
			return;
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
		if (component == null)
			return;

		Wall wall = component as Wall;
		wall.DestroyWall();

		//Set the attack trigger of animator to trigger Enemy attack animation.
//		animator.SetTrigger ("enemyAttack");
	}
	
	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == "Player")
		{
			Debug.Log ("Player hit.");
			GameManager.instance.GameOver();
//			Invoke ("Restart", 1);
		}
	}
	
	
	//Restart reloads the scene when called.
	private void Restart ()
	{
		Application.LoadLevel(Application.loadedLevel); //Load the last scene loaded, in this case Main, the only scene in the game.
	}
	
	public void MoveEnemy ()
	{
		//Declare variables for X and Y axis move directions, these range from -1 to 1.
		//These values allow us to choose between the cardinal directions: up, down, left and right.
		int xDir = 0;
		int yDir = 0;
		
		//If the difference in positions is approximately zero (Epsilon) do the following:
		if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			
			//If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
			yDir = target.position.y > transform.position.y ? 1 : -1;
		
		//If the difference in positions is not approximately zero (Epsilon) do the following:
		else
			//Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
			xDir = target.position.x > transform.position.x ? 1 : -1;


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

		//Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
		AttemptMove <Wall> (xDir, yDir);
	}
}