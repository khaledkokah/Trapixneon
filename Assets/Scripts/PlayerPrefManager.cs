using UnityEngine;
using System.Collections;

//== The PlayerPrefManager is an organized class for read/write player preferences, originally from Coursera MSU course
//== Not all methods are utilized in this game, but these functions will be used in the future updates
public static class PlayerPrefManager
{
    //Read the score
    public static int GetScore()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            return PlayerPrefs.GetInt("Score");
        }
        else
        {
            return 0;
        }
    }

    public static void SetScore(int score)
    {
        PlayerPrefs.SetInt("Score", score);
    }

    public static int GetHighscore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            return PlayerPrefs.GetInt("Highscore");
        }
        else
        {
            return 0;
        }
    }

    public static void SetHighscore(int highscore)
    {
        PlayerPrefs.SetInt("Highscore", highscore);
    }


    // story the current player state info into PlayerPrefs
    public static void SavePlayerState(int score, int highScore, int lives)
    {
        // save currentscore and lives to PlayerPrefs for moving to next level
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.SetInt("Highscore", highScore);
    }

    // reset stored player state and variables back to defaults
    public static void ResetPlayerState(int startLives, bool resetHighscore)
    {
        Debug.Log("Player State reset.");
        PlayerPrefs.SetInt("Lives", startLives);
        PlayerPrefs.SetInt("Score", 0);

        if (resetHighscore)
            PlayerPrefs.SetInt("Highscore", 0);
    }

    // store a key for the name of the current level to indicate it is unlocked
    public static void UnlockLevel()
    {
        PlayerPrefs.SetInt(Application.loadedLevelName, 1);
    }

    // determine if a levelname is currently unlocked (i.e., it has a key set)
    public static bool LevelIsUnlocked(string levelName)
    {
        return (PlayerPrefs.HasKey(levelName));
    }

    // output the defined Player Prefs to the console
    public static void ShowPlayerPrefs()
    {
        // store the PlayerPref keys to output to the console
        string[] values = { "Score", "Highscore", "Lives" };

        // loop over the values and output to the console
        foreach (string value in values)
        {
            if (PlayerPrefs.HasKey(value))
            {
                Debug.Log(value + " = " + PlayerPrefs.GetInt(value));
            }
            else
            {
                Debug.Log(value + " is not set.");
            }
        }
    }
}
