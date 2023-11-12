using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System;

//ä�� �α� ����
//Json - ��ü ���� ������ 
[System.Serializable]
public struct ChatInfoList
{
    public string opponentName;
    public List<ChatInfo> data;
}

//Json�� ��� ���� "Ű" : "��"
[System.Serializable]
public struct ChatInfo
{
    public string nickName;
    public string chatText;
}

//ê�� ��ȭ
//Json�� ��� ���� "Ű" : "��"
[System.Serializable]
public struct AiChatInfo
{
    public string chatRequest;
    public string memberNickname;
    public string weNickname;
}

public class ChatManager : MonoBehaviourPun
{
    [Header("���̽� ä�� �ɼ�")]
    public Button but_VoiceChat;
    
    [Header("�Ϲ� ä��")]
    //InputChat -> ����ڰ� ä���� ����
    public InputField inputChat;
    //ScorllView�� Content
    [Header("*ContentRect*")]
    public RectTransform trContent;
    [Header("*ChatItem*")]
    public GameObject chatItemMeFactory;
    public GameObject chatItemOtherFactory;
    public GameObject chatItemRoomFactory;
    [Header("*ChatItem*")]
    public Scrollbar scrollBar;
    public Scrollbar AIscrollBar;

    ChatAreaScript LastArea;
    ChatAreaScript Area;

    ChatAreaScript AILastArea;
    ChatAreaScript AIArea;



    [Header("���� ä��")]
    public InputField aiInputChat;
    //ScorllView�� Content
    public RectTransform aiContent;
    public GameObject chatItemAIFactory;
    public GameObject chatItemAIOtherFactory;

    [Header("Json")]
    //��ü ���� ������ ����
    public List<ChatInfo> chatList = new List<ChatInfo>();
    string chatBot;

    [Header("ä�� UI")]
    // MainChat �˾�â
    Animator anim;
    public GameObject mainPopup;
    Animator aiAnim;
    public GameObject aiPopup;
    // BtnChat �˾�â
    public GameObject chatPopup;
    //���� ��ư
    public GameObject btn_Restore;
    // ���̽� �˾�â
    public GameObject voicePopup;
    public GameObject voiceChatPopup;
    Animator voiceAnim;

    //�� ���̵� ��
    Color idColor;
    Color aiColor;
    Color otherColor;

    //���� �÷��� ��
    int playerAll;

    //���� Content�� H
    float prevContentH;

    //ScorllView�� RectTransform
    [Header("��ũ�Ѻ�")]
    public RectTransform trScrollView;
    public RectTransform AIScrollView;

    string jsonData;
    GameObject item;

    // ä��â ������ ���� ���̿���
    // �ٵ� �߸��߾ �̷��� ���� ������ �ʿ䰡 ���� �� ����
    // �ڵ� ������ �� �ٽ� �غ���
    bool isSend;
    Texture2D picture;
    GameObject itemAI;
    GameObject itemAIChat;


    void Start()
    {
        //InputField���� ���͸� ���� �� ȣ��Ǵ� �Լ� ���
        inputChat.onSubmit.AddListener(OnSubmit);
        inputChat.onValueChanged.AddListener(ValueChanged);


        aiInputChat.onSubmit.AddListener(OnSubmitAI);
        aiInputChat.onValueChanged.AddListener(AiValueChanged);

        //���콺 Ŀ�� ��Ȱ��ȭ
        //Cursor.visible = false;

        //idColor�� �����ϰ�
        //idColor = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);

        //idColor = Color.yellow;
        //otherColor = Color.green;
        //aiColor = Color.blue;

        // Chat �˾�â
        anim = mainPopup.GetComponent<Animator>();
        aiAnim = aiPopup.GetComponent<Animator>();
        voiceAnim = voicePopup.GetComponent<Animator>();

        but_VoiceChat.onClick.AddListener(() => OnVoicePopUp());
    }

    void Update()
    {
        //���� �濡 �����ִ� �ο��� // ȥ���϶��� ê�� �����ϰ�!
        playerAll = PhotonNetwork.CurrentRoom.PlayerCount;

        //if (playerAll > 2)
        //{
        //    idColor = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
        //}

        #region ���콺 ����
        //���࿡ escŰ�� ������ Ŀ�� Ȱ��ȭ
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Cursor.visible = true;
        //}

        ////���࿡ ���콺 Ŭ���� �ϸ� Ŀ�� ��Ȱ��ȭ
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //���࿡ Ŀ���� �ش� ��ġ�� UI�� ������
        //    //IsPointerOverGameObject�� pointer�� UI�� �ִ� ��� True�� �ƴ� ��쿡�� false�� ��ȯ
        //    if (EventSystem.current.IsPointerOverGameObject() == false)
        //    {
        //        Cursor.visible = false;
        //    }
        //}
        #endregion

        //�ִϸ��̼��� ������ �˾�â �ݱ�
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("close"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                mainPopup.SetActive(false);
            }
        }

        //�ִϸ��̼��� ������ �˾�â �ݱ�
        if (voiceAnim.GetCurrentAnimatorStateInfo(0).IsName("close"))
        {
            if (voiceAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                voicePopup.SetActive(false);
            }
        }

        ////���� 0���� �����ٸ� ������� Ȱ��ȭ
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    chatPopup.GetComponent<Image>().enabled = false;
        //    chatPopup.GetComponent<Button>().enabled = false;

        //    voiceChatPopup.SetActive(false);

        //    btn_Restore.SetActive(true);
        //}
    }

    // ���� �÷��̾� �̸� �˱� ���ؼ�
    // ���� ������� �̸��� ����Ʈ�� ���?
    string findPlayer()
    {
        string s = "";

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].NickName != PhotonNetwork.NickName)
            {
                s = PhotonNetwork.PlayerList[i].NickName;
            }
        }
        return s;
    }
    void ValueChanged(string text) // string �Ű������� �⺻���� ���� �Ű������̴�
    {
        //if (photonView.IsMine == false) return;

        //Debug.Log(text + " - ���� �Է� ��");

        //PlayerMove_LHS.instance.isMove = false;


        //UiController_LHS.instance.ismic = false;
    }

    //InputField���� ���͸� ������ ȣ��Ǵ� �Լ�
    void OnSubmit(string s)
    {
        //UiController_LHS.instance.ismic = true;

        //PlayerMove_LHS.instance.isMove = true;

        //�ƹ��͵� ���� ���� ��
        //if (s.Trim() != "")
        //{

        //}
        
        //�̹����� ��� �ؾ��ұ� ?

        photonView.RPC("RpcAddChat", RpcTarget.All, PhotonNetwork.NickName, s);

        // ���ǹ� 
        //if (photonView.IsMine == true)
        //if (HttpManager.instance.nickname == photonView.Owner.NickName)
      
        //4. InputChat�� ������ �ʱ�ȭ
        inputChat.text = "";

        //5. InputChat�� Focusing �� ������.
        //Enter �� ������ InputField �� ��� Ȱ��ȭ �ǰ� ���ִ� �ڵ�
        inputChat.ActivateInputField();
    }

    void AiValueChanged(string text)
    {
        MouseUI_LHS.instance.isChat = true;
        //PlayerMove_LHS.instance.isMove = false; 
    }

    void OnSubmitAI(string s)
    {
        //MouseUI_LHS.instance.isChat = false;
        //PlayerMove_LHS.instance.isMove = true;

        //<color=#FFFFFF>�г���</color>
        //string aichatText = "<color=#" + ColorUtility.ToHtmlStringRGB(aiColor) + ">" + PhotonNetwork.NickName + "</color>" + " : " + s;

        //AIchat(aichatText);

        AIchat(s);

        //4. InputChat�� ������ �ʱ�ȭ
        aiInputChat.text = "";
        //print("�ʱ�ȭ��Ű��");

        //5. InputChat�� Focusing �� ������.
        //Enter �� ������ InputField �� ��� Ȱ��ȭ �ǰ� ���ִ� �ڵ�
        aiInputChat.ActivateInputField();

        AiChatInfo aiInfo = new AiChatInfo();

        aiInfo.chatRequest = s;
        aiInfo.memberNickname = "����";
        aiInfo.weNickname = "ȸ��";
        //aiInfo.memberId = photonView.Owner.NickName;
        //aiInfo.weId = findPlayer();

        //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);


        HttpManager.instance.isAichat = false;
        //AI�� ä���� �Ѵ�!
        OnGetPost(aiJsonData);
    }

    void OnSubmitAIBool(string s)
    {
        //PlayerMove_LHS.instance.isMove = false;
    }

    [PunRPC]
    void RpcAddChat(string nick, string chatText)
    {
        //���� ������ ����
        ChatInfo info = new ChatInfo();
        info.nickName = nick;
        info.chatText = chatText;

        //<color=#FFFFFF>�г���</color>
        //string s = "<color=#" + ColorUtility.ToHtmlStringRGB(new Color(r, g, b)) + ">" + nick + "</color>" + " : " + chatText;

        //0. �ٲ�� ���� Content H���� ����
        prevContentH = trContent.sizeDelta.y;

        if (chatText.Trim() == "") return;

        bool isBottom = scrollBar.value <= 0.00001f;

        // �̸��� ���� ���ٸ� chatItemFactory�� ��������
        if (HttpManager.instance.nickname == nick)
        {
            //print("�̸� ����");
            //item = Instantiate(chatItemFactory, trContent);
            item = Instantiate(chatItemMeFactory, trContent);

            isSend = true;
        }

        // ���� �̸��� �ٸ��ٸ� chatItemOtherFactory�� �����´�
        // �濡 ���� ������� ĳ���� �̹����� �����´�
        // �г����� ä��ģ ����� ���� ��
        // ����� �̹��� �� ���� �̸��� �̹����� �����ͼ� �ִ´�.
        else if(nick == "����")
        {
            //print("������ �� ��");
            //item = Instantiate(chatItemOtherFactory, trContent);
            item = Instantiate(chatItemOtherFactory, trContent);

            isSend = false;
        }

        //Test��
        else
        {
            //print("�̸� �ٸ�");
            //item = Instantiate(chatItemOtherFactory, trContent);
            item = Instantiate(chatItemOtherFactory, trContent);

            isSend = false;
        }


        //2.���� ChatItem���� ChatItem ������Ʈ �����´�
        //ChatItem chat = item.GetComponent<ChatItem>();

        #region ä�� ���� ����
        Area = item.GetComponent<ChatAreaScript>();
        Area.transform.SetParent(trContent.transform, false);
        ////���� �ִ� 600, ���̴� ����
        Area.BoxRect.sizeDelta = new Vector2(500, Area.BoxRect.sizeDelta.y);
        ////���� ����
        Area.TextRect.GetComponent<Text>().text = chatText;
        Fit(Area.BoxRect);

        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭, �� ���� �Ʒ��� �������� �ٷ� �� ũ�⸦ ���� 
        float X = Area.TextRect.sizeDelta.x + 42;
        float Y = Area.TextRect.sizeDelta.y;
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                Fit(Area.BoxRect);

                if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }
        else Area.BoxRect.sizeDelta = new Vector2(X, Y);

        // ���� �Ϳ� �б��� ������ ��¥�� �����̸� ����
        DateTime t = DateTime.Now;
        Area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        Area.User = nick;

        // ���� ���� �׻� ���ο� �ð� ����
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        Area.TimeText.text = (t.Hour > 12 ? "���� " : "���� ") + hour + ":" + t.Minute.ToString("D2");

        // ���� �Ͱ� ������ ���� �ð�, ���� ���ֱ�
        bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
        if (isSame) LastArea.TimeText.text = "";


        // Ÿ���� ���� �Ͱ� ������ �̹���, �̸� ���ֱ�
        if (!isSend)
        {
            Area.UserImage.gameObject.SetActive(!isSame);
            Area.UserText.gameObject.SetActive(!isSame);
            Area.UserText.text = Area.User;

            //������ �̸��� �־����
            UserImage(nick);
        }

        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(trContent);
        LastArea = Area;

        // ��ũ�ѹٰ� ���� �ö� ���¿��� �޽����� ������ �� �Ʒ��� ������ ����
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);

        #endregion


        // ê�� ������ ��´�
        chatBot = chatText;

        //3.������ ������Ʈ�� s�� ����
        //chat.SetText(chatText);

        //Json ������ -> List�� ���
        chatList.Add(info);

        // 5�� �̻��� �ȴٸ� Json������
        if (chatList.Count >= 6)
        {
            ChatInfoList chatInfoList = new ChatInfoList();
            //���� �г��� �־����
            chatInfoList.opponentName = findPlayer();
            chatInfoList.data = chatList;

            //Json �������� ���� ������ �� -> �̻ڰ� ������ ���� true
            jsonData = JsonUtility.ToJson(chatInfoList, true);
            print(jsonData);

            chatList.Clear();

            //[�����ؾ���] API ����Ʈ ��� -> �ٵ� Http������� ������
            OnPost(jsonData);
        }

        //��ũ�� �� ��� ������ �ڵ�� ����
        StartCoroutine(AutoScrollBottom());
    }

    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    void ScrollDelay() => scrollBar.value = 0;

    void ScrollDelayAI() => AIscrollBar.value = 0;


    void AIchat(string aichatText)
    {
        //0. �ٲ�� ���� Content H���� ����
        prevContentH =aiContent.sizeDelta.y;

        if (aichatText.Trim() == "") return;

        bool isBottom = AIscrollBar.value <= 0.00001f;

        //1. ChatItem�� �����(�θ� Scorllview�� Content)
        itemAI = Instantiate(chatItemAIFactory, aiContent);
        //print("�ֱ�");

        #region ä�� ���� ����
        AIArea = itemAI.GetComponent<ChatAreaScript>();
        AIArea.transform.SetParent(aiContent.transform, false);
        ////���� �ִ� 600, ���̴� ����
        AIArea.BoxRect.sizeDelta = new Vector2(500, AIArea.BoxRect.sizeDelta.y);
        ////���� ����
        AIArea.TextRect.GetComponent<Text>().text = aichatText;
        Fit(AIArea.BoxRect);

        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭, �� ���� �Ʒ��� �������� �ٷ� �� ũ�⸦ ���� 
        float X = AIArea.TextRect.sizeDelta.x + 42;
        float Y = AIArea.TextRect.sizeDelta.y;
        
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                AIArea.BoxRect.sizeDelta = new Vector2(X - i * 2, AIArea.BoxRect.sizeDelta.y);
                Fit(AIArea.BoxRect);

                if (Y != AIArea.TextRect.sizeDelta.y) { AIArea.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }
        else AIArea.BoxRect.sizeDelta = new Vector2(X, Y);

        // ���� �Ϳ� �б��� ������ ��¥�� �����̸� ����
        DateTime t = DateTime.Now;
        AIArea.Time = t.ToString("yyyy-MM-dd-HH-mm");
        //Area.User = nick;

        // ���� ���� �׻� ���ο� �ð� ����
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        AIArea.TimeText.text = (t.Hour > 12 ? "���� " : "���� ") + hour + ":" + t.Minute.ToString("D2");

        // ���� �Ͱ� ������ ���� �ð�, ���� ���ֱ�
        bool isSame = LastArea != null && LastArea.Time == AIArea.Time && LastArea.User == AIArea.User;
        if (isSame) LastArea.TimeText.text = "";


        // Ÿ���� ���� �Ͱ� ������ �̹���, �̸� ���ֱ�
        //if (!isSend)
        //{
        //    Area.UserImage.gameObject.SetActive(!isSame);
        //    Area.UserText.gameObject.SetActive(!isSame);
        //    Area.UserText.text = Area.User;

        //    //������ �̸��� �־����
        //    //UserImage(nick);
        //}

        Fit(AIArea.BoxRect);
        Fit(AIArea.AreaRect);
        Fit(aiContent);
        AILastArea = AIArea;

        // ��ũ�ѹٰ� ���� �ö� ���¿��� �޽����� ������ �� �Ʒ��� ������ ����
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelayAI", 0.03f);

        #endregion

        //2.���� ChatItem���� ChatItem ������Ʈ �����´�
        ChatItem chat = itemAI.GetComponent<ChatItem>();

        // ê�� ������ ��´�
        chatBot = aichatText;

        //3.������ ������Ʈ�� s�� ����
        //chat.SetText(aichatText);

        StartCoroutine(AIAutoScrollBottom());
    }

    //Ai
    // ���� ���� �� -> ê�� ������ ����
    // ������ �Խù� ��ȸ ��û -> HttpManager���� �˷��ַ��� ��
    public void OnGetPost(string s)
    {
        #region Get
        //string url = "https://da8f-119-194-163-123.jp.ngrok.io/chat_bot?chat_request=";
        //url += s;
        //url += "&user_id=" + 1;
        //url += "&we_id=" + 1;

        ////���� -> ������ ��ȸ -> ���� �־��� 
        //HttpRequester requester = new HttpRequester();
        //requester.SetUrl(RequestType.GET, url, false);
        //requester.isChat = true;

        //requester.onComplete = OnGetPostComplete;
        ////���ٽ�
        ////requester.onComplete = (DownloadHandler result) => { 
        ////};

        //HttpManager.instance.SendRequest(requester);
        #endregion

        #region Post
        string url = "/chat/";
        url += HttpManager.instance.username;
        url += "/chat_bot";

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = true;

        requester.onComplete = OnGetPostComplete;
        requester.onFailed = OnGetPostFailed;

        HttpManager.instance.SendRequest(requester);
        #endregion

    }

    void OnGetPostComplete(DownloadHandler result)
    {
        print("ê�� ������!!");
        //1.url�ּҿ� �ִ� �� print
        print(result.text);

        HttpChatBotData chatBotData = new HttpChatBotData();

        //downloadHandler�� �޾ƿ� Json���� ������ �����ϱ�
        //2.FromJson���� ���� �ٲ��ֱ�
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);
        chatBotData = JsonUtility.FromJson<HttpChatBotData>(result.text);

        //-----------------ê�� �ֱ�--------------

        if (chatBotData.results.body.response.Trim() == "") return;

        bool isBottom = AIscrollBar.value <= 0.00001f;

        //0. �ٲ�� ���� Content H���� ����
        prevContentH = aiContent.sizeDelta.y;

        //1. ChatItem�� �����(�θ� Scorllview�� Content)
        itemAIChat = Instantiate(chatItemAIOtherFactory, aiContent);

        #region ä�� ���� ����
        AIArea = itemAIChat.GetComponent<ChatAreaScript>();
        AIArea.transform.SetParent(aiContent.transform, false);
        ////���� �ִ� 600, ���̴� ����
        AIArea.BoxRect.sizeDelta = new Vector2(400, AIArea.BoxRect.sizeDelta.y);
        ////���� ����
        AIArea.TextRect.GetComponent<Text>().text = chatBotData.results.body.response;
        Fit(AIArea.BoxRect);

        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭, �� ���� �Ʒ��� �������� �ٷ� �� ũ�⸦ ���� 
        float X = AIArea.TextRect.sizeDelta.x + 42;
        float Y = AIArea.TextRect.sizeDelta.y;
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                AIArea.BoxRect.sizeDelta = new Vector2(X - i * 2, AIArea.BoxRect.sizeDelta.y);
                Fit(AIArea.BoxRect);

                if (Y != AIArea.TextRect.sizeDelta.y) { AIArea.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }
        else AIArea.BoxRect.sizeDelta = new Vector2(X, Y);

        // ���� �Ϳ� �б��� ������ ��¥�� �����̸� ����
        DateTime t = DateTime.Now;
        AIArea.Time = t.ToString("yyyy-MM-dd-HH-mm");

        string nick = "ȸ��";

        AIArea.User = nick;

        // ���� ���� �׻� ���ο� �ð� ����
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        AIArea.TimeText.text = (t.Hour > 12 ? "���� " : "���� ") + hour + ":" + t.Minute.ToString("D2");

        // ���� �Ͱ� ������ ���� �ð�, ���� ���ֱ�
        bool isSame = LastArea != null && LastArea.Time == AIArea.Time && LastArea.User == AIArea.User;
        if (isSame) LastArea.TimeText.text = "";

        AIArea.UserText.text = AIArea.User;

        //������ �̸��� �־����
        AIUserImage(nick);

        // Ÿ���� ���� �Ͱ� ������ �̹���, �̸� ���ֱ�
        //if (!isSend)
        //{
        //    AIArea.UserImage.gameObject.SetActive(!isSame);
        //    AIArea.UserText.gameObject.SetActive(!isSame);
        //    AIArea.UserText.text = AIArea.User;

        //    //������ �̸��� �־����
        //    AIUserImage(nick);
        //}

        Fit(AIArea.BoxRect);
        Fit(AIArea.AreaRect);
        Fit(aiContent);
        AILastArea = AIArea;

        // ��ũ�ѹٰ� ���� �ö� ���¿��� �޽����� ������ �� �Ʒ��� ������ ����
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelayAI", 0.03f);

        #endregion

        //2.���� ChatItem���� ChatItem ������Ʈ �����´�
        ChatItem chat = itemAIChat.GetComponent<ChatItem>();

        //3.������ ������Ʈ�� s�� ����
        //chat.SetText("����AI : " + chatBotData.results.body.response);

        // ��ũ�� �� ������
        StartCoroutine(AIAutoScrollBottom());


    }
    void OnGetPostFailed()
    {
        print("ê�� ���� ��!!");
    }
    IEnumerator AutoScrollBottom()
    {
        yield return null;

        //trScrollView H ���� Content H ���� Ŀ����(��ũ�� ���ɻ���)
        if (trContent.sizeDelta.y > trScrollView.sizeDelta.y)
        {
            //4. Content�� �ٴڿ� ����־��ٸ�
            if (trContent.anchoredPosition.y >= prevContentH - trScrollView.sizeDelta.y)
            {
                //5. Content�� y���� �ٽ� ����������
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trScrollView.sizeDelta.y);
            }
        }
    }

    IEnumerator AIAutoScrollBottom()
    {
        yield return null;

        //trScrollView H ���� Content H ���� Ŀ����(��ũ�� ���ɻ���)
        if (aiContent.sizeDelta.y > AIScrollView.sizeDelta.y)
        {
            //4. Content�� �ٴڿ� ����־��ٸ�
            if (aiContent.anchoredPosition.y >= prevContentH - AIScrollView.sizeDelta.y)
            {
                //5. Content�� y���� �ٽ� ����������
                aiContent.anchoredPosition = new Vector2(0, aiContent.sizeDelta.y - AIScrollView.sizeDelta.y);
            }
        }
    }


    //��Ʈ��ũ
    public void OnPost(string s)
    {
        string url = "/chat/";
        url += HttpManager.instance.username;

        //���� -> ������ ��ȸ -> ���� �־��� 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = true;

        requester.onComplete = OnPostComplete;
        requester.onFailed = OnGetFailed;

        HttpManager.instance.SendRequest(requester);
    }

    void OnPostComplete(DownloadHandler result)
    {
        print(result.text);
        print("ä�� �α� ������ ����");
    }

    void OnGetFailed()
    {
        print("ä�� �α� ������ ����!");
    }

    //�濡 �÷��̾ ���� ���� �� ȣ�����ִ� �Լ� -> GameManager���� ����
    public void AddPlayer(string add)
    {
        bool isBottom = scrollBar.value <= 0.00001f;

        //0. �ٲ�� ���� Content H���� ����
        prevContentH = trContent.sizeDelta.y;

        //1. ChatItem�� �����(�θ� Scorllview�� Content)
        item = Instantiate(chatItemRoomFactory, trContent);

        #region ä�� ���� ����
        ChatAreaScript Area = item.GetComponent<ChatAreaScript>();
        Area.transform.SetParent(trContent.transform, false);
        ////���� �ִ� 600, ���̴� ����
        //Area.BoxRect.sizeDelta = new Vector2(300, Area.BoxRect.sizeDelta.y);
        ////���� ����
        Area.TextRect.GetComponent<Text>().text = add;
        Fit(Area.BoxRect);
        #endregion

        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(trContent);
        LastArea = Area;

        // ��ũ�ѹٰ� ���� �ö� ���¿��� �޽����� ������ �� �Ʒ��� ������ ����
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);

        //2.���� ChatItem���� ChatItem ������Ʈ �����´�
        //ChatItem chat = item.GetComponent<ChatItem>();

        ////3.������ ������Ʈ�� s�� ����
        //chat.SetText(add);

        // ��ũ�� �� ������
        StartCoroutine(AutoScrollBottom());
    }

    // Chat �˾�â ����
    public void OnOpenPopUp()
    {
        anim.SetTrigger("open");

        //mainPopup.SetActive(true);
        //chatPopup.SetActive(false);
    }

    public void OnQuitPopUp()
    {
        anim.SetTrigger("close");

        //chatPopup.SetActive(true);
        //testChatList.text = "";
    }

    public void OnAiOpenPopUp()
    {

        aiAnim.SetTrigger("open");

        //MouseUI_LHS.instance.isChat = true;

    }

    public void OnAiQuitPopUp()
    {

        aiAnim.SetTrigger("close");

        //MouseUI_LHS.instance.isChat = false;
    }

    public int butCount = 0;

    public void OnVoicePopUp()
    {
        voicePopup.SetActive(true);
        
        butCount += 1;

        voiceAnim.SetBool("isOpen", true);

        // isClose�� �ƴ϶�� �ݱ�
        if (butCount >= 2)
        {
            voiceAnim.SetBool("isClose", true);

            butCount = 0;
        }
    }

    public void OnVoiceQuitPopUp()
    {
        aiAnim.SetTrigger("close");

        MouseUI_LHS.instance.isChat = true;
    }

    void UserImage(string userNick)
    {
        picture = Resources.Load<Texture2D>("ETC/" + userNick);

        // ����� �̹����� ��ü�ؼ� �ִ´� �̰� �� PC������ �ϸ� ��!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }

    void AIUserImage(string userNick)
    {
        picture = Resources.Load<Texture2D>("ETC/" + userNick);

        // ����� �̹����� ��ü�ؼ� �ִ´� �̰� �� PC������ �ϸ� ��!
        if (picture != null) AIArea.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
