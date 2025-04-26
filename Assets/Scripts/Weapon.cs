using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public Sprite sprite;
    public float damage;
    public int durability;
    public bool ranged;
    public int projectile;
    public float range;
}
