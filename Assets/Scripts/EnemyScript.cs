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
            agent.Warp(new Vector3(Random.Range(-135, 135), 5, Random.Range(-135, 135))); // hoping this drops the enemy down to floor height (IT DOES)
            if (ws.wave > 2)
            {
                int rng = Random.Range(1, 31);
                if (rng < 31 && ws.wave > 9)
                {
                    enemy = validEnemies[5];
                }
                if (rng < 30 && ws.wave > 7)
                {
                    enemy = validEnemies[4];
                }
                if (rng < 25 && ws.wave > 4)
                {
                    enemy = validEnemies[3];
                }
                if (rng < 21 && ws.wave > 3)
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
            if (enemy.artifactDrop != null)
            {
                if (Random.Range(1, chance * 12) == 1)
                {
                    GameObject wd = Instantiate(artifactPickupOnDeath, transform.position, Quaternion.identity);
                    if (wd.GetComponent<ArtifactPickup>() != null)
                    {
                        wd.GetComponent<ArtifactPickup>().me = enemy.artifactDrop;
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
        health += enemy.regen * Time.deltaTime;
        if (Vector3.Distance(player.position, transform.position) <= 20 && player.GetComponent<PlayerScript>().singing)
        {
            health -= (10 + enemy.regen) * Time.deltaTime;
        }
    }

    public IEnumerator Fire(float time)
    {
        float timmer = time;
        fire.SetActive(true);
        while (timmer > 0)
        {
            timmer -= Time.deltaTime;
            health -= 4 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (health > 0)
        {
            fire.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("Cobweb") && enemy.name != "Spidey Boy")
        {
            agent.speed /= 1.5f;
        }
    }

    public Transform player;
    NavMeshAgent agent;

    public EnemyScriptable enemy;

    public float health;
    public float damage;
    SpriteRenderer sr;

    public GameObject weaponPickupOnDeath;
    public GameObject artifactPickupOnDeath;

    public int chance;

    public bool spawnedByWave = true;
    public WaveScript ws;

    public EnemyScriptable[] validEnemies;

    public GameObject[] specificGO;

    public GameObject fire;
}
