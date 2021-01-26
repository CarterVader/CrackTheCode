using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectLang))]
public class PaintingInteractable : MonoBehaviour, IInteractable
{
    bool moving; //Booleans showing if the painting is moving or has been moved
    bool moved;
    GameObject safe; //Safe gameobject hidden behind painting

    public int GetId() //Sets Id of safe
    {
        return 0;
    }

    public void Interact(Player player, GameObject obj) //Allows painting to be interactable
    {
        safe = GameObject.FindGameObjectWithTag("Safe");
        if (obj == null) //Runs if the player is not holding an object
        {
            if (!moving && !safe.GetComponentInChildren<InputUI>().open) //Painting has to not be moving and safe must be closed
            {
                if (!moved)
                {
                    StartCoroutine(MoveToPosition(transform, transform.position + new Vector3(3, 0, 0), 1.0f)); //Slides the painting over
                    moved = true;
                }
                else
                {
                    StartCoroutine(MoveToPosition(transform, transform.position - new Vector3(3, 0, 0), 1.0f)); //Slides the painting back
                    moved = false;
                }
            }
        } else
        {
            player.Alert("[Nothing Happens]"); //Alerts that nothing happened if they use an object on the painting
        }
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove) //Moves object to the position over time
    {
        moving = true;
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        moving = false;
    }

    public string InteractText() //Text returned from interacted with
    {
        return "move";
    }
}
