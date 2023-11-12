using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatItem : MonoBehaviour
{
    //Text 
    ChatText chatText;
    //RectTransform
    RectTransform rt;
    //preferredHeight
    float preferredH;

    void Awake()
    {
        chatText = GetComponent<ChatText>();

        chatText.onChangedSize = OnChangedTextSize;

        rt = GetComponent<RectTransform>();
    }

    private void Update()
    {

    }

    //Text ����, Text������ ũ�⿡ �°� �ڽ��� ContetSize�� ����
    public void SetText(string s)
    {
        chatText.text = s;
    }

    void OnChangedTextSize()
    {
        if (preferredH != chatText.preferredHeight)
        {
            //chatText.text �� ũ�⿡ �°� ContetSize������
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);

            preferredH = chatText.preferredHeight;
        }
    }
}
