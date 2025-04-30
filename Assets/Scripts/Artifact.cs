using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifact", menuName = "Artifact", order = 1)]
public class Artifact : ScriptableObject
{
    public Sprite sprite;
    public float durability;
    public string enemyEffective;
}
