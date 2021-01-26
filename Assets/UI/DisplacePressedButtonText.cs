using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplacePressedButtonText : MonoBehaviour
{
 
    [SerializeField] //Sets Offset Values and allows for text to be moved
    int hoveroffsetX = 0, hoveroffsetY = 10;
    [SerializeField]
    int clickoffsetX = 0, clickoffsetY = 40;
    private RectTransform textRect;
    Vector3 pos;
    
    void Start() //Sets the original Position
    {
        textRect = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        pos = textRect.localPosition;
    }
    
    public void HoverDown() //Moves Button's Text when hovered over
    {
        textRect.localPosition = new Vector3(pos.x + (float)hoveroffsetX, pos.y - (float)hoveroffsetY, pos.z);
    }
    
    public void HoverUp() //Moves Button's Text when hovered off
    {
        textRect.localPosition = pos;
    }
    
    public void ClickDown() //Moves Button's Text Position when clicked
    {
        textRect.localPosition = new Vector3(pos.x + (float)clickoffsetX, pos.y - (float)clickoffsetY, pos.z);
    }
    
    public void ClickUp() //Resets Button's Text Position after clicked
    {
        textRect.localPosition = pos;
    }
}