using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(ObjectLang))]
public class Holdable : MonoBehaviour, IInteractable
{
    public int id; //Sets id for holdable
    public bool held = false; //Boolean changes if the object is being held or not
    public float maxSpeed = 100f; //Max speed of the object
    public Player player; //Gets player object from Player class
    public bool touchingPlayer; //Boolean if it is touching the player

    void FixedUpdate() //Physics Update
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity; //Limits Velocity for collision bugs
        if (velocity.magnitude > maxSpeed)
        {
            GetComponent<Rigidbody>().velocity = velocity.normalized * maxSpeed;         
        }

        //Prevent from falling through floor
        if (transform.position.y < 0 && GetComponent<Rigidbody>().velocity.y < 0f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        //Drop if too far from player
        if (held && (transform.position - player.transform.position).sqrMagnitude > 20)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.DropHeldObject();
        }
    }

    public void Interact(Player player, GameObject obj)
    {
        if (obj == null)
        {
            GetComponent<Holdable>().enabled = true; //Enables this script
            player.SetHeldObject(gameObject); //Picks up the object 
            gameObject.layer = 8;
            held = true;
            GetComponent<Rigidbody>().useGravity = false; //Makes it so object stays in front of player
            GetComponent<Rigidbody>().freezeRotation = true; //Freezes rotation of the object
            this.player = player;
        }
    }

    public void Drop() //When player drops the object by clicking e
    {
        gameObject.layer = 0;
        held = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().freezeRotation = false;
    }

    public string InteractText() //Displays interact text when hovering over object
    {
        return "pick up " + GetComponent<ObjectLang>().name;
    }

    public int GetId() //Gets Id of object
    {
        return id;
    }
}

