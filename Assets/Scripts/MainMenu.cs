using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject black;
    [SerializeField] GameObject settingsPanel;
    public void Play() 
    {
        Invoke("PlayGame", 1f);
    }

    public void PlayGame()
    {
        //black.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void Settings(bool isOpened)
    {
        settingsPanel.SetActive(!isOpened);
    }
}
