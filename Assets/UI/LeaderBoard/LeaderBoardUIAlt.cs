using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderBoardUIAlt : MonoBehaviour
{
    public GameObject leaderBoardTitle;
    public GameObject scoreHolder; //Sets GameObject for score list

    void Start()
    {
        //Load the Scores
        LoadScores();
        leaderBoardTitle.GetComponent<Text>().text = "Leaderboard: " + Levels.levels[Levels.currentLevel].name;
    }

    private static int CompareScores(PlayerScore a, PlayerScore b) //Sorting algorithm for leaderboard
    {
        if (a.score > b.score)
        {
            return -1;
        }
        else if (b.score > a.score)
        {
            return 1;
        }
        else return 0;
    }

    public void BackButton()
    {
        SceneManager.LoadScene(3);
    }

    void LoadScores()
    {
        //read scores from file
        LeaderBoard.Load(Levels.currentLevel);
        List<PlayerScore> scores = LeaderBoard.scores[Levels.currentLevel];
        //Sort and display scores
        scores.Sort(CompareScores);
        for (int i = 0; i < 5; i++)
        {
            //Handles errors if score count is not enough to fill the leaderboard and sets leaderboard text
            try {
                string scoreText = scores.ToArray()[i].score.ToString();
                string playerText = scores.ToArray()[i].name;
                scoreHolder.transform.GetChild(i).GetComponent<Text>().text =  (i + 1).ToString() + ". " + playerText + ": " + scoreText;
            } catch
            {
                scoreHolder.transform.GetChild(i).GetComponent<Text>().text = "";
            }
        }
    }
}
