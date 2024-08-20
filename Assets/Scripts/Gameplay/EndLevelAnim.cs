using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndLevelAnim : MonoBehaviour
{
    public float floatSpeed = 10.0f; // Скорость движения текста вверх
    private void Update()
    {
        if (transform.position.y < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            transform.position += Vector3.down * floatSpeed * Time.deltaTime;
        }
    }
}
