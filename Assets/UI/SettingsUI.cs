using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    private int returnScene;
    public GameObject settingUI;
    public GameObject soundUI;

    public void ReturntoPrevious() //When button is pressed scene is returned to the previous one
    {
        SceneManager.LoadScene(0);
    }

    public void GoToSoundSettings() //When Sound Button is pressed, Sound Settings are opened
    {
        settingUI.SetActive(false);
        soundUI.SetActive(true);
    }

    public void BackToSettings() //When Back Button is pressed screen is returned to Main Settings Page
    {
        soundUI.SetActive(false);
        settingUI.SetActive(true);
    }

    public void BackToSettingsControl() //When Back is pressed goes from Controls to Settings Main Page or MainMenu depending on the location the player came from
    {
        returnScene = PlayerPrefs.GetInt("SaveScene");
        SceneManager.LoadScene(returnScene);
    }
    public void ResetScreen()
    {
        SceneManager.LoadScene(7);
    }
}
