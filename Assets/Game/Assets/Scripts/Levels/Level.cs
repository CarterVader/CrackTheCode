using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    //List of Objects in level
    List<LevelObject> objects;
 
    //store pos and rot as float bc vec3 and quaternions are not serializable

    //Player start position
    public float startPosX;
    public float startPosY;
    public float startPosZ;

    //Player start rotation
    public float startRotX;
    public float startRotY;
    public float startRotZ;

    //Level picture location
    public string thumbnailLocation;

    //Controls if level can be played
    public bool unlocked;

    public string name;

    //Time given to complete level
    public float time;

    //returns list of objects in level
    public List<LevelObject> GetObjects()
    {
        return objects;
    }

    //add object to level
    public void addObject(LevelObject obj)
    {
        objects.Add(obj);
    }

    public Level(Vector3 startPos, Quaternion startRot)
    {
        objects = new List<LevelObject>();
        this.startPosX = startPos.x;
        this.startPosY = startPos.y;
        this.startPosZ = startPos.z;
        this.startRotX = startRot.eulerAngles.x;
        this.startRotY = startRot.eulerAngles.y;
        this.startRotZ = startRot.eulerAngles.z;
    }

    //Return player start pos as Vector3
    public Vector3 GetStartPos()
    {
        return new Vector3(startPosX, startPosY, startPosZ);
    }

    //Return player start rotation as Quaternion
    public Quaternion GetStartRot()
    {
        return Quaternion.Euler(new Vector3(startRotX, startRotY, startRotZ));
    }
}
