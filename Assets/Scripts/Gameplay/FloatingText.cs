using UnityEngine;
using UnityEngine.UI; // Используем Unity UI для работы с компонентом Text

public class FloatingText : MonoBehaviour
{
    public float floatSpeed = 1.0f; // Скорость движения текста вверх
    public float fadeSpeed = 1.0f;  // Скорость исчезновения текста
    private Text text;              // Ссылка на компонент Text
    private CanvasGroup canvasGroup; // Ссылка на CanvasGroup для управления прозрачностью

    private void Start()
    {
        text = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        // Перемещение текста вверх
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        transform.position += Vector3.left * floatSpeed* 0.5f * Time.deltaTime;

        // Постепенное уменьшение альфа канала (прозрачности)
        canvasGroup.alpha -= fadeSpeed * Time.deltaTime;

        // Уничтожение объекта, когда текст полностью исчезает
        if (canvasGroup.alpha <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
