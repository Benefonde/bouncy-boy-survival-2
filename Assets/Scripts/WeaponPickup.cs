using UnityEngine;
public class WeaponPickup : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = me.sprite;
    }
    public Weapon me;
}