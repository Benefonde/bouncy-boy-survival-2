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
            if (ws.wave > 2)
            {
                int rng = Random.Range(1, 31);
                if (rng < 31)
                {
                    enemy = validEnemies[5];
                }
                if (rng < 30)
                {
                    enemy = validEnemies[4];
                }
                if (rng < 25)
                {
                    enemy = validEnemies[3];
                }
                if (rng < 21)
                {
                    enemy = validEnemies[2];
                }
                if (rng < 16)
                {
                    enemy = validEnemies[1];
                }
                if (rng < 10)
                {
                    enemy = validEnemies[0];
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
            Instantiate(specificGO[enemy.enemySpecificGameObjectId], transform).SetActive(true);
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
        if (Vector3.Distance(player.position, transform.position) <= 10 && player.GetComponent<PlayerScript>().singing)
        {
            health -= 5 * Time.deltaTime;
        }
    }

    public Transform player;
    NavMeshAgent agent;

    public EnemyScriptable enemy;

    public float health;
    public float damage;
    SpriteRenderer sr;

    public GameObject weaponPickupOnDeath;

    public int chance;

    public bool spawnedByWave = true;
    public WaveScript ws;

    public EnemyScriptable[] validEnemies;

    public GameObject[] specificGO;
}
