using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelAnim : MonoBehaviour
{
    public float floatSpeed = 10.0f; // Скорость движения текста вверх
    private void Update()
    {
        transform.position += Vector3.down * floatSpeed * Time.deltaTime;

        if (transform.position.y < -35)
        {
            Destroy(gameObject);
        }
    }
}
