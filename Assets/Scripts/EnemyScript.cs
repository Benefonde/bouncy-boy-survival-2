using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sr = GetComponent<SpriteRenderer>();
        if (spawnedByWave)
        {
            /*if (ws.wave == 15 || ws.wave == 35) 
            {
                agent.Warp(new Vector3(0, 5, 120));
                switch (ws.wave)
                {
                    case 15: enemy = validBosses[0]; break;
                    case 35: enemy = validBosses[1]; break;
                }
                bossStuffs[0].SetActive(true); // text
                bossStuffs[0].GetComponent<TMP_Text>().text = enemy.enemyName.ToUpper();
                bossStuffs[1].SetActive(true); // slider
                bossStuffs[1].GetComponent<Slider>().maxValue = enemy.health;
                return;
            }*/
            agent.Warp(new Vector3(Random.Range(-135, 135), 5, Random.Range(-135, 135))); // hoping this drops the enemy down to floor height (IT DOES)
            if (ws.wave > 2)
            {
                int rng = Random.Range(1, 31);
                if (rng < 31 && ws.wave > 9)
                {
                    enemy = validEnemies[5];
                }
                else if (rng < 31)
                {
                    enemy = validEnemies[4];
                }
                if (rng < 30 && ws.wave > 7)
                {
                    enemy = validEnemies[4];
                }
                else if (rng < 30)
                {
                    enemy = validEnemies[3];
                }
                if (rng < 25 && ws.wave > 4)
                {
                    enemy = validEnemies[3];
                }
                else if (rng < 25)
                {
                    enemy = validEnemies[2];
                }
                if (rng < 21 && ws.wave > 3)
                {
                    enemy = validEnemies[2];
                }
                else if (rng < 21)
                {
                    enemy = validEnemies[1];
                }
                if (rng < 16)
                {
                    enemy = validEnemies[1];
                }
                if (rng < 10)
                {
                    enemy = validEnemies[0];
                }
            } // sorry
        }
        sr.sprite = enemy.sprite;
        health = enemy.health;
        damage = enemy.damage;
        agent.speed = enemy.speed;
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
                if (Random.Range(1, enemy.chanceOfDrop) == 1)
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
                if (Random.Range(1, enemy.chanceOfDrop * 12) == 1)
                {
                    GameObject wd = Instantiate(artifactPickupOnDeath, transform.position, Quaternion.identity);
                    if (wd.GetComponent<ArtifactPickup>() != null)
                    {
                        wd.GetComponent<ArtifactPickup>().me = enemy.artifactDrop;
                    }
                    wd.SetActive(true);
                }
            }
            if (enemy.boss)
            {
                bossStuffs[0].SetActive(false);
                bossStuffs[1].SetActive(false);
            }
            Destroy(gameObject);
        }
        else
        {
            agent.enabled = true;
            agent.SetDestination(player.position);
        }
        health += enemy.regen * Time.deltaTime;
        if (health > enemy.health)
        {
            health = enemy.health;
        }
        if (ws.wave == 15 || ws.wave == 35)
        {
            bossStuffs[1].GetComponent<Slider>().value = health;
        }
        if (Vector3.Distance(player.position, transform.position) <= 20 && player.GetComponent<PlayerScript>().singing == 1)
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
            health -= 6 * Time.deltaTime;
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

    public bool spawnedByWave = true;
    public WaveScript ws;

    public EnemyScriptable[] validEnemies;
    public EnemyScriptable[] validBosses;

    public GameObject[] specificGO;

    public GameObject fire;

    public GameObject[] bossStuffs;
}
