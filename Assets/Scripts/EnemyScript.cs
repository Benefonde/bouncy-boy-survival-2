using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sr = GetComponent<SpriteRenderer>();
        if (spawnedByWave)
        {
            agent.Warp(new Vector3(Random.Range(-135, 135), 5, Random.Range(-135, 135))); // hoping this drops the enemy down to floor height
            if (ws.wave > 3)
            {
                int rng = Random.Range(1, 31);
                if (rng < 11)
                {
                    enemy = validEnemies[0]; // spiky
                }
                if (rng > 11 && rng < 16)
                {
                    enemy = validEnemies[rng - 11]; // random enemies
                }
                if (rng >= 16 && rng < 26)
                {
                    enemy = validEnemies[Random.Range(0, validEnemies.Length)]; // same as before
                }
                if (rng > 26)
                {
                    enemy = validEnemies[rng - 25];
                }
            }
        }
        sr.sprite = enemy.sprite;
        health = enemy.health;
        damage = enemy.damage;
        agent.speed = enemy.speed;
        chance = enemy.chanceOfDrop;
        if (enemy.enemySpecificGameObjectId != -1)
        {
            Instantiate(specificGO[enemy.enemySpecificGameObjectId], transform);
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            if (enemy.weaponDrop != null)
            {
                if (Random.Range(1, chance) == 1)
                {
                    GameObject wd = Instantiate(weaponPickupOnDeath, transform.position, Quaternion.identity);
                    if (wd.GetComponent<WeaponPickup>() != null)
                    {
                        wd.GetComponent<WeaponPickup>().me = enemy.weaponDrop;
                    }
                    wd.SetActive(true);
                }
            }
            Destroy(gameObject);
        }
        else
        {
            agent.enabled = true;
            agent.SetDestination(player.position);
        }
    }

    public Transform player;
    NavMeshAgent agent;

    public EnemyScriptable enemy;

    public int health;
    public float damage;
    SpriteRenderer sr;

    public GameObject weaponPickupOnDeath;

    public int chance;

    public bool spawnedByWave = true;
    public WaveScript ws;

    public EnemyScriptable[] validEnemies;

    public GameObject[] specificGO;
}
