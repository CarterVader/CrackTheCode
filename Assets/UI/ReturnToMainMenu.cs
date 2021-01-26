using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    // Goes back to Main Menu
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void ReturnLevelSelect()
    {
        SceneManager.LoadScene(3);
    }
}
