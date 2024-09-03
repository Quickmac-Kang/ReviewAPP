using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isButtonPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        // 버튼이 눌렸을 때 실행될 코드.
        isButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 버튼이 뗐을 때 실행될 코드.
        isButtonPressed = false;
    }
}
