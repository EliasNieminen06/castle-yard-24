using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject creditsWindow;

    public void LoadScene(int index)
	{
        SceneManager.LoadScene(index);
	}

    public void ToggleCredits()
    {
        creditsWindow.SetActive(!creditsWindow.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
