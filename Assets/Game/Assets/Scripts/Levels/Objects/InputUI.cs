using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectLang))]
public class InputUI : MonoBehaviour, IInteractable
{
    public int id;
    public GameObject textObject; //Sets gameobject for text object and input object
    public GameObject inputObject;
    public string text = ""; //Sets Safe input to nothing when opened and resets text when closes
    private Player player; //Gets player object from Player class
    public string target;
    public GameObject hinge; //Adds an invisible Gameobject from safe door to rotate around
    public bool locked = true; //Boolean if it is locked
    public bool open; //Boolean if it is open
    private bool opening; //Boolean if its in opening animation
    public float startRot; //Float for the starting rotation of object
    new AudioSource audio; //Adds audio source and clips for unlock, open, close
    AudioClip unlockClip;
    AudioClip closeClip;
    AudioClip openClip;

    public int GetId() //Returns Id of the safe
    {
        return id;
    }

    public void Interact(Player player, GameObject obj) //Sets actions for interacting with safe door
    {
        audio = GetComponent<AudioSource>(); //Sets all the variables with audio clips, textobjects, or gameobjects
        audio.enabled = true; //Enables audio source
        unlockClip = Resources.Load<AudioClip>("Sounds/Door_Unlock"); //Loads all the audio clips from the resource folders
        closeClip = Resources.Load<AudioClip>("Sounds/Safe_Close");
        openClip = Resources.Load<AudioClip>("Sounds/Safe_Open");
        this.player = player; //Gets player object
        textObject = player.transform.GetChild(2).GetChild(6).GetChild(1).GetChild(0).gameObject; //Gets text object from player prefab
        inputObject = player.transform.GetChild(2).GetChild(6).gameObject; //Gets input UI
        hinge = transform.GetChild(1).gameObject; //Gets invisible hinge Object
        textObject.GetComponent<Text>().text = text;
        if (locked)
        {
            player.activeInput = this; //If locked open Input UI
            player.Pause();
            inputObject.SetActive(true);
        } else
        {
            if (!opening) //If not opening
            {
                if (open) //Closes the door if open
                {
                    CloseDoor();
                }
                else //Opens door if open
                {
                    OpenDoor();
                }
            }
        }
        
    }

    public void Input(char c) //Safe input
    {
        if (text.Length < target.Length)
        {
            text += c;
            textObject.GetComponent<Text>().text = text;
        }
    }

    public void Submit() //Sets commands for Submit button
    {
        if (text == target) //If the code is correct
        {
            Debug.Log("Correct"); //Unlocks the safe and plays unlock sound
            Close();
            locked = false;
            audio.PlayOneShot(unlockClip);
            player.Alert("[Unlocked]");
        } else
        {
            Debug.Log("Incorrect"); //If incorrect, life is lost and the text is reset
            player.LoseLife();
            text = "";
            textObject.GetComponent<Text>().text = text;
        }
    }

    public void Close() //Closes the Input UI and resets the current code written
    {
        text = "";
        textObject.GetComponent<Text>().text = text;
        player.activeInput = null;
        player.Pause();
        inputObject.SetActive(false);
    }

    public void BackSpace() //Removes one character when the backspace button is clicked
    {
        if (text.Length > 0)
        {
            text = text.Substring(0, text.Length - 1);

            textObject.GetComponent<Text>().text = text;
        }
    }

    public void OpenDoor() //Opens safe door with animation and plays audio clip
    {
        audio.PlayOneShot(openClip);
        StartCoroutine(RotateOverTime(transform, hinge.transform, 90, startRot + 90));
        open = true;
    }
    public void CloseDoor() //Closes safe door with animation and plays close audio clip
    {
        audio.PlayOneShot(closeClip);
        StartCoroutine(RotateOverTime(transform, hinge.transform, -90, startRot));
        open = false;
    }

    public IEnumerator RotateOverTime(Transform transform, Transform target, float speed, float targetRot) //Sets the IEnumerator for closing the door with an animation
    {
        Vector3 initialRot = transform.rotation.eulerAngles;
        opening = true;
        if (transform.rotation.eulerAngles.y < targetRot)
        {
            while (transform.rotation.eulerAngles.y < targetRot && transform.rotation.eulerAngles.y >= initialRot.y)
            {
                transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
                yield return null;
            }
        } else
        {
            while (transform.rotation.eulerAngles.y > targetRot && transform.rotation.eulerAngles.y <= initialRot.y)
            {
                transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
                yield return null;
            }
        }
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(initialRot.x, targetRot, initialRot.z));
        opening = false;
    }

    public string InteractText() //Returns Interactable text depending on state of the safe
    {
        if (locked)
        {
            return "input code"; //Asks to input code if locked
        }
        else
        {
            if (open) //Closes if open
            {
                return "close";
            }
            else
            {
                return "open"; //Opens if closes
            }
        }
        
    }
}
