using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class StartGameUIEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ScreenUIManager screenUI;
    public float animSpeed = 1;
    public bool isSelected = false;

    public IEnumerator UIEventCountdown()
    {
        float countDown = 0;
        while(isSelected && countDown <= 1)
        {
            countDown += Time.deltaTime * animSpeed;
            screenUI.startSlider.value = Mathf.Clamp01(countDown/1f);
            yield return null;
        }
        if(isSelected)
        {
            screenUI.StartGame();
        }
        screenUI.startSlider.value = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isSelected = true;
        StartCoroutine(UIEventCountdown());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isSelected = false;
    }
}