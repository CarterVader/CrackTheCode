using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public void Reset() //Used to reset the levels and leaderboards
    {
        //Set all levels to locked
        foreach (Level l in Levels.levels)
        {
            l.unlocked = false;
        }
        //Unlock level one and save
        Levels.levels.ToArray()[0].unlocked = true;
        Levels.Save();

        //Delete all scores
        for (int i = 0; i < Levels.levels.ToArray().Length; i++)
        {
            LeaderBoard.Reset(i);
        }

        //Return to title screen
        SceneManager.LoadScene(0);
    }

    //Return to settings
    public void BackToSettings()
    {
        SceneManager.LoadScene(2);
    }
}
