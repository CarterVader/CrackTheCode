using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectLang))]
public class Readable : MonoBehaviour, IInteractable
{
    public int id; //Sets Variables to be Used in Unity Editor
    public Sprite image;
    public GameObject imageObject;
    public GameObject textObject;
    public int fontsize = 111;
    [TextArea] public string text; //Allows different texts to be entered depending on the situation
    private Player player; //Gives Access to the player object in the player class

    public int GetId() //Get ID of the object
    {
        return id;
    }

    public void Interact(Player player, GameObject obj) //Sets up the interaction with the object and what happens when it is clicked on
    {

        imageObject = player.transform.GetChild(2).GetChild(4).GetChild(0).gameObject;
        textObject = player.transform.GetChild(2).GetChild(4).GetChild(0).GetChild(0).gameObject;

        if (image == null)
        {
            image = Resources.Load<Sprite>("Graphics/Sprites/Paper");
        }

        imageObject.GetComponent<AspectRatioFitter>().aspectRatio = (float)image.texture.width / (float)image.texture.height; //imageObject and textObject are the elements in the readable that are edited here
        
        textObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

        imageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(image.texture.width, image.texture.height);
        
        
        imageObject.GetComponent<Image>().sprite = image;
        textObject.GetComponent<Text>().text = text;
        textObject.GetComponent<Text>().fontSize = fontsize;
        imageObject.SetActive(true);
        this.player = player;
        this.player.Pause();
    }

    public string InteractText() //What is returned when readable object is clicked on
    {
        return "read";
    }
}
