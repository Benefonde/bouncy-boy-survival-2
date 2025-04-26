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
        sr.sprite = enemy.sprite;
        health = enemy.health;
        damage = enemy.damage;
        agent.speed = enemy.speed;
        chance = enemy.chanceOfDrop;
        if (enemy.enemySpecificGameObject != null)
        {
            Instantiate(enemy.enemySpecificGameObject, transform);
        }
        agent.Warp(new Vector3(Random.Range(-135, 135), 5, Random.Range(-135, 135))); // hoping this drops the enemy down to floor height
    }

    void Update()
    {
        if (health <= 0)
        {
            if (weaponPickupOnDeath != null)
            {
                if (Random.Range(1, chance) == 1)
                {
                    Instantiate(weaponPickupOnDeath, transform.position, Quaternion.identity).SetActive(true);
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
}
