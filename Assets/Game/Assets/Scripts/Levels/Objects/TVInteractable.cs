using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVInteractable : MonoBehaviour, IInteractable
{

    public Material screen; //Sets material for screen and display to be changed in code
    public Material display;
    public bool displayActive; //Boolean if the display is active or not

    public int GetId() //Returns Id for object
    {
        return 0;
    }

    public void Interact(Player player, GameObject obj) //Interacting with object
    {
        GetComponent<AudioSource>().enabled = true; //Plays audio source if display is active
        if (obj == null)
        {
            if (displayActive)
            {
                GetComponent<AudioSource>().Stop(); //If tv off, audio source off and tv material is black
                GetComponent<MeshRenderer>().material = screen;
                displayActive = false;
            } else
            {
                GetComponent<AudioSource>().Play(); //If tv on, audio source on and tv material is picture
                GetComponent<MeshRenderer>().material = display;
                displayActive = true;
            }
        }
    }

    public string InteractText() //Returns text based on if the tv is turned on or off
    {
        if (displayActive)
        {
            return "turn off";
        } else
        {
            return "turn on";
        }
    }
}