using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
    }

    public void PauseToggle()
    {
        pause.SetActive(!pause.activeSelf);
        Cursor.visible = pause.activeSelf;
        if (pause.activeSelf)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Bye()
    {
        SceneManager.LoadScene("Title");
    }

    public void DIE()
    {
        FindObjectOfType<PlayerScript>().hp = 0;
        PauseToggle();
    }

    public GameObject pause;
}
