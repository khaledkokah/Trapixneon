using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//=== MenuManager class contains all the methods for menu scene ==
public class MenuManager : MonoBehaviour {

    //Level UI menu
    public GameObject LevelsMenu;

    //Array of level worlds (defined as WorldTemplate object in scene)
    public GameObject[] levelWorlds;

    //Array of level names (should match level scene name)
    [Header("Level names (12)")]
    [Tooltip("Insert 12 names according to the level select image place holders, if you want to change this please change level select image")]
    public string[] levelNames = new string[11];

    //Level button prefab
    public GameObject levelUIButton;

    private void Awake()
    {
        //Level select menu
        SetupLevelSelect();
    }

    //Loop through all the LevelButtons and set them to interactable 
    //Based on if PlayerPref key is set for the level
    void SetupLevelSelect()
    {
        //Loop through each level world defined in the editor 
        for (int w = 0; w < levelWorlds.Length; w++)
        {
            //Loop through level names defined in the editor
            for (int i = 0; i < levelNames.Length; i++)
            {
                //Get the level name and append it to world number
                //Level name will be "Level_1_1, Level_1_2, .. etc"
                string levelname = "Level_" + (w+1).ToString() + "_"+ levelNames[i];

                //Level display name that will be written on gui buttons
                string levelDisplayName = levelNames[i];

                //Create a button from the template
                GameObject levelButton = Instantiate(levelUIButton, Vector3.zero, Quaternion.identity) as GameObject;

                //Button name in scene 
                levelButton.name = levelname + "Btn";

                //Set the parent of button as the LevelsPanel so it will be dynamically arrange based on the defined layout
                levelButton.transform.SetParent(levelWorlds[w].transform.GetChild(1).transform, false);

                //Get the Button script attached to the button
                Button levelButtonScript = levelButton.GetComponent<Button>();

                //Setup the listener to loadlevel when clicked
                levelButtonScript.onClick.RemoveAllListeners();
                levelButtonScript.onClick.AddListener(() => FindObjectOfType<AudioManager>().Play("Click"));
                levelButtonScript.onClick.AddListener(() => LoadLevel(levelname));

                //Set the label of the button
                Text levelButtonLabel = levelButton.GetComponentInChildren<Text>();
                levelButtonLabel.text = levelDisplayName;

                //Should be interactable based on if the level is unlocked?
                if (PlayerPrefManager.LevelIsUnlocked(levelname))
                    levelButtonScript.interactable = true;
                else
                    levelButtonScript.interactable = false;
            }
        }
    }

    //Load scene by name 
    public void LoadLevel(string levelName)
    {
        //Load the specified level
        SceneManager.LoadScene(levelName);
    }
}
