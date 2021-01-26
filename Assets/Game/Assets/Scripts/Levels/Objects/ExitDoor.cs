using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectLang))]
public class ExitDoor : MonoBehaviour, IInteractable
{
    Player player; //Gets player object from player class
    public int keyId; //Sets KeyId to open door

    public int GetId() //Sets Id of door
    {
        return 0;
    }

    public void Interact(Player player, GameObject obj) //Allows the player to interact with the door
    {
        this.player = player;
        if (obj != null && obj.GetComponent<Holdable>().id == keyId) //Level is won and goes to win screen
        {
            StartCoroutine(ExitCoroutine());
        } else
        {
            player.Alert("[Locked]"); //Door is locked and alerts player
        }
    }

    IEnumerator ExitCoroutine() //Exit Coroutine that fades out the level and goes to the win screen
    {
        GetComponent<AudioSource>().enabled = true;
        GetComponent<AudioSource>().Play();
        player.Pause();
        Image image = player.transform.GetChild(2).GetChild(8).GetComponent<Image>();
        image.gameObject.SetActive(true);
        image.CrossFadeAlpha(0.0f, 0.0f, false);
        image.CrossFadeAlpha(1.0f, 1.0f, false);
        yield return new WaitForSeconds(1);
        PlayerScore score = new PlayerScore("Player", player.timer, Levels.levels[Levels.currentLevel].time, player.lives);
        LeaderBoard.currentScore = score;       
        if (Levels.currentLevel < Levels.levels.ToArray().Length - 1) Levels.levels[Levels.currentLevel + 1].unlocked = true;
        Levels.Save();
        Debug.Log(Levels.currentLevel + 1);
        SceneManager.LoadScene(4);
    }

    public string InteractText() //Returns Unlocked when the door is unlocked
    {
        return "unlock";
    }
}
