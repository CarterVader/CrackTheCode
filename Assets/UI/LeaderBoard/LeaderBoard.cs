using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class LeaderBoard
{
    //Array to store scores for each level
    public static List<PlayerScore>[] scores = new List<PlayerScore>[3];

    //Path to file with scores
    public static string path;

    //Current score being stored
    public static PlayerScore currentScore;

    //Current filestream
    public static FileStream file;

    //Adds new score and writes scores list to file
    public static void Save(PlayerScore score, bool overwrite, int level)
    {
        //Create leaderboard data location if it does not exist
        Directory.CreateDirectory(Application.persistentDataPath.Replace("LocalLow", "Roaming"));

        //Store level data location
        path = Application.persistentDataPath.Replace("LocalLow", "Roaming") + "/savedScoresLevel" + level + ".gd";

        //Delete levels if overwrite is true
        scores[level] = overwrite ? new List<PlayerScore>() : scores[level];

        //Store level to save in list
        scores[level].Add(score);

        //Binary formatter
        BinaryFormatter bf = new BinaryFormatter();

        //Make sure file is closed in case last save or load failed
        try
        {
            file.Close();
        }
        catch
        {

        }

        //Check is file already exists
        if (File.Exists(path) && !overwrite)
        {
            //Open file
            file = File.Open(path, FileMode.Open);
        }
        else
        {
            //Clear file and recreate
            file = File.Create(path);
        }

        //Serialize via binary formatter and save all level data in file location
        bf.Serialize(file, scores[level]);
        file.Close();
    }

    public static void Reset(int level)
    {
        //Create leaderboard data location if it does not exist
        Directory.CreateDirectory(Application.persistentDataPath.Replace("LocalLow", "Roaming"));

        //Store level data location
        path = Application.persistentDataPath.Replace("LocalLow", "Roaming") + "/savedScoresLevel" + level + ".gd";

        //Reset list
        scores[level] = new List<PlayerScore>();

        //Binary formatter
        BinaryFormatter bf = new BinaryFormatter();

        //Make sure file is closed in case last save or load failed
        try
        {
            file.Close();
        }
        catch
        {

        }

        //Create new file and serialize to it
        file = File.Create(path);
        bf.Serialize(file, scores[level]);
        file.Close();
    }

    //Loads the file for leaderboard values and allows it to be editted with the proper information
    public static void Load(int level)
    {
        //Reset score list for level
        scores[level] = new List<PlayerScore>();

        //Create leaderboard data location if it does not exist
        Directory.CreateDirectory(Application.persistentDataPath.Replace("LocalLow", "Roaming"));

        //Store level data location
        path = Application.persistentDataPath.Replace("LocalLow", "Roaming") + "/savedScoresLevel" + level + ".gd";

        //Make sure file is closed in case last save or load failed
        try
        {
            file.Close();
        }
        catch
        {

        }

        //Check if file exists
        if (File.Exists(path))
        {
            //Deserialize file to list
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(path, FileMode.Open);
            scores[level] = (List<PlayerScore>)bf.Deserialize(file);
            file.Close();
        }
    }
}
