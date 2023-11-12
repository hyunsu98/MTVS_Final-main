using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBtnManager_LHS : MonoBehaviourPunCallbacks
{
    [Header("ĳ���� ���� ��ư")]
    public Button but_Character;

    [Header("�� ���� ��ư")]
    public Button but_Room;

    void Start()
    {
        but_Character.onClick.AddListener(() => CharacterScene());

    }

    void Update()
    {

    }

    public void CharacterScene()
    {
        //�� ������
        PhotonNetwork.LeaveRoom();
        
       PhotonNetwork.LoadLevel("CharacterScene_LHS");
    }

    public void LobbyScene()
    {
        //�� ������
        PhotonNetwork.LeaveRoom();

        PhotonNetwork.LoadLevel("LobbyScene");
    }
    //    //�κ� ������
    //    PhotonNetwork.LeaveLobby();
    //    //�κ� ����
    //    PhotonNetwork.JoinLobby();
    //    print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    //}

    ////�κ� ���� ������ ȣ��
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();

    //    //LobbyScene���� �̵�
    //    PhotonNetwork.LoadLevel("CharacterScene_LHS");
    //}

    void RoomIcon()
    {

    }
}
