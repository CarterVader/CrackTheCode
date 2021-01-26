using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable //Interactable object interface
{

    int GetId(); //Returns Object Id

    void Interact(Player player, GameObject obj); //Runs on interaction with player

    string InteractText(); //Text to display while hovering over interactable objects

}
