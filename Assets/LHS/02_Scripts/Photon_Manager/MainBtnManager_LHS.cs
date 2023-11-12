using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBtnManager_LHS : MonoBehaviourPunCallbacks
{
    [Header("캐릭터 변경 버튼")]
    public Button but_Character;

    [Header("방 정보 버튼")]
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
        //방 나가기
        PhotonNetwork.LeaveRoom();
        
       PhotonNetwork.LoadLevel("CharacterScene_LHS");
    }

    public void LobbyScene()
    {
        //방 나가기
        PhotonNetwork.LeaveRoom();

        PhotonNetwork.LoadLevel("LobbyScene");
    }
    //    //로비를 나가고
    //    PhotonNetwork.LeaveLobby();
    //    //로비 참가
    //    PhotonNetwork.JoinLobby();
    //    print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    //}

    ////로비 진입 성공시 호출
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();

    //    //LobbyScene으로 이동
    //    PhotonNetwork.LoadLevel("CharacterScene_LHS");
    //}

    void RoomIcon()
    {

    }
}
