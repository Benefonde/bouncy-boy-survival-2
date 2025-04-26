using UnityEngine;
public class SpikySummoner : MonoBehaviour
{
    void Update()
    {
        if (timmer <= 0)
        {
            GameObject a = Instantiate(spikayBoy, transform.position, Quaternion.identity);
            a.GetComponent<EnemyScript>().spawnedByWave = false;
            a.GetComponent<EnemyScript>().enemy = spikkyBoys[0];
            if (Random.Range(1, 5) == 3)
            {
                a.GetComponent<EnemyScript>().enemy = spikkyBoys[1];
            }
            timmer = Random.Range(1f, 12f);
        }
        else
        {
            timmer -= Time.deltaTime;
        }
    }
    public GameObject spikayBoy;
    public EnemyScriptable[] spikkyBoys;
    float timmer;
}