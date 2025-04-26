using UnityEngine;
using UnityEngine.SceneManagement;
public class Dead : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            SceneManager.LoadScene("Title");
        }
    }
}