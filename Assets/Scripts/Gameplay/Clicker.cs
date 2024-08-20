using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    public float clickReward = 0.01f;   // Награда прогресса за 25 нажатий, меняется в зависимости от бустеров 

    // Ссылки на кнопки
    public Button button1;
    public Button button2;
    public Button button3;

    // Ссылки на прогресс-бары
    public Image slider1;
    public Image slider2;
    public Image slider3;
    private bool slider1Ready = false;
    private bool slider2Ready = false;
    private bool slider3Ready = false;

    // Отображение прогресса в процентах
    public Text progressText1;
    public Text progressText2;
    public Text progressText3;

    // Количество нажатий
    private int clicks1 = 0;
    private int clicks2 = 0;
    private int clicks3 = 0;

    private const int clicksForOnePercent = 25;
    public int coins = 0;
    public Text coinCount;

    // Бонусы
    private bool coinMultiplierActive = false;
    private int coinMultiplierClicksRemaining = 0;
    private int coinMultiplierValue = 5;  // Умножение монет

    private bool progressMultiplierActive = false;
    private int progressMultiplierClicksRemaining = 0;
    private float progressMultiplierValue = 1f;  // Множитель для прогресса

    private bool autoClickActive = false;
    private int autoClickClicksRemaining = 0;
    private int autoClickInterval = 1;  // Интервал автоклика

    // Стоимость бонусов
    private int coinMultiplierCost = 250;
    private int progressMultiplierCost = 250;
    private int autoClickCost = 250;

    private const int costIncrease = 150;  // Увеличение стоимости после каждой покупки

    // Ссылки на кнопки бонусов
    public Button coinMultiplierButton;
    public Button progressMultiplierButton;
    public Button autoClickButton;

    // Ссылки на попапы для каждого бонуса
    public GameObject coinMultiplierPopup;
    public Text coinMultiplierPopupMessage;
    public Button coinMultiplierPopupConfirmButton;
    public Button coinMultiplierPopupCancelButton;

    public GameObject progressMultiplierPopup;
    public Text progressMultiplierPopupMessage;
    public Button progressMultiplierPopupConfirmButton;
    public Button progressMultiplierPopupCancelButton;

    public GameObject autoClickPopup;
    public Text autoClickPopupMessage;
    public Button autoClickPopupConfirmButton;
    public Button autoClickPopupCancelButton;

    // Анимации текста монет
    public GameObject floatingTextPrefab;  
    public GameObject floatingProgressTextPrefab;
    public Transform spawnPoint; 
    public string targetProgressBarName1;
    public string targetProgressBarName2;   
    public string targetProgressBarName3;           

    public GameObject EndRoundAnim;  
    

    void Start()
    {
        // Обработчики нажатий для кнопок и обнуление всех показателей
        button1.onClick.AddListener(OnButton1Click);
        button2.onClick.AddListener(OnButton2Click);
        button3.onClick.AddListener(OnButton3Click);
        slider1.fillAmount = 0;
        slider2.fillAmount = 0;
        slider3.fillAmount = 0;
        progressText1.text = "0%";
        progressText2.text = "0%";
        progressText3.text = "0%";

        // Подключаем обработчики для бонусов
        coinMultiplierButton.onClick.AddListener(() => ShowPopup("coinMultiplier"));
        progressMultiplierButton.onClick.AddListener(() => ShowPopup("progressMultiplier"));
        autoClickButton.onClick.AddListener(() => ShowPopup("autoClick"));

        // Обработчики для попапа монетного бонуса
        coinMultiplierPopupConfirmButton.onClick.AddListener(ConfirmCoinMultiplierPurchase);
        coinMultiplierPopupCancelButton.onClick.AddListener(() => ClosePopup("coinMultiplier"));

        // Обработчики для попапа прогресс бонуса
        progressMultiplierPopupConfirmButton.onClick.AddListener(ConfirmProgressMultiplierPurchase);
        progressMultiplierPopupCancelButton.onClick.AddListener(() => ClosePopup("progressMultiplier"));

        // Обработчики для попапа автоклика
        autoClickPopupConfirmButton.onClick.AddListener(ConfirmAutoClickPurchase);
        autoClickPopupCancelButton.onClick.AddListener(() => ClosePopup("autoClick"));

        coinMultiplierPopup.SetActive(false);
        progressMultiplierPopup.SetActive(false);
        autoClickPopup.SetActive(false);
    }
    private void Update() {
        if(slider1Ready && slider2Ready && slider3Ready)
        {
            Debug.Log("Переход на некст локу");
            EndRoundAnim.SetActive(true);
        }
    }

    void OnButton1Click()
    {
        if(slider1.fillAmount < 1)
        {
            HandleClick(ref clicks1, slider1, progressText1);
            Debug.Log(clicks1);
            if(clicks1>=clicksForOnePercent-1)
            {
                ShowFloatingProgressText(targetProgressBarName1);
            }
        }
        else
        {
            slider1Ready = true;
        }

    }

    void OnButton2Click()
    {
        if (slider2.fillAmount < 1)
        {
            HandleClick(ref clicks2, slider2, progressText2);
            if(clicks2>=clicksForOnePercent-1)
            {
                ShowFloatingProgressText(targetProgressBarName2);
            }            
        }
        else
        {
            slider2Ready = true;
        }

    }

    void OnButton3Click()
    {
        if(slider3.fillAmount < 1)
        {
            HandleClick(ref clicks3, slider3, progressText3);
            if(clicks3>=clicksForOnePercent-1)
            {
                ShowFloatingProgressText(targetProgressBarName3);
            }
        }
        else
        {
            slider3Ready = true;
        }
    }

    void HandleClick(ref int clicks, Image slider, Text progressText)
    {
        // Проверяем активен ли мультипликатор монет
        int coinReward = coinMultiplierActive ? 10 : 5;
        coins += coinReward;
        coinCount.text = coins.ToString();
        ShowFloatingTextMoney();
        clicks++;
        UpdateProgress(slider, ref clicks, progressText);

        // Уменьшаем счетчик оставшихся кликов для бонусов
        if (coinMultiplierActive)
        {
            coinMultiplierClicksRemaining--;
            if (coinMultiplierClicksRemaining <= 0)
            {
                coinMultiplierActive = false;
            }
        }

        if (progressMultiplierActive)
        {
            progressMultiplierClicksRemaining--;
            if (progressMultiplierClicksRemaining <= 0)
            {
                progressMultiplierActive = false;
            }
        }

        if (autoClickActive)
        {
            autoClickClicksRemaining--;
            if (autoClickClicksRemaining <= 0)
            {
                autoClickActive = false;
                CancelInvoke("AutoClick");
            }
        }
    }

    // Основная логика при нажатии на кнопку
    void UpdateProgress(Image slider, ref int clicks, Text progressText)
    {
        int clicksRequired = progressMultiplierActive ? clicksForOnePercent / 2 : clicksForOnePercent; // Уменьшаем количество кликов при активном прогресс-ускорителе

        if(clicks >= clicksRequired)
        {
            slider.fillAmount += clickReward * (progressMultiplierActive ? 2 : 1); // Увеличиваем прогресс при активном бонусе
            progressText.text = Mathf.RoundToInt(slider.fillAmount * 100) + "%";
            clicks = 0;
        }
    }

    // Показываем попап окно для подтверждения покупки бонуса
    void ShowPopup(string bonusType)
    {
        if (bonusType == "coinMultiplier")
        {
            coinMultiplierPopupMessage.text = coinMultiplierCost.ToString();
            coinMultiplierPopup.SetActive(true);
            Time.timeScale = 0;
        }
        else if (bonusType == "progressMultiplier")
        {
            progressMultiplierPopupMessage.text = progressMultiplierCost.ToString();
            progressMultiplierPopup.SetActive(true);
            Time.timeScale = 0;
        }
        else if (bonusType == "autoClick")
        {
            autoClickPopupMessage.text = autoClickCost.ToString();
            autoClickPopup.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Подтверждение покупки бонуса монетного множителя
    void ConfirmCoinMultiplierPurchase()
    {
        if (coins >= coinMultiplierCost)
        {
            coins -= coinMultiplierCost;
            coinMultiplierCost += costIncrease;
            ActivateCoinMultiplier();
            coinCount.text = coins.ToString();
            ClosePopup("coinMultiplier");
            Time.timeScale = 1;
        }
    }

    // Подтверждение покупки бонуса прогресс множителя
    void ConfirmProgressMultiplierPurchase()
    {
        if (coins >= progressMultiplierCost)
        {
            coins -= progressMultiplierCost;
            progressMultiplierCost += costIncrease;
            ActivateProgressMultiplier();
            coinCount.text = coins.ToString();
            ClosePopup("progressMultiplier");
            Time.timeScale = 1;
        }
    }

    // Подтверждение покупки автоклика
    void ConfirmAutoClickPurchase()
    {
        if (coins >= autoClickCost)
        {
            coins -= autoClickCost;
            autoClickCost += costIncrease;
            ActivateAutoClick();
            coinCount.text = coins.ToString();
            ClosePopup("autoClick");
            Time.timeScale = 1;
        }
    }

    // Закрыть попап окно
    void ClosePopup(string bonusType)
    {
        if (bonusType == "coinMultiplier")
        {
            coinMultiplierPopup.SetActive(false);
            Time.timeScale = 1;
        }
        else if (bonusType == "progressMultiplier")
        {
            progressMultiplierPopup.SetActive(false);
            Time.timeScale = 1;
        }
        else if (bonusType == "autoClick")
        {
            autoClickPopup.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // Активация бонуса на удвоение монет
    void ActivateCoinMultiplier()
    {
        coinMultiplierActive = true;
        coinMultiplierClicksRemaining = 50;  // Бонус действует на 50 кликов
    }

    // Активация бонуса на ускорение прогресса
    void ActivateProgressMultiplier()
    {
        progressMultiplierActive = true;
        progressMultiplierClicksRemaining = 100;  // Бонус действует на 100 кликов
    }

    // Активация автоклика
    void ActivateAutoClick()
    {
        autoClickActive = true;
        autoClickClicksRemaining = 50;  // Бонус действует на 50 кликов
        InvokeRepeating("AutoClick", 0f, autoClickInterval);
    }

    void AutoClick()
    {
        OnButton1Click();  // В качестве примера автокликер нажимает кнопку 1
    }

    //*****************************************АНИМАЦИИ*****************************************

    public void ShowFloatingTextMoney()
    {
        int coinReward = coinMultiplierActive ? 10 : 5;
        // Создание текста на заданной позиции
        GameObject floatingText = Instantiate(floatingTextPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        
        // Устанавливаем начальное значение текста
        floatingText.GetComponent<Text>().text = "+" + coinReward.ToString();
    }

    public void ShowFloatingProgressText(string targetObjectName)
    {
        // Находим объект по имени
        GameObject targetObject = GameObject.Find(targetObjectName);
        if (targetObject == null)
        {
            Debug.LogError("Object not found: " + targetObjectName);
            return;
        }

        // Получаем компонент Image объекта
        Image targetImage = targetObject.GetComponent<Image>();
        if (targetImage == null || targetImage.sprite == null)
        {
            Debug.LogError("Image or Sprite not found on object: " + targetObjectName);
            return;
        }

        // Получаем текстуру из спрайта (текстура должна быть доступна для чтения)
        Texture2D texture = targetImage.sprite.texture;
        if (texture == null)
        {
            Debug.LogError("Texture not found in sprite: " + targetObjectName);
            return;
        }

        // Используем пипетку: берем средний цвет текстуры (по центру, например)
        Color pickedColor = texture.GetPixel(texture.width, texture.height);
        Debug.Log(pickedColor);
        pickedColor.a = 255;

        // Создаем объект с текстом
        GameObject floatingText = Instantiate(floatingProgressTextPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);

        // Устанавливаем текст
        Text textComponent = floatingText.GetComponent<Text>();
        textComponent.text = "+1%";

        // Применяем полученный цвет к тексту
        textComponent.color = pickedColor;
    }
}
