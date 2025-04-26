using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveScript : MonoBehaviour
{
    void Start()
    {
        wave = 1;
        enemies = 3;
        waveInProgress = false;
        Invoke(nameof(NewWave), 5);
    }

    void Update()
    {
        if (FindObjectsOfType<EnemyScript>().Length <= 0 && waveInProgress)
        {
            waveInProgress = false;
            if (wave >= 20)
            {
                // WIN
                return;
            }
            wave++;
            enemies += 2;
            NewWave();
        }
        else
        {
            enemySlider.value = FindObjectsOfType<EnemyScript>().Length;
        }
    }

    void NewWave()
    {
        waveTxt.text = $"Wave {wave}";
        for (int i = 0; i < enemies; i++)
        {
            Instantiate(enemy).SetActive(true);
        }
        enemySlider.maxValue = enemies;
        waveInProgress = true;
    }

    public int wave;
    float enemies;
    public GameObject enemy;

    public TMP_Text waveTxt;
    public Slider enemySlider;

    bool waveInProgress;
}