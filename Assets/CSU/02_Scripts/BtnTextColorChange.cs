using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnTextColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Text text;

    private void Awake()
    {
        text = transform.GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (text.color == Color.black) return;
        Color color = new Color(0.7f, 0.7f, 0.7f);
        text.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (text.color == Color.black) return;
        text.color = Color.white;
    }
}
