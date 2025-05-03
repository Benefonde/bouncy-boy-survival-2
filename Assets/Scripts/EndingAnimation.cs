using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<PlayerScript>() != null)
        {
            FindObjectOfType<PlayerScript>().Stop();
        }
        cam = Camera.main;
        playerLight = FindObjectOfType<Light>();
        aud = GetComponent<AudioSource>();

        if (Environment.GetCommandLineArgs().ToList().Contains("-bleh"))
        {
            aud.Stop();
            aud.clip = musicGood;
            aud.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (skyColor < 0.6f)
        {
            skyColor += Time.deltaTime / 12;
        }
        RenderSettings.ambientLight = new Color(skyColor, skyColor - 0.1f, skyColor - 0.3f);
        cam.backgroundColor = new Color(skyColor / 8, skyColor, skyColor * 1.6666667f);
        playerLight.range = 45 - skyColor * 70;
        shadowIg.color = new Color(0, 0, 0, skyColor * 1.55555556f);

        if (!aud.isPlaying)
        {
            SceneManager.LoadScene("Title");
        }
    }

    public float skyColor;

    Camera cam;

    Light playerLight;

    public SpriteRenderer shadowIg;

    public Transform player;

    AudioSource aud;
    public AudioClip musicGood;
}
