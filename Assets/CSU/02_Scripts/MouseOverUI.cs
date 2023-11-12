using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // MouseOver UI
    Transform child;
    // 말풍선, 꼬리 Image
    Image[] imgs = new Image[2];
    // 말풍선 Text
    Text text;
    // 알파값 변경을 위한 Color
    Color imgColor;
    Color textColor;

    // UI 크기 변경 속도
    float changeSpeed = 5f;

    private void Awake()
    {
        // 자식 및 UI 찾기
        if (child == null) child = transform.Find("MouseOverUI");

        if (child != null)
        {
            imgs = child.GetComponentsInChildren<Image>();
            text = child.GetComponentInChildren<Text>();

            child.gameObject.SetActive(false);

            // Color 초기화
            imgColor = imgs[0].color;
            textColor = text.color;
            AlphaReset();
        }
    }

    // 마우스가 UI 위에 있을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine("SizeUp");
        if (child != null) StartCoroutine("AlphaUp");
    }

    // 마우스가 UI 를 빠져 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine("SizeDown");
    }

    #region UI 크기 변경(증가, 감소) 코루틴
    IEnumerator SizeUp()
    {
        if (child != null) child.gameObject.SetActive(true);

        while (transform.localScale.x < 1.19f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.2f, Time.deltaTime * changeSpeed);
            yield return null;
        }

        transform.localScale = Vector3.one * 1.2f;
    }

    IEnumerator SizeDown()
    {
        if (child != null)
        {
            child.gameObject.SetActive(false);
            AlphaReset();
        }

        while (transform.localScale.x > 1.001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * changeSpeed);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
    #endregion

    #region child 알파값 증가 코루틴 & 감소(초기화) 함수
    IEnumerator AlphaUp()
    {
        imgColor.a = 1f;
        textColor.a = 1f;

        while (text.color.a < 0.9f)
        {
            text.color = Color.Lerp(text.color, textColor, Time.deltaTime * changeSpeed);
            imgs[0].color = Color.Lerp(imgs[0].color, imgColor, Time.deltaTime * changeSpeed);
            imgs[1].color = Color.Lerp(imgs[1].color, imgColor, Time.deltaTime * changeSpeed);
            yield return null;
        }

        imgs[0].color = imgColor;
        imgs[1].color = imgColor;
        text.color = textColor;
    }

    void AlphaReset()
    {
        imgColor.a = 0f;
        textColor.a = 0f;

        imgs[0].color = imgColor;
        imgs[1].color = imgColor;
        text.color = textColor;
    }
    #endregion
}
