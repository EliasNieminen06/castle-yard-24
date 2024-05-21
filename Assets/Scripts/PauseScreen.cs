using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;

    private bool paused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) UnPause();
            else Pause();
        }
    }

    private void Pause()
    {
        paused = true;
        pauseScreen.SetActive(true);
        StatsAndHealthText.Show_Static();
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        paused = false;
        pauseScreen.SetActive(false);
        StatsAndHealthText.Hide_Static(0);
        
        if (!LevelUpScreen.active) Time.timeScale = 1;
    }

    public void Quit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
