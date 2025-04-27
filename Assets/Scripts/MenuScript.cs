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
        sensitivity.value = PlayerPrefs.GetFloat("sensitivity", 10f);
        PlayerPrefs.SetInt("endless", 0);
        if (PlayerPrefs.GetInt("healthAlt", 0) == 0)
        {
            healthThing.value = 10;
        }
        else
        {
            healthThing.value = 3;
        }
    }

    void Update()
    {
        if (help != null)
        {
            if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.H))
            {
                help.SetActive(!help.activeSelf);
            }
        }
    }

    public void Bye()
    {
        Application.Quit();
    }

    public void Hi(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void HiEndless()
    {
        PlayerPrefs.SetInt("endless", 1);
    }

    public void SetSensitivity()
    {
        PlayerPrefs.SetFloat("sensitivity", (sensitivity.value));
    }
    public void SetHealthAppearance()
    {
        if (PlayerPrefs.GetInt("healthAlt", 0) == 1)
        {
            PlayerPrefs.SetInt("healthAlt", 0);
            healthThing.value = 10;
        }
        else
        {
            PlayerPrefs.SetInt("healthAlt", 1);
            healthThing.value = 3;
        }
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
    public Slider healthThing;

    public GameObject help;
}
