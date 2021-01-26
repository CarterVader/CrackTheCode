using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //Active objects in current level
    public List<LevelObject> activeObjects;

    //Index of current level
    public int currentLevelIndex;

    //Player object to be loaded
    public static UnityEngine.Object playerObject;
    public static GameObject player;

    //Is game running
    public bool running;
    

    void Start()
    {
        StartGame(Levels.currentLevel);
    }

    public void StartGame(int levelIndex)
    {
        Debug.Log(Application.dataPath);
        //Load player from resources
        playerObject = Resources.Load("Player/Player");

        //Generate Level
        Levels.Load();
        LoadLevel(levelIndex);

        //Load level into scene
        Render();

        //Turn on lives display
        player.transform.GetChild(2).GetChild(9).gameObject.SetActive(true);

        //start running the game
        running = true;

        Debug.Log(Levels.levels.Count);
        //Fade out
        StartCoroutine(LoadCoroutine());
       
    }

    IEnumerator LoadCoroutine()
    {
        //Fade out into level
        player.transform.GetChild(2).GetChild(8).gameObject.SetActive(true);
        Image image = player.transform.GetChild(2).GetChild(8).GetComponent<Image>();
        image.CrossFadeAlpha(0.0f, 1.0f, false);
        yield return new WaitForSeconds(1);
        image.gameObject.SetActive(false);
    }

    //Pause
    public void PauseGame()
    {
        running = false;
    }

    //Unpause
    public void UnPauseGame()
    {
        running = true;
    }

    //Get level from Levels class
    public void LoadLevel(int index)
    {
        Level level = Levels.levels.ToArray()[index];
        activeObjects = level.GetObjects();
        player = (GameObject)GameObject.Instantiate(playerObject, level.GetStartPos(), level.GetStartRot());
        player.transform.SetParent(gameObject.transform);
        currentLevelIndex = index;

        //Start game
        player.GetComponent<Player>().StartGame(currentLevelIndex);

    }

    //Generate level objects
    public void Render()
    {
        //create each object from current level
        foreach (LevelObject obj in activeObjects)
        {
            if (obj != null)
            {
                //generate object
                GameObject gobj = Instantiate(obj.getPrefab(), Vector3.zero, Quaternion.identity);
                gobj.transform.SetParent(transform);
                gobj.transform.position = obj.position();
                gobj.transform.rotation = obj.getPrefab().transform.rotation;

                //turn on lights
                if (gobj.GetComponentInChildren<Light>() != null)
                {
                    gobj.GetComponentInChildren<Light>().enabled = true;
                }
            }
        }
    }
}
