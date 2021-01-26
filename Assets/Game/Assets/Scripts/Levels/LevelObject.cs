using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelObject
{
    //Prefab location
    public string prefabLoc;

    //Position
    public float posX;
    public float posY;
    public float posZ;

    //Rotation
    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;

    //Scale
    public float scaleX;
    public float scaleY;
    public float scaleZ;

    public LevelObject() { }

    public LevelObject(string prefabLoc, Vector3 pos, Vector3 rot, Vector3 scale)
    {
        this.prefabLoc = prefabLoc;
        this.posX = pos.x;
        this.posY = pos.y;
        this.posZ = pos.z;
        this.rotX = rot.x;
        this.rotY = rot.y;
        this.rotZ = rot.z;
        scaleX = scale.x;
        scaleY = scale.y;
        scaleZ = scale.z;
    }

    //Get position as Vector3
    public Vector3 position()
    {
        return new Vector3(posX, posY, posZ);
    }

    //Get rotation as quaternion
    public Quaternion rotation()
    {
        return new Quaternion(rotX, rotX, rotY, rotZ);
    }

    //Get instance of prefab
    public GameObject getPrefab()
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load(prefabLoc));
        GameObject.Destroy(obj);
        return obj;
    }


    //Destroy
    public void Destroy()
    {
        GameObject.Destroy(getPrefab());
    }

    
}
