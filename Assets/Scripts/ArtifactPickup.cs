using UnityEngine;
public class ArtifactPickup : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = me.sprite;
    }
    public Artifact me;
}