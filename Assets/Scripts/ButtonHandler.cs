using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isButtonPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        // ��ư�� ������ �� ����� �ڵ�.
        isButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ��ư�� ���� �� ����� �ڵ�.
        isButtonPressed = false;
    }
}
