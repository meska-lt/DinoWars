/// <summary>
/// Main menu.
/// Attached to Main Camera
/// </summary>

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Texture backgroundTexture;

	void OnGUI() {
		// Display our background sprite
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);

		// Display buttons
		if (GUI.Button (new Rect (Screen.width * .4f, Screen.height * .58f, Screen.width * .2f, Screen.height * .1f), "1 PLAYER")) {
			Debug.Log("Start button clicked.");
			Application.LoadLevel("Main");
		}
	}
}