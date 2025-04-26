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
    }

    void Update()
    {
        agent.SetDestination(player.position);
    }

    public Transform player;
    NavMeshAgent agent;

    public EnemyScriptable enemy;

    public int health;
    public float damage;
    SpriteRenderer sr;
}
