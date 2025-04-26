using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sensitivity.value = (PlayerPrefs.GetFloat("sensitivity", 0.8f) +  5) / 10;
    }

    public void Bye()
    {
        Application.Quit();
    }

    public void Hi(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void SetSensitivity()
    {
        PlayerPrefs.SetFloat("sensitivity", (sensitivity.value + 5) / 10);
    }
    
    public void ToggleFullscreen()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(Screen.width - 50, Screen.height - 50, false);
        }
        else
        {
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        }
    }

    public Slider sensitivity;
}
