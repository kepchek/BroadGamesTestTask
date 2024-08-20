using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void BackToMenu1()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
