using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesBroHaveAScript : MonoBehaviour
{
    void Update()
    {
        if (GetComponent<EnemyScript>() == null)
        {
            Destroy(gameObject);
        } 
    }
}
