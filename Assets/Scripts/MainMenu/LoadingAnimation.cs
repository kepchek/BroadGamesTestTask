using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PaintDropEffect : MonoBehaviour
{
    public GameObject[] paintDrops;  // Массив префабов капель
    public int numberOfDrops = 100;  // Количество капель
    public float fadeDuration = 2f;  // Длительность исчезновения
    public float maxAlpha = 1f;      // Максимальная непрозрачность капли
    public float dropSpawnInterval = 0.1f; // Интервал спавна капель

    public RectTransform dropsContainer; // Контейнер для капель

    void Start()
    {
        if (dropsContainer == null)
        {
            Debug.LogError("DropsContainer не установлен!");
            return;
        }

        StartCoroutine(SpawnDrops());
    }

    IEnumerator SpawnDrops()
    {
        for (int i = 0; i < numberOfDrops; i++)
        {
            SpawnDrop();
            yield return new WaitForSeconds(dropSpawnInterval);
        }

        yield return new WaitForSeconds(fadeDuration);
        StartCoroutine(FadeOutDrops());
    }

    void SpawnDrop()
    {
        // Выбираем случайный префаб капли
        GameObject dropPrefab = paintDrops[Random.Range(0, paintDrops.Length)];

        // Создаем каплю внутри контейнера
        GameObject drop = Instantiate(dropPrefab, dropsContainer);
        Image dropImage = drop.GetComponent<Image>();

        // Генерация случайной позиции в пределах контейнера
        Vector2 randomPosition = GetRandomPositionWithinContainer();
        drop.GetComponent<RectTransform>().anchoredPosition = randomPosition;

        // Устанавливаем начальную альфа-прозрачность
        Color color = dropImage.color;
        color.a = 0f;
        dropImage.color = color;

        // Анимация проявления капли
        StartCoroutine(FadeIn(dropImage));
    }

    Vector2 GetRandomPositionWithinContainer()
    {
        Vector2 containerSize = dropsContainer.sizeDelta;

        // Преобразование в координаты контейнера с учетом якорей
        float randomX = Random.Range(-containerSize.x / 2f, containerSize.x / 2f);
        float randomY = Random.Range(-containerSize.y / 2f, containerSize.y / 2f);

        return new Vector2(randomX, randomY);
    }

    IEnumerator FadeIn(Image image)
    {
        Color color = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, maxAlpha, elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        color.a = maxAlpha;
        image.color = color;
    }

    IEnumerator FadeOutDrops()
    {
        Image[] dropImages = dropsContainer.GetComponentsInChildren<Image>();
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            foreach (Image dropImage in dropImages)
            {
                Color color = dropImage.color;
                color.a = Mathf.Lerp(maxAlpha, 0f, elapsedTime / fadeDuration);
                dropImage.color = color;
            }
            yield return null;
        }

        // Удаление капель после исчезновения
        foreach (Image dropImage in dropImages)
        {
            Destroy(dropImage.gameObject);
        }

        // Загрузка сцены с главным меню
        LoadMainMenu();
    }

    void LoadMainMenu()
    {
        // Замените "MainMenu" на название вашей сцены с главным меню
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
