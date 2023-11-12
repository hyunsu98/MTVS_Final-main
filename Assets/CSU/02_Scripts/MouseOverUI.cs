using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // MouseOver UI
    Transform child;
    // ��ǳ��, ���� Image
    Image[] imgs = new Image[2];
    // ��ǳ�� Text
    Text text;
    // ���İ� ������ ���� Color
    Color imgColor;
    Color textColor;

    // UI ũ�� ���� �ӵ�
    float changeSpeed = 5f;

    private void Awake()
    {
        // �ڽ� �� UI ã��
        if (child == null) child = transform.Find("MouseOverUI");

        if (child != null)
        {
            imgs = child.GetComponentsInChildren<Image>();
            text = child.GetComponentInChildren<Text>();

            child.gameObject.SetActive(false);

            // Color �ʱ�ȭ
            imgColor = imgs[0].color;
            textColor = text.color;
            AlphaReset();
        }
    }

    // ���콺�� UI ���� ���� ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine("SizeUp");
        if (child != null) StartCoroutine("AlphaUp");
    }

    // ���콺�� UI �� ���� ������ ��
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine("SizeDown");
    }

    #region UI ũ�� ����(����, ����) �ڷ�ƾ
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

    #region child ���İ� ���� �ڷ�ƾ & ����(�ʱ�ȭ) �Լ�
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
