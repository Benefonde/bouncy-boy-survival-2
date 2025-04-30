using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MenuScript : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = PlayerPrefs.GetFloat(slidersValue[i], 10f);
        }
        PlayerPrefs.SetInt("endless", 0);
        if (PlayerPrefs.GetInt("healthAlt", 0) == 0)
        {
            healthThing.value = 10;
        }
        else
        {
            healthThing.value = 3;
        }
        performanceMode.isOn = true;
        if (PlayerPrefs.GetInt("performanceMode") == 0)
        {
            performanceMode.isOn = false;
        }
        if (PlayerPrefs.GetInt("wonMain") == 1)
        {
            star.SetActive(true);
        }
        endlessWaveNum.text = $"Wave {PlayerPrefs.GetInt("endlessWaves", 0)}";
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

    public void SetSlider(int sliderId)
    {
        float val = sliders[sliderId].value;
        PlayerPrefs.SetFloat(slidersValue[sliderId], val);
        if (sliderId > 0 && sliderId < 4) // volume
        {
            val /= 20;
            val = Mathf.Log10(val) * 50;
            if (sliders[sliderId].value == 0)
            {
                val = -80;
            }
            mixer.SetFloat(slidersValue[sliderId], val);
        }
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
    public void PerformanceMode()
    {
        if (performanceMode.isOn)
        {
            PlayerPrefs.SetInt("performanceMode", 1);
        }
        else
        {
            PlayerPrefs.SetInt("performanceMode", 0);
        }
    }

    public Slider[] sliders;
    public string[] slidersValue;

    public Slider healthThing;

    public GameObject help;

    public AudioMixer mixer;

    public Toggle performanceMode;

    public TMP_Text endlessWaveNum;
    public GameObject star;
}
