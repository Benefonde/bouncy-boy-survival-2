using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileSpawner : MonoBehaviour
{
    void Update()
    {
        if (timer <= 0)
        {
            Instantiate(projectile, transform.position, transform.rotation);
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
}
