using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using Assets.Scripts.Helper;

//== The GameManager contains all the logic of the current active level ==
public class GameManager : MonoBehaviour
{
    //Static instance of GameManager which allows it to be accessed by  other scripts
    public static GameManager instance = null;

    //Define player and enemey prefabs
    public GameObject player;
    public GameObject enemyPrefab;

    //Flags to determine if game has started or is finished
    public static bool gameStarted;
    public static bool gameFinished = false;

    //In-game panels, win, lose and pause
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;

    //GUI text containing the current level number 
    public Text txtLevelNo;

    //GUI text containing the current level time 
    public Text txtTime;

    //GUI text containing remaining jewels
    public Text txtJewels;

    //Variable containing remaining jewels
    public static int jewels;

    //Default level time
    public float levelTime = 30;

    //Game music audio clip
    public AudioClip gameMusicAudioClip;

    private float m_StartTime;

    //Pause button button
    public GameObject pauseBtn;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this; //if not, set instance to this
        else if (instance != this)
            Destroy(gameObject); //then destroy this, this enforces singleton pattern, meaning there can only ever be one instance of an GameManager

        //Get the total count of jewels from the scene
        GameObject[] totalJewels = GameObject.FindGameObjectsWithTag("Jewel");
        jewels = totalJewels.Length;
    }

    void Start()
    {
        //Setup all startup game stuff
        Setup();

        //Start the gameplay objects and flags
        StartPlay();
    }

    //Setup all startup game stuff
    public void Setup()
    {
        //Indicate that gameFinsihed is false
        gameFinished = false;

        //Set start time counter 
        m_StartTime = Time.time;

        //Assign level number to gui
        txtLevelNo.text = "Level: " + Application.loadedLevelName.After("_");

        //Get all UI Buttons in order to assign them click sound
        GameObject btnResume = GeneralMethods.GetChildWithName(pausePanel, "BtnResume");
        GameObject btnExitPause = GeneralMethods.GetChildWithName(pausePanel, "BtnExit");
        GameObject btnRetry = GeneralMethods.GetChildWithName(losePanel, "BtnRestart");
        GameObject btnExitLose = GeneralMethods.GetChildWithName(losePanel, "BtnExit");
        GameObject btnNext = GeneralMethods.GetChildWithName(winPanel, "BtnNext");
        GameObject btnExitWin = GeneralMethods.GetChildWithName(winPanel, "BtnExit");

        //Assign buttons the click sound when clicked (instead of doing it in inspector manually)
        GeneralMethods.AssignSoundToButton(pauseBtn, "Click");
        GeneralMethods.AssignSoundToButton(btnResume, "Click");
        GeneralMethods.AssignSoundToButton(btnExitPause, "Click");
        GeneralMethods.AssignSoundToButton(btnRetry, "Click");
        GeneralMethods.AssignSoundToButton(btnExitLose, "Click");
        GeneralMethods.AssignSoundToButton(btnNext, "Click");
        GeneralMethods.AssignSoundToButton(btnExitWin, "Click");

        //Save that this level has been accessed so the MainMenu can enable it
        PlayerPrefManager.UnlockLevel();
    }

    //Start the gameplay objects and flags
    public void StartPlay()
    {
        //Indicate that gameplay started
        gameStarted = true;

        //Activate jewels UI 
        txtJewels.gameObject.SetActive(true);

        //Play level music
        PlayMusic(gameMusicAudioClip);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if game is finished or not
        if (!gameFinished)
        {
            //Apply countdown on levelTime 
            levelTime -= Time.deltaTime;

            //If levelTime is less than or equal zero, game over
            if (levelTime <= 0)
            {
                levelTime = 0; //Set to zero manually since it is float sometimes it can still have fraction
                LevelLose(); //Call LevelLose method that contains all lose logic
            }
        }

        //Update the GUI texts of time and jewels
        txtTime.text = "Time: " + levelTime.ToString("00");
        txtJewels.text = "Jewels:" + jewels.ToString();
    }

    //Level won logic
    public void LevelWon(int levelIndex = 0)
    {
        //Set gameFinished to true (important to indicate to other classes that game is not in play state)
        gameFinished = true;

        //Show winPanel
        winPanel.SetActive(true);

        //Make sure to hide pause panel
        pauseBtn.SetActive(false);

        //== Save the current player prefs before moving to the next level
        //== Not currently used, but keep it for reference
        //PlayerPrefManager.SavePlayerState(score, highscore, lives);
    }

    //Level lose logic
    public void LevelLose(int levelIndex = 0)
    {
        //Set gameFinished to true (important to indicate to other classes that game is not in play state)
        gameFinished = true;

        //Show lose panel
        losePanel.SetActive(true);

        //Make sure to hide pause panel
        pauseBtn.SetActive(false);
    }

    //Enable the Exit game object when all jewels are collected
    public void EnableExit()
    {
        //Play the Exit revealed sfx
        FindObjectOfType<AudioManager>().Play("Exit");

        //Enable the animation on Exit game object
        GameObject.FindGameObjectWithTag("Exit").GetComponent<Animator>().enabled = true;

        //Enable the particle system assigned to Exit game object
        GameObject.FindGameObjectWithTag("Exit").transform.GetChild(0).gameObject.SetActive(true);
    }

    //Load main menu
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    //Load levels by levelIndex (if levelIndex=0 restart level)
    public void LoadLevel(int levelIndex = 0)
    {
        //Indicate game not yet started
        gameStarted = false;

        //Set timescale to 1 in case the game was paused
        Time.timeScale = 1f;

        //Hide panels
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        //If levelIndex=0 restart level
        if (levelIndex == 0)
            SceneManager.LoadScene(Application.loadedLevel);
        else
            SceneManager.LoadScene(levelIndex);
    }

    //Load next level in buildIndex
    public void LoadNextLevel()
    {
        //Indicate game not yet started
        gameStarted = false;

        //Set timescale to 1 in case the game was paused
        Time.timeScale = 1f;

        //Hide panels
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        //Load next level in buildIndex
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Method for playing level music
    public void PlayMusic(AudioClip ac)
    {
        //Assign the audio clip to main camera AudioSource
        Camera.main.gameObject.GetComponent<AudioSource>().clip = ac;
        Camera.main.gameObject.GetComponent<AudioSource>().loop = true;
        Camera.main.gameObject.GetComponent<AudioSource>().volume = Mathf.Lerp(Camera.main.gameObject.GetComponent<AudioSource>().volume, 1, 0.3F * Time.deltaTime);

        //If not playing, then play it
        if (!Camera.main.gameObject.GetComponent<AudioSource>().isPlaying)
            Camera.main.gameObject.GetComponent<AudioSource>().Play();
    }

    //Additional method to play sfx coming from main camera AudioSource
    public void PlaySound(string sfxName, AudioClip ac = null)
    {
        if (ac)
        {
            GetComponent<AudioSource>().clip = ac;
            GetComponent<AudioSource>().Play();
        }
    }
}