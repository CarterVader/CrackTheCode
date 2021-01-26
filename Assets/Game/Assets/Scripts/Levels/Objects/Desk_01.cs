using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectLang), typeof(AudioSource))]
public class Desk_01 : MonoBehaviour, IInteractable
{
    int id;
    public int keyId; //Assigns Id to different keys
    bool open = false; //Sets booleans for different states of desk drawer
    bool opening = false;
    bool unlocked = false;
    public float x; //Floats for positions of the drawer
    public float y;
    public float z;
    new AudioSource audio; //Audio information
    AudioClip openClip;
    AudioClip closeClip;
    AudioClip unlockClip;

    public int GetId() //Gets the ID of the object
    {
        return id;
    }

    public void Interact(Player player, GameObject obj) //Sets Interaction between key and object depending on the keyId and other factors
    {
        audio = GetComponent<AudioSource>(); //Sets audio clips to different actions of drawer
        audio.enabled = true;
        openClip = Resources.Load<AudioClip>("Sounds/Drawer_Open");
        closeClip = Resources.Load<AudioClip>("Sounds/Drawer_Close");
        unlockClip = Resources.Load<AudioClip>("Sounds/Door_Unlock");

        if (obj == null)
        {
            if (!opening && unlocked)
            {
                if (!open) //Opens the drawer with animation and plays opening drawer audio clip
                {
                    StartCoroutine(MoveToPosition(transform, transform.position + new Vector3(x, y, z), 1f));
                    audio.PlayOneShot(openClip);
                    open = true;
                }
                else //Closes the drawer with animation and plays closing drawer audio clip
                {
                    StartCoroutine(MoveToPosition(transform, transform.position - new Vector3(x, y, z), 1f));
                    audio.PlayOneShot(closeClip);
                    open = false;
                }
            }
            else if (!unlocked) //Alerts player that the drawer is locked
            {
                player.Alert("[Locked]");
            }
        } else
        {
            if (obj.GetComponent<Holdable>().GetId() == keyId)
            {
                //Consumes key on unlock
                unlocked = true;
                audio.PlayOneShot(unlockClip);
                Destroy(obj);
                player.DropHeldObject();
                player.Alert("[Unlocked]");
            }
            else
            {
                player.Alert("[Locked]");
            }
        }
    }

    public string InteractText()
    {
        //Displays if unlocked
        if (unlocked)
        {
            if (open) return "close";
            else return "open";
        }
        else return "unlock";
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove) //Opens desk over time
    {
        opening = true;
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        opening = false;
    }
}
