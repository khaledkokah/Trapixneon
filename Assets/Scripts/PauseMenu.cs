using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

//== Game pause menu originally created by "Please modify here" ==
public class PauseMenu : MonoBehaviour {
    
    //Variable to store pause state 
	public static bool GameIsPaused = false;

    //Pause menu game object
	public GameObject pauseMenuUI;

    //Puase button game object
	public GameObject pauseButton;

    //Called when resume button clicked
	public void Resume(){
		pauseButton.SetActive (true);
		pauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

    //Called when pause button clicked
	public void Pause(){
        //
		pauseButton.SetActive (false);
		pauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

    //Load main menu scene
	public void LoadMenu(){
		Time.timeScale = 1f;
		SceneManager.LoadScene ("_Menu");
	}

    //Quit the application
	public void QuitGame(){
		Debug.Log ("Application quit.");
		Application.Quit ();
	}
}
