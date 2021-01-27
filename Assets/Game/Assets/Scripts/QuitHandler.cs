using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitHandler : MonoBehaviour
{
    static QuitHandler instance;

    // Start is called before the first frame update
    void Start()
    {
        //Add to DontDestroyOnLoad
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }
    }
}
