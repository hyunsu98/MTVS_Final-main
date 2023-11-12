using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseUI_LHS : MonoBehaviour, IPointerClickHandler
{
    // MouseOver UI
    public Transform taget;
    //public Text text;
    //public Text textView;
    //public Text textHeart;

    public GameObject mouseOver;

    // UI ũ�� ���� �ӵ�
    public float changeSpeed = 5f;

    bool zero = true;

    public Image[] imgs = new Image[2];

    // ���İ� ������ ���� Color
    Color imgColor;
    Color textColor;

    public static MouseUI_LHS instance;

    [SerializeField]
    public bool isChat;

    private void Awake()
    {
        //imgColor = imgs[0].color;
        //textColor = text.color;
        //AlphaReset();

        instance = this;
    }

    void start()
    {
        isChat = false;
    }

    // ���콺�� UI ���� ���� ��
    public void OnPointerClick (PointerEventData eventData)
    {
        StopAllCoroutines();

        if(zero == true)
        {
            //PlayerMove_LHS.instance.isMove = false;
            //UiController_LHS.instance.ismic = false;

            isChat = true;

            zero = false;
            StartCoroutine("SizeUp");

            //StartCoroutine("AlphaUp");

            mouseOver.SetActive(false);

        }
        else
        {
            //PlayerMove_LHS.instance.isMove = true;
            //UiController_LHS.instance.ismic = false;

            isChat = false;

            zero = true;
            StartCoroutine("SizeDown");

            //imgColor = imgs[0].color;
            //textColor = text.color;
            //AlphaReset();
        }
    }

    #region UI ũ�� ����(����, ����) �ڷ�ƾ
    IEnumerator SizeUp()
    {
       

        while (taget.localScale.x < 1f)
        {
            taget.localScale = Vector3.Lerp(taget.localScale, Vector3.one, Time.deltaTime * changeSpeed);
            yield return null;
        }

        taget.localScale = Vector3.one;

    }

    IEnumerator SizeDown()
    {
       
        //AlphaReset();

        while (taget.localScale.x > 0)
        {
            taget.localScale = Vector3.Lerp(taget.localScale, Vector3.zero, Time.deltaTime * changeSpeed);
            yield return null;
        }

        taget.localScale = Vector3.zero;

    }
    #endregion

    #region child ���İ� ���� �ڷ�ƾ & ����(�ʱ�ȭ) �Լ�
    //IEnumerator AlphaUp()
    //{
    //    imgColor.a = 1f;
    //    textColor.a = 1f;

    //    while (text.color.a < 0.9f)
    //    {
    //        text.color = Color.Lerp(text.color, textColor, Time.deltaTime * changeSpeed);
    //        imgs[0].color = Color.Lerp(imgs[0].color, imgColor, Time.deltaTime * changeSpeed);
    //        imgs[1].color = Color.Lerp(imgs[1].color, imgColor, Time.deltaTime * changeSpeed);
    //        yield return null;
    //    }

    //    imgs[0].color = imgColor;
    //    imgs[1].color = imgColor;
    //    text.color = textColor;
    //}

    //void AlphaReset()
    //{
    //    imgColor.a = 0f;
    //    textColor.a = 0f;

    //    imgs[0].color = imgColor;
    //    imgs[1].color = imgColor;
    //    text.color = textColor;
    //}
    #endregion
}
