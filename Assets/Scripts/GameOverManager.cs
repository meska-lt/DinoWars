using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
	public static GameOverManager instance = null;

	public bool timeToFinish = false;
	public float restartDelay = 5f;

	private bool finishOngoing = false;

	Animator anim;
	float restartTimer;

	private void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject); //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
		
		DontDestroyOnLoad(gameObject); //Sets this to not be destroyed when reloading scene

		anim = GetComponent<Animator> ();
	}

	void Update ()
	{
		if (timeToFinish && !finishOngoing)
		{
			finishOngoing = true;
			anim.SetTrigger("GameOver");
			restartTimer = Time.timeSinceLevelLoad;
		}
		/*
		if (finishOngoing && ((restartDelay+restartTimer) < Time.timeSinceLevelLoad))
		{
			GameManager.instance.level += 1;
			GameManager.instance.InitGame();
			restartTimer = 0f;
			timeToFinish = false;
			finishOngoing = false;
			anim.ResetTrigger("GameOver");
			anim.SetTrigger("GameStarts");
		}
		*/
	}
}
