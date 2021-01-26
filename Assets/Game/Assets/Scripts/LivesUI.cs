using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    private Player player;
    private void Start() //Gets a component when game is started
    {
        player = transform.parent.GetComponentInParent<Player>();
    }
    public void UpdateLives(int lives) //Lowers the lives and alerts the player when live is lost due to mistake
    {
        for (int i = 0; i < 4; i++)
        {
            //Send alert to player
            transform.GetChild(i).gameObject.SetActive(false);
            player.Alert("[-1]");

        }
        transform.GetChild(lives).gameObject.SetActive(true);
    }
}
