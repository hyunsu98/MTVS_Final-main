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

    //�� ����Ʈ Content
    public Transform trListContent;
    public GameObject roomItemFactory;

    // �� ���� ����� UI
    public InputField roomNameField;
    public InputField roomDescField;

    public Image thumbs;
    public Sprite defaultThumbs;


    //���� ������   
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

        //���� -> ������ ��ȸ -> ���� �־��� 
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

        //���� -> ������ ��ȸ -> ���� �־��� 
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

        //���� -> ������ ��ȸ -> ���� �־��� 
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

        //���� -> ������ ��ȸ -> ���� �־��� 
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
        print("�� ���� ������ ����");
        SaveJson();


        CreateRoomItem();
    }

    void OnPostFailed()
    {
        print("�� ���� ������ ����!");
    }

    void OnGetComplete(DownloadHandler result)
    {
        print("�� ���� �ҷ����� ����");

        RoomInfoResult myRoomInfoResult = JsonUtility.FromJson<RoomInfoResult>(result.text);
        Debug.Log(myRoomInfoResult.results.room.id);
        room = myRoomInfoResult.results.room;

        RoomManager.instance.room = room;

        CreateRoom();
    }

    void OnGetFailed()
    {
        print("�� ���� �ҷ����� ����");
    }

    void OnGetListComplete(DownloadHandler result)
    {
        print("�� ���� ����Ʈ �ҷ����� ����");
        Debug.Log(result.text);
        ArrayRoomInfo arrayJson = JsonUtility.FromJson<ArrayRoomInfo>(result.text);

        for (int i = 0; i < arrayJson.roomList.Count; ++i)
        {
            RoomInfoLoadJson info = arrayJson.roomList[i];
            // Load ����� �ؾߵ�
            LoadRoomItem(info.memberNickname, info.roomName, info.roomLikes, info.roomViews, info.id);
        }


        LobbyUIManager.instance.ChangeUIList("Popular");


    }

    void OnGetListFailed()
    {
        print("�� ���� ����Ʈ �ҷ����� ����");
    }

    //�� ����
    public void CreateRoom()
    {
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        roomOptions.MaxPlayers = 10;
        // �� ����Ʈ�� ������ �ʰ�? ���̰�?
        roomOptions.IsVisible = true;

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["room_id"] = room.id;
        hash["room_name"] = room.roomName;
        //hash["description"] = "";
        hash["like"] = room.roomLikes;
        hash["view"] = room.roomViews;

        // custom ������ �����ϴ� ����
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
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        roomOptions.MaxPlayers = 10;
        // �� ����Ʈ�� ������ �ʰ�? ���̰�?
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(_roonName, roomOptions, TypedLobby.Default);
    }

    //���� �����Ǹ� ȣ�� �Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    //�� ������ ���� �ɶ� ȣ�� �Ǵ� �Լ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed , " + returnCode + ", " + message);

        JoinRoom(room.roomName);
    }

    //�� ���� ��û
    public void JoinRoom(string _roomName)
    {
        PhotonNetwork.JoinRoom(_roomName);
    }

    //�� ������ �Ϸ� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
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

    //�� ������ ���� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
    }

    //�濡 ���� ������ ����Ǹ� ȣ�� �Ǵ� �Լ�(�߰�/����/����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //�븮��Ʈ UI �� ��ü����
        //DeleteRoomListUI();
        ////�븮��Ʈ ������ ������Ʈ
        //UpdateRoomCache(roomList);
        ////�븮��Ʈ UI ��ü ����
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
            // ����, ����
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                //���࿡ �ش� ���� �����Ȱ��̶��
                if (roomList[i].RemovedFromList)
                {
                    //roomCache ���� �ش� ������ ����
                    roomCache.Remove(roomList[i].Name);
                }
                //�׷��� �ʴٸ�
                else
                {
                    //���� ����
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
            //�߰�
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
            //������� �����.
            GameObject go = Instantiate(roomItemFactory, trListContent);
            //������� ������ ����(������(0/0))
            MyRoomInfo myRoonInfo = go.GetComponent<MyRoomInfo>();
            myRoonInfo.SetInfo(info);
        }
    }
}
