using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System;

//채팅 로그 저장
//Json - 전체 보낼 데이터 
[System.Serializable]
public struct ChatInfoList
{
    public string opponentName;
    public List<ChatInfo> data;
}

//Json에 담길 내용 "키" : "값"
[System.Serializable]
public struct ChatInfo
{
    public string nickName;
    public string chatText;
}

//챗봇 대화
//Json에 담길 내용 "키" : "값"
[System.Serializable]
public struct AiChatInfo
{
    public string chatRequest;
    public string memberNickname;
    public string weNickname;
}

public class ChatManager : MonoBehaviourPun
{
    [Header("보이스 채팅 옵션")]
    public Button but_VoiceChat;
    
    [Header("일반 채팅")]
    //InputChat -> 사용자가 채팅한 내용
    public InputField inputChat;
    //ScorllView의 Content
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



    [Header("복원 채팅")]
    public InputField aiInputChat;
    //ScorllView의 Content
    public RectTransform aiContent;
    public GameObject chatItemAIFactory;
    public GameObject chatItemAIOtherFactory;

    [Header("Json")]
    //전체 보낼 데이터 생성
    public List<ChatInfo> chatList = new List<ChatInfo>();
    string chatBot;

    [Header("채팅 UI")]
    // MainChat 팝업창
    Animator anim;
    public GameObject mainPopup;
    Animator aiAnim;
    public GameObject aiPopup;
    // BtnChat 팝업창
    public GameObject chatPopup;
    //복원 버튼
    public GameObject btn_Restore;
    // 보이스 팝업창
    public GameObject voicePopup;
    public GameObject voiceChatPopup;
    Animator voiceAnim;

    //내 아이디 색
    Color idColor;
    Color aiColor;
    Color otherColor;

    //들어온 플레이 수
    int playerAll;

    //이전 Content의 H
    float prevContentH;

    //ScorllView의 RectTransform
    [Header("스크롤뷰")]
    public RectTransform trScrollView;
    public RectTransform AIScrollView;

    string jsonData;
    GameObject item;

    // 채팅창 구현을 위한 것이였음
    // 근데 잘못했어서 이렇게 변수 선언할 필요가 없는 거 같음
    // 코드 정리할 때 다시 해보기
    bool isSend;
    Texture2D picture;
    GameObject itemAI;
    GameObject itemAIChat;


    void Start()
    {
        //InputField에서 엔터를 쳤을 때 호출되는 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);
        inputChat.onValueChanged.AddListener(ValueChanged);


        aiInputChat.onSubmit.AddListener(OnSubmitAI);
        aiInputChat.onValueChanged.AddListener(AiValueChanged);

        //마우스 커서 비활성화
        //Cursor.visible = false;

        //idColor를 랜덤하게
        //idColor = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);

        //idColor = Color.yellow;
        //otherColor = Color.green;
        //aiColor = Color.blue;

        // Chat 팝업창
        anim = mainPopup.GetComponent<Animator>();
        aiAnim = aiPopup.GetComponent<Animator>();
        voiceAnim = voicePopup.GetComponent<Animator>();

        but_VoiceChat.onClick.AddListener(() => OnVoicePopUp());
    }

    void Update()
    {
        //현재 방에 들어와있는 인원수 // 혼자일때만 챗봇 가능하게!
        playerAll = PhotonNetwork.CurrentRoom.PlayerCount;

        //if (playerAll > 2)
        //{
        //    idColor = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
        //}

        #region 마우스 설정
        //만약에 esc키를 누르면 커서 활성화
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Cursor.visible = true;
        //}

        ////만약에 마우스 클릭을 하면 커서 비활성화
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //만약에 커서가 해당 위치에 UI가 없을때
        //    //IsPointerOverGameObject는 pointer가 UI에 있는 경우 True를 아닌 경우에는 false를 반환
        //    if (EventSystem.current.IsPointerOverGameObject() == false)
        //    {
        //        Cursor.visible = false;
        //    }
        //}
        #endregion

        //애니메이션이 끝나면 팝업창 닫기
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("close"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                mainPopup.SetActive(false);
            }
        }

        //애니메이션이 끝나면 팝업창 닫기
        if (voiceAnim.GetCurrentAnimatorStateInfo(0).IsName("close"))
        {
            if (voiceAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                voicePopup.SetActive(false);
            }
        }

        ////만약 0번을 누른다면 복원기능 활성화
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    chatPopup.GetComponent<Image>().enabled = false;
        //    chatPopup.GetComponent<Button>().enabled = false;

        //    voiceChatPopup.SetActive(false);

        //    btn_Restore.SetActive(true);
        //}
    }

    // 상대방 플레이어 이름 알기 위해서
    // 들어온 순서대로 이름을 리스트에 담기?
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
    void ValueChanged(string text) // string 매개변수는 기본으로 들어가는 매개변수이다
    {
        //if (photonView.IsMine == false) return;

        //Debug.Log(text + " - 글자 입력 중");

        //PlayerMove_LHS.instance.isMove = false;


        //UiController_LHS.instance.ismic = false;
    }

    //InputField에서 엔터를 쳤을때 호출되는 함수
    void OnSubmit(string s)
    {
        //UiController_LHS.instance.ismic = true;

        //PlayerMove_LHS.instance.isMove = true;

        //아무것도 있지 않을 때
        //if (s.Trim() != "")
        //{

        //}
        
        //이미지를 어떻게 해야할까 ?

        photonView.RPC("RpcAddChat", RpcTarget.All, PhotonNetwork.NickName, s);

        // 조건문 
        //if (photonView.IsMine == true)
        //if (HttpManager.instance.nickname == photonView.Owner.NickName)
      
        //4. InputChat의 내용을 초기화
        inputChat.text = "";

        //5. InputChat에 Focusing 을 해주자.
        //Enter 를 눌러도 InputField 가 계속 활성화 되게 해주는 코드
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

        //<color=#FFFFFF>닉네임</color>
        //string aichatText = "<color=#" + ColorUtility.ToHtmlStringRGB(aiColor) + ">" + PhotonNetwork.NickName + "</color>" + " : " + s;

        //AIchat(aichatText);

        AIchat(s);

        //4. InputChat의 내용을 초기화
        aiInputChat.text = "";
        //print("초기화시키자");

        //5. InputChat에 Focusing 을 해주자.
        //Enter 를 눌러도 InputField 가 계속 활성화 되게 해주는 코드
        aiInputChat.ActivateInputField();

        AiChatInfo aiInfo = new AiChatInfo();

        aiInfo.chatRequest = s;
        aiInfo.memberNickname = "남경";
        aiInfo.weNickname = "회은";
        //aiInfo.memberId = photonView.Owner.NickName;
        //aiInfo.weId = findPlayer();

        //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
        string aiJsonData = JsonUtility.ToJson(aiInfo, true);
        print(aiJsonData);


        HttpManager.instance.isAichat = false;
        //AI와 채팅을 한다!
        OnGetPost(aiJsonData);
    }

    void OnSubmitAIBool(string s)
    {
        //PlayerMove_LHS.instance.isMove = false;
    }

    [PunRPC]
    void RpcAddChat(string nick, string chatText)
    {
        //보낼 데이터 생성
        ChatInfo info = new ChatInfo();
        info.nickName = nick;
        info.chatText = chatText;

        //<color=#FFFFFF>닉네임</color>
        //string s = "<color=#" + ColorUtility.ToHtmlStringRGB(new Color(r, g, b)) + ">" + nick + "</color>" + " : " + chatText;

        //0. 바뀌기 전의 Content H값을 넣자
        prevContentH = trContent.sizeDelta.y;

        if (chatText.Trim() == "") return;

        bool isBottom = scrollBar.value <= 0.00001f;

        // 이름이 나와 같다면 chatItemFactory를 가져오고
        if (HttpManager.instance.nickname == nick)
        {
            //print("이름 같음");
            //item = Instantiate(chatItemFactory, trContent);
            item = Instantiate(chatItemMeFactory, trContent);

            isSend = true;
        }

        // 나와 이름이 다르다면 chatItemOtherFactory를 가져온다
        // 방에 들어온 사람들의 캐릭터 이미지를 가져온다
        // 닉네임이 채팅친 사람과 같을 때
        // 저장된 이미지 중 같은 이름의 이미지를 가져와서 넣는다.
        else if(nick == "남경")
        {
            //print("남경이 일 때");
            //item = Instantiate(chatItemOtherFactory, trContent);
            item = Instantiate(chatItemOtherFactory, trContent);

            isSend = false;
        }

        //Test용
        else
        {
            //print("이름 다름");
            //item = Instantiate(chatItemOtherFactory, trContent);
            item = Instantiate(chatItemOtherFactory, trContent);

            isSend = false;
        }


        //2.만든 ChatItem에서 ChatItem 컴포넌트 가져온다
        //ChatItem chat = item.GetComponent<ChatItem>();

        #region 채팅 셋팅 관련
        Area = item.GetComponent<ChatAreaScript>();
        Area.transform.SetParent(trContent.transform, false);
        ////가로 최대 600, 높이는 한줄
        Area.BoxRect.sizeDelta = new Vector2(500, Area.BoxRect.sizeDelta.y);
        ////글자 쓰는
        Area.TextRect.GetComponent<Text>().text = chatText;
        Fit(Area.BoxRect);

        // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입 
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

        // 현재 것에 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        Area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        Area.User = nick;

        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        Area.TimeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");

        // 이전 것과 같으면 이전 시간, 꼬리 없애기
        bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
        if (isSame) LastArea.TimeText.text = "";


        // 타인이 이전 것과 같으면 이미지, 이름 없애기
        if (!isSend)
        {
            Area.UserImage.gameObject.SetActive(!isSame);
            Area.UserText.gameObject.SetActive(!isSame);
            Area.UserText.text = Area.User;

            //상대방의 이름을 넣어야함
            UserImage(nick);
        }

        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(trContent);
        LastArea = Area;

        // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);

        #endregion


        // 챗봇 내용을 담는다
        chatBot = chatText;

        //3.가져온 컴포넌트에 s를 셋팅
        //chat.SetText(chatText);

        //Json 보내기 -> List에 담기
        chatList.Add(info);

        // 5개 이상이 된다면 Json보내기
        if (chatList.Count >= 6)
        {
            ChatInfoList chatInfoList = new ChatInfoList();
            //상대방 닉네임 넣어야함
            chatInfoList.opponentName = findPlayer();
            chatInfoList.data = chatList;

            //Json 형식으로 값이 들어가지게 됨 -> 이쁘게 나오기 위해 true
            jsonData = JsonUtility.ToJson(chatInfoList, true);
            print(jsonData);

            chatList.Clear();

            //[설정해야함] API 포스트 방식 -> 바디 Http통신으로 보내기
            OnPost(jsonData);
        }

        //스크롤 바 계속 내리기 코드로 구현
        StartCoroutine(AutoScrollBottom());
    }

    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    void ScrollDelay() => scrollBar.value = 0;

    void ScrollDelayAI() => AIscrollBar.value = 0;


    void AIchat(string aichatText)
    {
        //0. 바뀌기 전의 Content H값을 넣자
        prevContentH =aiContent.sizeDelta.y;

        if (aichatText.Trim() == "") return;

        bool isBottom = AIscrollBar.value <= 0.00001f;

        //1. ChatItem을 만든다(부모를 Scorllview의 Content)
        itemAI = Instantiate(chatItemAIFactory, aiContent);
        //print("넣기");

        #region 채팅 셋팅 관련
        AIArea = itemAI.GetComponent<ChatAreaScript>();
        AIArea.transform.SetParent(aiContent.transform, false);
        ////가로 최대 600, 높이는 한줄
        AIArea.BoxRect.sizeDelta = new Vector2(500, AIArea.BoxRect.sizeDelta.y);
        ////글자 쓰는
        AIArea.TextRect.GetComponent<Text>().text = aichatText;
        Fit(AIArea.BoxRect);

        // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입 
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

        // 현재 것에 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        AIArea.Time = t.ToString("yyyy-MM-dd-HH-mm");
        //Area.User = nick;

        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        AIArea.TimeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");

        // 이전 것과 같으면 이전 시간, 꼬리 없애기
        bool isSame = LastArea != null && LastArea.Time == AIArea.Time && LastArea.User == AIArea.User;
        if (isSame) LastArea.TimeText.text = "";


        // 타인이 이전 것과 같으면 이미지, 이름 없애기
        //if (!isSend)
        //{
        //    Area.UserImage.gameObject.SetActive(!isSame);
        //    Area.UserText.gameObject.SetActive(!isSame);
        //    Area.UserText.text = Area.User;

        //    //상대방의 이름을 넣어야함
        //    //UserImage(nick);
        //}

        Fit(AIArea.BoxRect);
        Fit(AIArea.AreaRect);
        Fit(aiContent);
        AILastArea = AIArea;

        // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelayAI", 0.03f);

        #endregion

        //2.만든 ChatItem에서 ChatItem 컴포넌트 가져온다
        ChatItem chat = itemAI.GetComponent<ChatItem>();

        // 챗봇 내용을 담는다
        chatBot = aichatText;

        //3.가져온 컴포넌트에 s를 셋팅
        //chat.SetText(aichatText);

        StartCoroutine(AIAutoScrollBottom());
    }

    //Ai
    // 엔터 쳤을 때 -> 챗봇 보내는 내용
    // 서버에 게시물 조회 요청 -> HttpManager한테 알려주려고 함
    public void OnGetPost(string s)
    {
        #region Get
        //string url = "https://da8f-119-194-163-123.jp.ngrok.io/chat_bot?chat_request=";
        //url += s;
        //url += "&user_id=" + 1;
        //url += "&we_id=" + 1;

        ////생성 -> 데이터 조회 -> 값을 넣어줌 
        //HttpRequester requester = new HttpRequester();
        //requester.SetUrl(RequestType.GET, url, false);
        //requester.isChat = true;

        //requester.onComplete = OnGetPostComplete;
        ////람다식
        ////requester.onComplete = (DownloadHandler result) => { 
        ////};

        //HttpManager.instance.SendRequest(requester);
        #endregion

        #region Post
        string url = "/chat/";
        url += HttpManager.instance.username;
        url += "/chat_bot";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
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
        print("챗봇 성공함!!");
        //1.url주소에 있던 거 print
        print(result.text);

        HttpChatBotData chatBotData = new HttpChatBotData();

        //downloadHandler에 받아온 Json형식 데이터 가공하기
        //2.FromJson으로 형식 바꿔주기
        //ChatData chatData = JsonUtility.FromJson<ChatData>(result.text);
        chatBotData = JsonUtility.FromJson<HttpChatBotData>(result.text);

        //-----------------챗봇 넣기--------------

        if (chatBotData.results.body.response.Trim() == "") return;

        bool isBottom = AIscrollBar.value <= 0.00001f;

        //0. 바뀌기 전의 Content H값을 넣자
        prevContentH = aiContent.sizeDelta.y;

        //1. ChatItem을 만든다(부모를 Scorllview의 Content)
        itemAIChat = Instantiate(chatItemAIOtherFactory, aiContent);

        #region 채팅 셋팅 관련
        AIArea = itemAIChat.GetComponent<ChatAreaScript>();
        AIArea.transform.SetParent(aiContent.transform, false);
        ////가로 최대 600, 높이는 한줄
        AIArea.BoxRect.sizeDelta = new Vector2(400, AIArea.BoxRect.sizeDelta.y);
        ////글자 쓰는
        AIArea.TextRect.GetComponent<Text>().text = chatBotData.results.body.response;
        Fit(AIArea.BoxRect);

        // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입 
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

        // 현재 것에 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        AIArea.Time = t.ToString("yyyy-MM-dd-HH-mm");

        string nick = "회은";

        AIArea.User = nick;

        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        AIArea.TimeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");

        // 이전 것과 같으면 이전 시간, 꼬리 없애기
        bool isSame = LastArea != null && LastArea.Time == AIArea.Time && LastArea.User == AIArea.User;
        if (isSame) LastArea.TimeText.text = "";

        AIArea.UserText.text = AIArea.User;

        //상대방의 이름을 넣어야함
        AIUserImage(nick);

        // 타인이 이전 것과 같으면 이미지, 이름 없애기
        //if (!isSend)
        //{
        //    AIArea.UserImage.gameObject.SetActive(!isSame);
        //    AIArea.UserText.gameObject.SetActive(!isSame);
        //    AIArea.UserText.text = AIArea.User;

        //    //상대방의 이름을 넣어야함
        //    AIUserImage(nick);
        //}

        Fit(AIArea.BoxRect);
        Fit(AIArea.AreaRect);
        Fit(aiContent);
        AILastArea = AIArea;

        // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelayAI", 0.03f);

        #endregion

        //2.만든 ChatItem에서 ChatItem 컴포넌트 가져온다
        ChatItem chat = itemAIChat.GetComponent<ChatItem>();

        //3.가져온 컴포넌트에 s를 셋팅
        //chat.SetText("복원AI : " + chatBotData.results.body.response);

        // 스크롤 뷰 내리기
        StartCoroutine(AIAutoScrollBottom());


    }
    void OnGetPostFailed()
    {
        print("챗봇 성공 놉!!");
    }
    IEnumerator AutoScrollBottom()
    {
        yield return null;

        //trScrollView H 보다 Content H 값이 커지면(스크롤 가능상태)
        if (trContent.sizeDelta.y > trScrollView.sizeDelta.y)
        {
            //4. Content가 바닥에 닿아있었다면
            if (trContent.anchoredPosition.y >= prevContentH - trScrollView.sizeDelta.y)
            {
                //5. Content의 y값을 다시 설정해주자
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trScrollView.sizeDelta.y);
            }
        }
    }

    IEnumerator AIAutoScrollBottom()
    {
        yield return null;

        //trScrollView H 보다 Content H 값이 커지면(스크롤 가능상태)
        if (aiContent.sizeDelta.y > AIScrollView.sizeDelta.y)
        {
            //4. Content가 바닥에 닿아있었다면
            if (aiContent.anchoredPosition.y >= prevContentH - AIScrollView.sizeDelta.y)
            {
                //5. Content의 y값을 다시 설정해주자
                aiContent.anchoredPosition = new Vector2(0, aiContent.sizeDelta.y - AIScrollView.sizeDelta.y);
            }
        }
    }


    //네트워크
    public void OnPost(string s)
    {
        string url = "/chat/";
        url += HttpManager.instance.username;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
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
        print("채팅 로그 보내기 성공");
    }

    void OnGetFailed()
    {
        print("채팅 로그 보내기 실패!");
    }

    //방에 플레이어가 참여 했을 때 호출해주는 함수 -> GameManager에서 실행
    public void AddPlayer(string add)
    {
        bool isBottom = scrollBar.value <= 0.00001f;

        //0. 바뀌기 전의 Content H값을 넣자
        prevContentH = trContent.sizeDelta.y;

        //1. ChatItem을 만든다(부모를 Scorllview의 Content)
        item = Instantiate(chatItemRoomFactory, trContent);

        #region 채팅 셋팅 관련
        ChatAreaScript Area = item.GetComponent<ChatAreaScript>();
        Area.transform.SetParent(trContent.transform, false);
        ////가로 최대 600, 높이는 한줄
        //Area.BoxRect.sizeDelta = new Vector2(300, Area.BoxRect.sizeDelta.y);
        ////글자 쓰는
        Area.TextRect.GetComponent<Text>().text = add;
        Fit(Area.BoxRect);
        #endregion

        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(trContent);
        LastArea = Area;

        // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);

        //2.만든 ChatItem에서 ChatItem 컴포넌트 가져온다
        //ChatItem chat = item.GetComponent<ChatItem>();

        ////3.가져온 컴포넌트에 s를 셋팅
        //chat.SetText(add);

        // 스크롤 뷰 내리기
        StartCoroutine(AutoScrollBottom());
    }

    // Chat 팝업창 설정
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

        // isClose가 아니라면 닫기
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

        // 사용자 이미지로 대체해서 넣는다 이건 내 PC에서만 하면 됨!
        if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }

    void AIUserImage(string userNick)
    {
        picture = Resources.Load<Texture2D>("ETC/" + userNick);

        // 사용자 이미지로 대체해서 넣는다 이건 내 PC에서만 하면 됨!
        if (picture != null) AIArea.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
    }
}
