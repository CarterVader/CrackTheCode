using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore
{
    public string name; //Sets variable to be used for score calculation and display purposes
    public int score;
    public float time;
    public float lives;
    public float timeScore;
    public float liveScore;

    public PlayerScore(string name, float time, float maxTime, int lives) //Does calculations for My Score Panel
    {
        float timeRatio = time / maxTime;
        liveScore = lives * 100;
        timeScore = Mathf.RoundToInt(timeRatio * 700);
        score = Mathf.RoundToInt(timeScore + liveScore);
        this.name = name;
    }
}
