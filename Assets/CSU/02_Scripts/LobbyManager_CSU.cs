using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.SceneManagement;

public class LobbyManager_CSU : MonoBehaviourPunCallbacks
{
    public string roomName;
    public string roomId;

    //룸 리스트 Content
    public Transform trListContent;
    public GameObject roomItemFactory;

    // 룸 정보 기억할 UI
    public InputField roomNameField;
    public InputField roomDescField;

    public Image thumbs;
    public Sprite defaultThumbs;


    //방의 정보들   
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    RoomInfoLoadJson room = new RoomInfoLoadJson();

    private void Awake()
    {
        RoomManager.instance.thumbsDic = new Dictionary<string, string>();

        string jsonData = File.ReadAllText(Application.streamingAssetsPath + "/Json/Sprites.txt");
        SpriteInfoList info = JsonUtility.FromJson<SpriteInfoList>(jsonData);

        for (int i = 0; i < info.sprites.Count; ++i)
        {
            SpriteInfo spriteInfo = info.sprites[i];
            RoomManager.instance.thumbsDic.Add(spriteInfo.roomName, spriteInfo.fileName);
        }
    }

    public void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LobbyScene")
        { 
            OnGetList();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            trListContent.GetComponent<GridLayoutGroup>().spacing = new Vector2(35f, 35f);
        }
    }

    void SaveJson()
    {
        SpriteInfo info;

        SpriteInfoList infoList = new SpriteInfoList();
        infoList.sprites = new List<SpriteInfo>();

        foreach(KeyValuePair<string, string> item in RoomManager.instance.thumbsDic)
        {
            info = new SpriteInfo();
            info.roomName = item.Key;
            info.fileName = item.Value;

            infoList.sprites.Add(info);
        }

        string jsonData = JsonUtility.ToJson(infoList, true);
        File.WriteAllText(Application.streamingAssetsPath + "/Json/Sprites.txt", jsonData);
    }

    public void InitUI()
    {
        roomNameField.text = "";
        roomDescField.text = "";
        thumbs.sprite = defaultThumbs;
    }

    void SetFlag(string s)
    {
        LobbyUIManager.instance.flag = s;
    }

    public void LoadRoomItem(string _nickname, string _roomName, int _like, int _view, int _id)
    {
        GameObject go = Instantiate(roomItemFactory, trListContent);
        MyRoomInfo myRoomInfo = go.GetComponent<MyRoomInfo>();
        myRoomInfo.SetInfo(_nickname, _roomName, _like, _view, _id);
        myRoomInfo.thumbNailButton.onClick.AddListener(() => SetRoomId(_id));
        myRoomInfo.thumbNailButton.onClick.AddListener(OnGet);

        LobbyUIManager.instance.roomInfoList.Add(myRoomInfo);
        LobbyUIManager.instance.roomInfoUI.Add(go);

        if(_id == 5)
        {
            myRoomInfo.thumbNailButton.onClick.AddListener(() => SetFlag("re"));
        }
        else if(_id == 2)
        {
            myRoomInfo.thumbNailButton.onClick.AddListener(() => SetFlag("kks"));
        }

        byte[] bytes = File.ReadAllBytes(Application.streamingAssetsPath + "/Sprite/" + RoomManager.instance.thumbsDic[_roomName] + ".png");
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
        myRoomInfo.thumbNail.sprite = sprite;
    }

    public void CreateRoomItem()
    {
        LoadRoomItem(HttpManager.instance.nickname, roomName, 0, 0, room.id);
    }

    public void SetRoomId(int idx)
    {
        roomId = idx.ToString();
        Debug.Log(roomId);
        LobbyUIManager.instance.flag = "";
    }

    public void MyCreateRoom()
    {

        RoomManager.instance.isFirst = true;
        RoomInfoSaveJson myRoomData = new RoomInfoSaveJson();
        myRoomData.username = HttpManager.instance.username;
        myRoomData.roomname = roomNameField.text;

        string jsonData = JsonUtility.ToJson(myRoomData, true);
        Debug.Log(jsonData);

        roomName = myRoomData.roomname;
        string file = RoomManager.instance.fileName.Substring(0, RoomManager.instance.fileName.Length - 4); ;
        RoomManager.instance.thumbsDic.Add(roomName, file);

        print(roomName + ", " + file);
        
        OnPost(jsonData);

        //SaveJson();
    }

    public void OnPost(string s)
    {
        string url = "/room";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();

        requester.SetUrl(RequestType.POST, url, true);
        requester.body = s;
        requester.isJson = true;
        requester.isChat = true;

        requester.onComplete = OnPostComplete;
        requester.onFailed = OnPostFailed;

        HttpManager.instance.SendRequest(requester);
    }

    public void OnGetTemp()
    {
        string url = "/room/withroomid?roomid=";
        url += roomId;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, true);
        requester.isChat = true;

        requester.onComplete = OnGetComplete;
        requester.onFailed = OnGetFailed;

        HttpManager.instance.SendRequest(requester);
    }

    public void OnGet()
    {
        string url = "/room/withroomid?roomid=";
        url += roomId;

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, true);
        requester.isChat = true;

        requester.onComplete = OnGetComplete;
        requester.onFailed = OnGetFailed;

        HttpManager.instance.SendRequest(requester);
    }

    public void OnGetList()
    {
        string url = "/room/roomlist";

        //생성 -> 데이터 조회 -> 값을 넣어줌 
        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, true);
        requester.isChat = true;

        requester.onComplete = OnGetListComplete;
        requester.onFailed = OnGetListFailed;

        HttpManager.instance.SendRequest(requester);
    }

    void OnPostComplete(DownloadHandler result)
    {
        print(result.text);
        print("방 정보 보내기 성공");
        SaveJson();


        CreateRoomItem();
    }

    void OnPostFailed()
    {
        print("방 정보 보내기 실패!");
    }

    void OnGetComplete(DownloadHandler result)
    {
        print("방 정보 불러오기 성공");

        RoomInfoResult myRoomInfoResult = JsonUtility.FromJson<RoomInfoResult>(result.text);
        Debug.Log(myRoomInfoResult.results.room.id);
        room = myRoomInfoResult.results.room;

        RoomManager.instance.room = room;

        CreateRoom();
    }

    void OnGetFailed()
    {
        print("방 정보 불러오기 실패");
    }

    void OnGetListComplete(DownloadHandler result)
    {
        print("방 정보 리스트 불러오기 성공");
        Debug.Log(result.text);
        ArrayRoomInfo arrayJson = JsonUtility.FromJson<ArrayRoomInfo>(result.text);

        for (int i = 0; i < arrayJson.roomList.Count; ++i)
        {
            RoomInfoLoadJson info = arrayJson.roomList[i];
            // Load 블라블라 해야됨
            LoadRoomItem(info.memberNickname, info.roomName, info.roomLikes, info.roomViews, info.id);
        }


        LobbyUIManager.instance.ChangeUIList("Popular");


    }

    void OnGetListFailed()
    {
        print("방 정보 리스트 불러오기 실패");
    }

    //방 생성
    public void CreateRoom()
    {
        // 방 옵션을 설정
        RoomOptions roomOptions = new RoomOptions();
        // 최대 인원 (0이면 최대인원)
        roomOptions.MaxPlayers = 10;
        // 룸 리스트에 보이지 않게? 보이게?
        roomOptions.IsVisible = true;

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_id"] = room.id;
        hash["room_name"] = room.roomName;
        //hash["description"] = "";
        hash["like"] = room.roomLikes;
        hash["view"] = room.roomViews;

        // custom 정보를 공개하는 설정
        roomOptions.CustomRoomPropertiesForLobby = new string[] {
            "room_id", "room_name", /*"description",*/ "like", "view"
        };

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "CharacterScene_LHS")
        {
            Debug.Log(RoomManager.instance.flag);
            if(RoomManager.instance.flag == "re")
            {
                room.roomName = "REmember";
            }
            else if(RoomManager.instance.flag == "kks")
            {
                room.roomName = "King_Kwang-seok";
            }
        }
        PhotonNetwork.CreateRoom(room.roomName, roomOptions, TypedLobby.Default);
    }

    public void CreateRoom(string _roonName)
    {
        // 방 옵션을 설정
        RoomOptions roomOptions = new RoomOptions();
        // 최대 인원 (0이면 최대인원)
        roomOptions.MaxPlayers = 10;
        // 룸 리스트에 보이지 않게? 보이게?
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(_roonName, roomOptions, TypedLobby.Default);
    }

    //방이 생성되면 호출 되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    //방 생성이 실패 될때 호출 되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed , " + returnCode + ", " + message);

        JoinRoom(room.roomName);
    }

    //방 참가 요청
    public void JoinRoom(string _roomName)
    {
        PhotonNetwork.JoinRoom(_roomName);
    }

    //방 참가가 완료 되었을 때 호출 되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");

        if (LobbyUIManager.instance.flag == "re")
        {
            roomName = "re";
            RoomManager.instance.flag = "re";
            PhotonNetwork.LoadLevel("REMainScene_LHS");
        }
        else if (LobbyUIManager.instance.flag == "kks")
        {
            roomName = "kks";
            RoomManager.instance.flag = "kks";
            PhotonNetwork.LoadLevel("KKSRoomScene");
        }
        else
        {
            PhotonNetwork.LoadLevel("RoomBaseScene");
        }
    }

    //방 참가가 실패 되었을 때 호출 되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }

    //방에 대한 정보가 변경되면 호출 되는 함수(추가/삭제/수정)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //룸리스트 UI 를 전체삭제
        //DeleteRoomListUI();
        ////룸리스트 정보를 업데이트
        //UpdateRoomCache(roomList);
        ////룸리스트 UI 전체 생성
        //CreateRoomListUI();
        Debug.Log("OnRoomListUpdate");

    }

    void DeleteRoomListUI()
    {
        foreach (Transform tr in trListContent)
        {
            Destroy(tr.gameObject);
        }
    }

    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // 수정, 삭제
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //만약에 해당 룸이 삭제된것이라면
                if (roomList[i].RemovedFromList)
                {
                    //roomCache 에서 해당 정보를 삭제
                    roomCache.Remove(roomList[i].Name);
                }
                //그렇지 않다면
                else
                {
                    //정보 수정
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
            //추가
            else
            {
                roomCache[roomList[i].Name] = roomList[i];
            }
        }
    }

    void CreateRoomListUI()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            //룸아이템 만든다.
            GameObject go = Instantiate(roomItemFactory, trListContent);
            //룸아이템 정보를 셋팅(방제목(0/0))
            MyRoomInfo myRoonInfo = go.GetComponent<MyRoomInfo>();
            myRoonInfo.SetInfo(info);
        }
    }
}
