using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{

    public int currentLevelIndex;
    public GameObject nextBtn; //GameObjects that are interacted in the LevelSelectionScreen
    public GameObject previousBtn;
    public GameObject imageObject;
    public GameObject levelName;
    public GameObject playBtn;
    public GameObject LoadingScreen;
    public Level[] levels; //Sets an array for all the level sin the game

    void Start() //Sets up the next level and previous level buttons depending on number of buttons, and loads all the levels
    {
        //Sets the current Level Index and loads them accordingly
        currentLevelIndex = 0;
        if (levels.Length == 0)
        {
            Levels.Load();
        }
        levels = Levels.levels.ToArray();
        if (currentLevelIndex == Levels.levels.ToArray().Length - 1)
        {
            nextBtn.SetActive(false);
        }
        else
        {
            nextBtn.SetActive(true);
        }
        if (currentLevelIndex == 0)
        {
            previousBtn.SetActive(false);
        }
        else
        {
            previousBtn.SetActive(true);
        }
        Debug.Log(levels[currentLevelIndex].thumbnailLocation);
        Debug.Log(levels.Length);
        LoadLevel();
    }

    public void NextLevel() //Moves to the next level in the list and sets the play button if unlocked (won't show up if there isn't a next level)
    {
        Debug.Log("Next");
        levels = Levels.levels.ToArray();
        currentLevelIndex += 1;
        //Sets the correct buttons depending on levels before and after
        if (currentLevelIndex == Levels.levels.ToArray().Length - 1)
        {
            nextBtn.SetActive(false);
        } else
        {
            nextBtn.SetActive(true);
        }
        previousBtn.SetActive(true);
        //Runs Load level when button is clicked
        LoadLevel();
    }

    public void PreviousLevel() //Moves to the previous level in the list and sets the play button if unlocked (won't show up if there isn't a next level)
    {
        Debug.Log("Previous");
        levels = Levels.levels.ToArray();
        currentLevelIndex -= 1;
        //Sets the correct buttons depending on levels before and after
        if (currentLevelIndex == 0)
        {
            previousBtn.SetActive(false);
        }
        else
        {
            previousBtn.SetActive(true);
        }
        nextBtn.SetActive(true);
        //Runs Load level when button is clicked
        LoadLevel();
    }

    void LoadLevel() //Sets to coad determining the name of the level and if the play button should be visible or not
    {
        //Sets the thumbnail of the level from a folder with thumbnails
        imageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(levels[currentLevelIndex].thumbnailLocation);
        //Displays Level Name if level is unlocked
        if (levels[currentLevelIndex].unlocked)
        {
            levelName.GetComponent<Text>().text = "Level " + (currentLevelIndex + 1).ToString() + ": " + levels[currentLevelIndex].name;
            playBtn.SetActive(true);
        }
        else
        {
            levelName.GetComponent<Text>().text = "Level " + (currentLevelIndex + 1).ToString() + ": [Locked]";
            playBtn.SetActive(false);
        }
    }

    public void Play() //Sets the scene to the game and puts a loading screen
    {
        Debug.Log(levels[currentLevelIndex].unlocked);
        Debug.Log(currentLevelIndex);
        //Only runs if the level is unlocked
        if (Levels.levels[currentLevelIndex].unlocked)
        {
            Levels.currentLevel = currentLevelIndex;
            SceneManager.LoadScene(1);
        }
        
        LoadingScreen.SetActive(true);
    }

    //Opens leaderboard for current level
    public void OpenLeaderBoard()
    {
        SceneManager.LoadScene(8);
    }
}
