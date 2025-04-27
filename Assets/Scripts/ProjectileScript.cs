using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    void Start()
    {
        if (!singing)
        {
            originalRot = transform.rotation.eulerAngles;
            rb = GetComponent<Rigidbody>();
            if (rb.useGravity)
            {
                rb.AddForce(transform.up * speed * yahoo);
            }
        }
    }

    void Update()
    {
        if (!singing)
        {
            if (rb.useGravity)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, originalRot.y, originalRot.z));
                transform.Translate(speed * Time.deltaTime * transform.forward, Space.World);
                transform.rotation = Quaternion.Euler(new Vector3(-rb.velocity.y, originalRot.y, originalRot.z));
            }
            else
            {
                transform.Translate(speed * Time.deltaTime * transform.forward, Space.World);
            }
        }
        if (singing)
        {
            GetComponent<SphereCollider>().radius += 1 * Time.deltaTime;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Enemy(Clone)" && other.gameObject.GetComponent<EnemyScript>().enemy != thatOne)
        {
            other.gameObject.GetComponent<EnemyScript>().health -= damage;
        }
        if (!player)
        {
            if (other.transform.name == "Player")
            {
                other.gameObject.GetComponent<PlayerScript>().hp -= damage;
                if (transform.name == "Spider Cobweb")
                {
                    StartCoroutine(other.gameObject.GetComponent<PlayerScript>().Cobweb());
                }
            }
        }
    }

    Rigidbody rb;

    public float speed;

    public bool player;
    public int damage;
    public float timer;

    public bool singing;

    Vector3 originalRot;

    public EnemyScriptable thatOne;

    public int yahoo = 15;
}
