using UnityEngine;
public class WeaponPickup : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = me.sprite;
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("performanceMode") == 1)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public Weapon me;

    public float timer;
}