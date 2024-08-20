using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBMainButtons : MonoBehaviour
{
    public Button[] buttons;
    public Button leftArrow;  
    public Button rightArrow; 

    [SerializeField] private int currentButtonIndex = 0;
    void Start()
    {

        leftArrow.onClick.AddListener(OnLeftArrowClick);
        rightArrow.onClick.AddListener(OnRightArrowClick);

        UpdateButtons();
    }
    void OnLeftArrowClick()
    {
        currentButtonIndex--;
        if (currentButtonIndex < 0)
        {
            currentButtonIndex = buttons.Length - 1; 
        }
        UpdateButtons();
    }

    void OnRightArrowClick()
    {
        currentButtonIndex++;
        if (currentButtonIndex >= buttons.Length)
        {
            currentButtonIndex = 0; 
        }
        UpdateButtons();
    }
    void UpdateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(i == currentButtonIndex);
        }
    }



}
