using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu1;
    public GameObject mainMenu2;

    private void Start() 
    {
        mainMenu1.SetActive(true);
        mainMenu2.SetActive(false);
    }
    public void Settings()
    {
        mainMenu1.SetActive(false);
        mainMenu2.SetActive(true);
    }

    public void BackToMenu()
    {
        mainMenu1.SetActive(true);
        mainMenu2.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }
}
