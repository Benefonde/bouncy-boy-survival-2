using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileSpawner : MonoBehaviour
{
    void Update()
    {
        if (timer <= 0)
        {
            GameObject a = Instantiate(projectile, transform.position, Quaternion.identity);
            a.SetActive(true);
            a.transform.LookAt(player);
            timer = timmer;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public GameObject projectile;
    public float timmer;
    float timer;
    public Transform player;
}
