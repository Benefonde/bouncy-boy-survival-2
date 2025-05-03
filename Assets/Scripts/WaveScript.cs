using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class WaveScript : MonoBehaviour
{
    void Start()
    {
        wave = 1;
        enemies = 3;
        waveInProgress = false;
        Invoke(nameof(NewWave), 5);
        aud = GetComponent<AudioSource>();
        if (Environment.GetCommandLineArgs().ToList().Contains("-bleh"))
        {
            for (int i = 0; i < goodMusic.Length; i++)
            {
                waveMusic[i] = goodMusic[i];
            }
        }
    }

    void Update()
    {
        if (FindObjectsOfType<EnemyScript>().Length <= 0 && waveInProgress)
        {
            waveInProgress = false;
            if (wave >= 35 && PlayerPrefs.GetInt("endless") == 0)
            {
                DontDestroyOnLoad(player.gameObject);
                PlayerPrefs.SetInt("WaveNumber", wave);
                PlayerPrefs.SetInt("wonMain", 1);
                SceneManager.LoadSceneAsync("Ending");
                return;
            }
            wave++;
            enemies += Mathf.RoundToInt(1.55f + (wave / 8));
            NewWave();
        }
        else
        {
            enemySlider.value = FindObjectsOfType<EnemyScript>().Length;
        }

        if (Input.GetKey(KeyCode.KeypadPlus) && Input.GetKeyDown(KeyCode.Quote))
        {
            for (int i = 0; i < FindObjectsOfType<EnemyScript>().Length; i++)
            {
                Destroy(FindObjectsOfType<EnemyScript>()[i]);
            }
            wave++;
            enemies += Mathf.RoundToInt(1.35f + (wave / 6) + 1);
            NewWave();
        }

        if (player.hp <= 0)
        {
            aud.mute = true;
        }
        else
        {
            aud.mute = false;
        }
        aud.volume = 0.6f - (player.singing * 0.4f);
    }

    void NewWave()
    {
        switch (Mathf.RoundToInt(UnityEngine.Random.Range(1, 5.5f)))
        {
            case 1: player.maxHp += 2; player.hp += 2; break;
            case 2: player.mainSpeed++; break;
            case 3: player.regen += 0.2f; break;
            case 4: player.weaponDamageBonus++; break;
            case 5: player.weaponDurabilityBonus += 2; break;
        }
        waveTxt.text = $"Wave {wave}"; 
        if (wave >= 35 && PlayerPrefs.GetInt("endless") == 0)
        {
            DontDestroyOnLoad(player.gameObject);
            PlayerPrefs.SetInt("WaveNumber", wave);
            PlayerPrefs.SetInt("wonMain", 1);
            if (SceneManager.sceneCount == 1)
            {
                SceneManager.LoadScene("Ending");
            }
            return;
        }
        /*if (wave == 15 || wave == 35)
        {
            Instantiate(enemy).SetActive(true); //boss! !!
        }
        else*/
        {
            for (int i = 0; i < enemies; i++)
            {
                Instantiate(enemy).SetActive(true);
            }
        }
        enemySlider.maxValue = enemies;
        waveInProgress = true;
        if (wave < 10)
        {
            if (aud.clip != waveMusic[0])
            {
                aud.Stop();
                aud.clip = waveMusic[0];
                aud.Play();
            }
        }
        else if (wave >= 10 && wave < 25)
        {
            if (aud.clip != waveMusic[1])
            {
                aud.Stop();
                aud.clip = waveMusic[1];
                aud.Play();
            }
        }
        else if (wave >= 25)
        {
            if (aud.clip != waveMusic[2])
            {
                aud.Stop();
                aud.clip = waveMusic[2];
                aud.Play();
            }
        }
    }

    public int wave;
    [SerializeField]
    float enemies;
    public GameObject enemy;

    public TMP_Text waveTxt;
    public Slider enemySlider;

    bool waveInProgress;

    public PlayerScript player;

    AudioSource aud;
    public AudioClip[] waveMusic;
    public AudioClip[] goodMusic;
}