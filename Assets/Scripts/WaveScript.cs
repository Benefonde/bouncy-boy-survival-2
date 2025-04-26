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
            if (wave >= 35)
            {
                // WIN
                return;
            }
            wave++;
            enemies += Mathf.RoundToInt(1.35f + (wave / 8));
            NewWave();
        }
        else
        {
            enemySlider.value = FindObjectsOfType<EnemyScript>().Length;
        }
    }

    void NewWave()
    {
        switch (Mathf.RoundToInt(Random.Range(1, 5.5f)))
        {
            case 1: player.maxHp += 2; break;
            case 2: player.mainSpeed++; break;
            case 3: player.regen += 0.2f; break;
            case 4: player.weaponDamageBonus++; break;
            case 5: player.weaponDurabilityBonus += 2; break;
        }
        waveTxt.text = $"Wave {wave}";
        for (int i = 0; i < enemies; i++)
        {
            Instantiate(enemy).SetActive(true);
        }
        enemySlider.maxValue = enemies;
        waveInProgress = true;
    }

    public int wave;
    [SerializeField]
    float enemies;
    public GameObject enemy;

    public TMP_Text waveTxt;
    public Slider enemySlider;

    bool waveInProgress;

    public PlayerScript player;
}