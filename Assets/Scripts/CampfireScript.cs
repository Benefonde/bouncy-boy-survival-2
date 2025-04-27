using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    void Update()
    {
        player.hp += Mathf.Clamp(10 - Vector3.Distance(player.transform.position, transform.position), 0, 5) * Time.deltaTime;
    }

    PlayerScript player;
}
