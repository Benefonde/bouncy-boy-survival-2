using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb.useGravity)
        {
            rb.AddForce(transform.up * speed * 15 * (Camera.main.transform.rotation.x));
        }
    }

    void Update()
    {
        if (rb.useGravity)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(-rb.velocity.y, transform.rotation.y, transform.rotation.z));
        }
        else
        {
            transform.Translate(transform.forward * speed * Time.deltaTime);
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (player)
        {
            if (other.transform.name == "Enemy(Clone)")
            {
                other.gameObject.GetComponent<EnemyScript>().health -= damage;
            }
        }
        else
        {
            if (other.transform.name == "Player")
            {
                other.gameObject.GetComponent<PlayerScript>().hp -= damage;
            }
        }
    }

    Rigidbody rb;

    public float speed;

    public bool player;
    public int damage;
    public float timer;
}
