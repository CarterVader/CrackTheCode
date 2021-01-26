using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectLang))]
public class TestInteractable : MonoBehaviour, IInteractable //Used for early tesing purposes
{
    public int id;

    public void Interact(Player player, GameObject obj)
    {
        Debug.Log("Interacted with test");
    }

    public string InteractText()
    {
        return "interact";
    }

    public int GetId()
    {
        return id;
    }
}
