using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    void Start()
    {
        //Makes Cursor visible and unlocks it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Displays Reason Lost
        GetComponent<Text>().text = Levels.lossReason;
    }
}
