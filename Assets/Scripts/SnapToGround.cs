using UnityEngine;
public class SnapToGround : MonoBehaviour
{
    void Update()
    {
        if (!waitehd)
        {
            Camera cam = Camera.main;
            if (Physics.Raycast(gameObject.GetComponent<Collider>().bounds.min + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit))
            {
                transform.position -= Vector3.up * Vector3.Distance(gameObject.GetComponent<Collider>().bounds.min, hit.point);
            }

            cam.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            cam.transform.rotation = transform.rotation;
            cam.transform.Translate(cam.transform.forward * 6, Space.World);
            cam.transform.Rotate(new Vector3(0, 180, 0));
            waitehd = true;
        }
    }

    bool waitehd;
}