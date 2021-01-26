#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// EDITOR SCRIPT: DO NOT INCLUDE IN BUILD
/// Loads objects from scene into folder to be recreated at runtime
/// </summary>
public class LevelBuilder : MonoBehaviour
{
    //Stores gameobjects in level
    public GameObject[] gameObjects;

    //stores objects to be added
    public List<GameObject> activeObjects;

    //Player start position
    private Vector3 startPos;
    public float x;
    public float y;
    public float z;

    //Player start location
    private Quaternion startRot;
    public float rotX;
    public float rotY;
    public float rotZ;

    //Level name
    public string levelName;

    //Clears Levels list if true
    public bool overwrite;

    //Level image
    public Sprite thumbnail;

    //Time given to complete level
    public float time;

    //Controls if level can be played
    public bool unlocked;

    void Start()
    {
        //Assign player start position and rotation
        startPos = new Vector3(x, y, z);
        startRot = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));

        //Weird fix to prevent file sharing violations
        try
        {
            Levels.Load();
        }
        catch
        {
            try
            {
                Levels.file.Close();
            }
            catch
            {

            }
        }

        //Create and save level
        Levels.Save(BuildLevelFromScene(), overwrite);
        Debug.Log(Levels.levels.ToArray().Length);
    }

    //Build level from scene
    public Level BuildLevelFromScene()
    {
        //Set start pos and rot
        Level level = new Level(startPos, startRot);
            
        //Clear folder for level objects
        if (AssetDatabase.IsValidFolder("Assets/Game/Assets/Resources/GameObjects/LevelObject/" + levelName))
        {
            FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Game/Assets/Resources/GameObjects/LevelObject/" + levelName);
            FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Game/Assets/Resources/GameObjects/LevelObject/" + levelName + ".meta");

        }
        AssetDatabase.Refresh();

        //Get all gameobjects in scene and add to active objects
        gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in gameObjects)
        {
            if (go.activeInHierarchy && go.GetComponent<LevelBuilder>() == null && go.transform.parent == null) activeObjects.Add(go);

        }

        //Create LevelObjects from activeObjects and add to level
        GameObject[] loadObjects = activeObjects.ToArray();
        for (int i = 0; i < loadObjects.Length; i++)
        {
            level.addObject(LevelBuilder.FromGameObject(loadObjects[i], i.ToString(), levelName));
        }

        //Set level settings from inspector values
        level.thumbnailLocation = AssetDatabase.GetAssetPath(thumbnail).Replace("Assets/Game/Assets/Resources/", "").Replace(".png","").Replace(".PNG", "");
        level.unlocked = unlocked;
        level.name = levelName;
        level.time = time;

        return level;
    }

    public static LevelObject FromGameObject(GameObject obj, string objName, string levelName)
    {
        if (obj != null)
        {

            string resourceLocation = "Assets/Game/Assets/Resources/GameObjects/LevelObject/" + levelName + "/" + objName + ".prefab";
            
            //Create folder from level objects
            if (!AssetDatabase.IsValidFolder("Assets/Game/Assets/Resources/GameObjects/LevelObject/" + levelName))
            {
                AssetDatabase.CreateFolder("Assets/Game/Assets/Resources/GameObjects/LevelObject", levelName);
            }

            //Clear object file if it already exists
            AssetDatabase.DeleteAsset(resourceLocation);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Create gameobject from levelobject
            GameObject gameObj = GameObject.Instantiate(obj, obj.transform.position, obj.transform.rotation);

            //Save gameobject as prefab and destroy
            PrefabUtility.SaveAsPrefabAsset(gameObj, resourceLocation);
            GameObject.Destroy(gameObj);

            //Save prefab location in levelobject
            string prefabLocation = "GameObjects/LevelObject/" + levelName + "/" + objName;
            return new LevelObject(prefabLocation, obj.transform.position, obj.transform.rotation.eulerAngles, obj.transform.localScale);
        }
        else return null;
    }
}
#endif