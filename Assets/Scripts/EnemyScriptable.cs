using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy", order = 1)]
public class EnemyScriptable : ScriptableObject
{
    public Sprite sprite;
    public string enemyName;
    public int health;
    public float regen;
    public float speed;
    public float damage;
}
