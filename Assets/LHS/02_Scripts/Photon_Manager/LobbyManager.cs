using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text nickName;
    
    //���� �����ؾ��� �׽�Ʈ����

    //���̸� InputField
    InputField inputRoomName;
    //��й�ȣ InputField
    InputField inputPassword;
    //���ο� InputField
    InputField inputMaxPlayer;
    //������ Button
    Button btnJoin;
    //����� Button
    Button btnCreate;

    //���� ������   
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
    //�� ����Ʈ Content
    Transform trListContent;

    //map Thumbnail
    GameObject[] mapThumbs;

    void Start()
    {
        // ���̸�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        //inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // ���ο�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        //inputMaxPlayer.onValueChanged.AddListener(OnMaxPlayerValueChanged);

        //Ŀ���� â�� �г��� �� �� �ְ� ����
        // nickName.text = PhotonNetwork.NickName;

    }

    public void OnRoomNameValueChanged(string s)
    {
        //����
        btnJoin.interactable = s.Length > 0;
        //����
        btnCreate.interactable = s.Length > 0 && inputMaxPlayer.text.Length > 0;
    }

    public void OnMaxPlayerValueChanged(string s)
    {
        //����
        btnCreate.interactable = s.Length > 0 && inputRoomName.text.Length > 0;
    }


    //�� ����
    public void CreateRoom()
    {
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        //roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text);
        roomOptions.MaxPlayers = 10;
        // �� ����Ʈ�� ������ �ʰ�? ���̰�?
        roomOptions.IsVisible = true;

        // �� ���� ��û (�ش� �ɼ��� �̿��ؼ�)
        //PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOptions);
        PhotonNetwork.CreateRoom("XR_LHS", roomOptions, TypedLobby.Default);
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

        JoinRoom();
    }

    //�� ���� ��û
    public void JoinRoom()
    {

        PhotonNetwork.JoinRoom("XR_LHS");
    }

    //�� ������ �Ϸ� �Ǿ��� �� ȣ�� �Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");

        

        if(LobbyUIManager.instance.flag == "re")
        {
            PhotonNetwork.LoadLevel("RoomBaseScene");
        }
        else if(LobbyUIManager.instance.flag == "kks")
        {
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

    //=================
    //�� ����
    public void CreateRoomKKS()
    {
        // �� �ɼ��� ����
        RoomOptions roomOptions = new RoomOptions();
        // �ִ� �ο� (0�̸� �ִ��ο�)
        //roomOptions.MaxPlayers = byte.Parse(inputMaxPlayer.text);
        roomOptions.MaxPlayers = 10;
        // �� ����Ʈ�� ������ �ʰ�? ���̰�?
        roomOptions.IsVisible = true;

        // �� ���� ��û (�ش� �ɼ��� �̿��ؼ�)
        //PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOptions);
        PhotonNetwork.CreateRoom("XR_LHS", roomOptions, TypedLobby.Default);
        //PhotonNetwork.CreateRoom("KKSRoomScene", roomOptions, TypedLobby.Default);

    }


    //=================
    //�濡 ���� ������ ����Ǹ� ȣ�� �Ǵ� �Լ�(�߰�/����/����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        //�븮��Ʈ UI �� ��ü����
       // DeleteRoomListUI();
        //�븮��Ʈ ������ ������Ʈ
        //UpdateRoomCache(roomList);
        //�븮��Ʈ UI ��ü ���� -> �ּ�ó����
        //CreateRoomListUI();
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

        //for (int i = 0; i < roomList.Count; i++)
        //{
        //    // ����, ����
        //    if (roomCache.ContainsKey(roomList[i].Name))
        //    {
        //        //���࿡ �ش� ���� �����Ȱ��̶��
        //        if (roomList[i].RemovedFromList)
        //        {
        //            //roomCache ���� �ش� ������ ����
        //            roomCache.Remove(roomList[i].Name);
        //            continue;
        //        }                
        //    }
        //    roomCache[roomList[i].Name] = roomList[i];            
        //}
    }

    GameObject roomItemFactory;
    //void CreateRoomListUI()
    //{
    //    foreach (RoomInfo info in roomCache.Values)
    //    {
    //        //������� �����.
    //        GameObject go = Instantiate(roomItemFactory, trListContent);
    //        //������� ������ ����(������(0/0))
    //        RoomItem item = go.GetComponent<RoomItem>();
    //        item.SetInfo(info);

    //        //roomItem ��ư�� Ŭ���Ǹ� ȣ��Ǵ� �Լ� ���
    //        item.onClickAction = SetRoomName;
    //        //���ٽ�
    //        //item.onClickAction = (string room) => {
    //        //    inputRoomName.text = room;
    //        //};

    //        string desc = (string)info.CustomProperties["desc"];
    //        int map_id = (int)info.CustomProperties["map_id"];
    //        print(desc + ", " + map_id);
    //    }
    //}


    //���� Thumbnail id
    int prevMapId = -1;
    void SetRoomName(string room, int map_id)
    {
        //���̸� ����
        inputRoomName.text = room;

        //���࿡ ���� �� Thumbnail�� Ȱ��ȭ�� �Ǿ��ִٸ�
        if (prevMapId > -1)
        {
            //���� �� Thumbnail�� ��Ȱ��ȭ
            mapThumbs[prevMapId].SetActive(false);
        }

        //�� Thumbnail ����
        mapThumbs[map_id].SetActive(true);

        //���� �� id ����
        prevMapId = map_id;
    }
}
