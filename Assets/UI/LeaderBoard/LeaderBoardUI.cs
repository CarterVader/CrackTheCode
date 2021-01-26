using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderBoardUI : MonoBehaviour
{
    public GameObject leaderBoardTitle;
    public GameObject scoreHolder; //Sets GameObject for score list
    public GameObject scoreBoard;
    public GameObject typeName; //Sets GameObjects for Name Input and their score w/ rank
    public GameObject myScore; //Sets GameObjects for elements in the scene that need to be interacted with
    public GameObject nameText;
    public GameObject continueButton;
    public GameObject mainMenuButton;
    public GameObject nextButton;
    public GameObject youWinScreen;
    public GameObject levelwon;
    public GameObject leaderboard;
    public string playerName; //Gives a string to store player name

    void Start()
    {
        //Opens leaderboard and allows player to interact
        Cursor.lockState = CursorLockMode.None;
        SetWinScreen();
        leaderBoardTitle.GetComponent<Text>().text = "Leaderboard: " + Levels.levels[Levels.currentLevel].name;
    }

    public void OpenScoreBoard() //Opens Scoreboard
    {
        DisplayScore();
        leaderboard.SetActive(false);
        scoreBoard.SetActive(true);
    }

    private void DisplayScore() //Display Score calculation
    {        
        scoreBoard.transform.GetChild(2).GetComponent<Text>().text = "Total Score: " + LeaderBoard.currentScore.score;
        scoreBoard.transform.GetChild(3).GetComponent<Text>().text = "Lives Score: " + LeaderBoard.currentScore.liveScore;
        scoreBoard.transform.GetChild(4).GetComponent<Text>().text = "Time Score: " + LeaderBoard.currentScore.timeScore;
        nextButton.SetActive(false);    
    }

    public void SetWinScreen() //Sets the Win Screen and gives text depending on level won
    {
        Debug.Log("ScreenSet");
        levelwon.GetComponent<Text>().text = "on beating " + Levels.levels[Levels.currentLevel].name;
    }

    public void CloseWinScreen() //Closes win screen when continue button is clicked
    {
        youWinScreen.SetActive(false);
    }

    public void OpenLeaderBoard() //Opens Leaderboard
    {
        //Load the Scores
        LoadScores();
        leaderboard.SetActive(true);
        scoreBoard.SetActive(false);
        nextButton.SetActive(myScore.activeSelf && Levels.currentLevel < Levels.levels.ToArray().Length - 1);
        mainMenuButton.SetActive(myScore.activeSelf);
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

    public void NextLevel() //Opens Next Level
    {
        Levels.currentLevel += 1;
        SceneManager.LoadScene(1);
    }

    public void EnterName() //When Name in entered and finished the player's score is shown
    {
        //Get name input
        playerName = nameText.GetComponent<Text>().text;

        //Check if name is valid
        if (!playerName.Replace(" ", "").Equals(""))
        {
            //Show score
            myScore.GetComponent<Text>().text = "Your Score: " + LeaderBoard.currentScore.score;

            //Show Buttons
            typeName.SetActive(false);
            myScore.SetActive(true);
            continueButton.SetActive(false);
            mainMenuButton.SetActive(true);
            if (Levels.currentLevel < Levels.levels.ToArray().Length - 1)
            {
                nextButton.SetActive(true);
            }

            //Save player's score to database
            LeaderBoard.currentScore.name = playerName;
            LeaderBoard.Save(LeaderBoard.currentScore, false, Levels.currentLevel);
            LoadScores();
        }
    }
}
