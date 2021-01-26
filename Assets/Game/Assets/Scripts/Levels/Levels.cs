using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Levels
{
    //List of loaded levels
    public static List<Level> levels = new List<Level>();

    //Level loader
    public static Game game;

    //Current filestream
    public static FileStream file;

    //Current level
    public static int currentLevel;

    //Path to level data save location
    public static string path;

    //Stores loss reason to preserve between scenes
    public static string lossReason;

    //Adds new level and writes levels list to file
    public static void Save(Level level, bool overwrite)
    {
        //Create level data location if it does not exist
        Directory.CreateDirectory(Application.persistentDataPath.Replace("LocalLow", "Roaming"));

        //Store level data location
        path = Application.persistentDataPath.Replace("LocalLow", "Roaming") + "/savedLevels.gd";

        //Delete levels if overwrite is true
        levels = overwrite ? new List<Level>() : levels;

        //Store level to save in list
        levels.Add(level);

        //Binary formatter
        BinaryFormatter bf = new BinaryFormatter();

        //Check is file already exists
        if (File.Exists(path) && !overwrite)
        {
            //Open file
            file = File.Open(path, FileMode.Open);
        } else
        {
            //Clear file and recreate
            file = File.Create(path);
        }
        //Serialize via binary formatter and save all level data in file location
        bf.Serialize(file, levels);
        file.Close();
    }

    //Writes existing levels list to file
    public static void Save()
    {
        //Create level data location if it does not exist
        Directory.CreateDirectory(Application.persistentDataPath.Replace("LocalLow", "Roaming"));

        //Store level data location
        path = Application.persistentDataPath.Replace("LocalLow", "Roaming") + "/savedLevels.gd";

        //Binary formatter
        BinaryFormatter bf = new BinaryFormatter();

        //Check is file already exists
        if (File.Exists(path))
        {
            //Open file
            file = File.Open(path, FileMode.Open);
        }
        else
        {
            //Create file
            file = File.Create(path);
        }

        //Serialize via binary formatter and save all level data in file location
        bf.Serialize(file, levels);
        file.Close();
    }

    public static void Load()
    {
        //Create level data location if it does not exist
        Directory.CreateDirectory(Application.persistentDataPath.Replace("LocalLow", "Roaming"));

        //Store level data location
        path = Application.persistentDataPath.Replace("LocalLow", "Roaming") + "/savedLevels.gd";

        //Check is file already exists
        if (File.Exists(path))
        {
            //Binary formatter
            BinaryFormatter bf = new BinaryFormatter();

            //Open file
            file = File.Open(path, FileMode.Open);

            //Load data from file into levels list
            levels = (List<Level>)bf.Deserialize(file);
            file.Close();
        }
    }
}
