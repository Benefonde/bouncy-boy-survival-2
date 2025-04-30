using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEndScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        int waave = PlayerPrefs.GetInt("WaveNumber");
        wave.text = $"Wave {waave}";
        if (waave >= 35)
        {
            howdidyoudo.text = $"was actually really good!";
            mayan[0].SetActive(true);
        }
        if (waave >= 15 && waave < 35)
        {
            howdidyoudo.text = $"was not bad!";
            mayan[1].SetActive(true);
        }
        if (waave < 15)
        {
            howdidyoudo.text = $"wasn't very great.";
            mayan[2].SetActive(true);
        }
    }

    public void Bye()
    {
        SceneManager.LoadScene("Title");
    }

    public GameObject[] mayan;
    public TMP_Text howdidyoudo;
    public TMP_Text wave;
}
