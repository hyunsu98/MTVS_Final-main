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

    //Text 셋팅, Text내용의 크기에 맞게 자신의 ContetSize를 변경
    public void SetText(string s)
    {
        chatText.text = s;
    }

    void OnChangedTextSize()
    {
        if (preferredH != chatText.preferredHeight)
        {
            //chatText.text 의 크기에 맞게 ContetSize를변경
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);

            preferredH = chatText.preferredHeight;
        }
    }
}
