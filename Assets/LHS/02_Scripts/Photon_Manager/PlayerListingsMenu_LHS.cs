using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerListingsMenu_LHS : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    //public RectTransform trContent;

    [SerializeField]
    private PlayerListing_LHS _playerListing;

    public List<PlayerListing_LHS> _listings = new List<PlayerListing_LHS>();

    public GameObject roomOption;

    //�� �̸�
    [SerializeField]
    private Text roomNum;

    //�濡 �ִ� ����� ��
    [SerializeField]
    private Text roomNumInfo;

    [SerializeField]
    private Text roomView;

    [SerializeField]
    private Text roomHeart;

    [SerializeField]
    private Text roomName;

    //ChatAreaScript LastArea;
    //ChatAreaScript Area;

    //[Header("*ChatItem*")]
    //public Scrollbar scrollBar;

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    private void GetCurrentRoomPlayers()
    {
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
       
        //bool isBottom = scrollBar.value <= 0.00001f;

        PlayerListing_LHS listing = Instantiate(_playerListing, _content);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            _listings.Add(listing);

            #region ä�� ���� ����
            //Area = listing.GetComponent<ChatAreaScript>();
            ////Area.transform.SetParent(_content.transform, false);
            //////���� �ִ� 600, ���̴� ����
            ////Area.BoxRect.sizeDelta = new Vector2(500, Area.BoxRect.sizeDelta.y);
            //Fit(Area.BoxRect);

            //// �� �� �̻��̸� ũ�⸦ �ٿ����鼭, �� ���� �Ʒ��� �������� �ٷ� �� ũ�⸦ ���� 
            ////float X = Area.TextRect.sizeDelta.x + 42;
            ////float Y = Area.TextRect.sizeDelta.y;
            ////if (Y > 49)
            ////{
            ////    for (int i = 0; i < 200; i++)
            ////    {
            ////        Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
            ////        Fit(Area.BoxRect);

            ////        if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            ////    }
            ////}
            ////else Area.BoxRect.sizeDelta = new Vector2(X, Y);

            ////string nick = "����";
            ////Area.User = nick;

            //Fit(Area.BoxRect);
            //Fit(Area.AreaRect);
            //Fit(trContent);
            //LastArea = Area;

            //// ��ũ�ѹٰ� ���� �ö� ���¿��� �޽����� ������ �� �Ʒ��� ������ ����
            //if (!isBottom) return;
            //Invoke("ScrollDelay", 0.03f);

            #endregion
        }
    }

    //void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    //void ScrollDelay() => scrollBar.value = 0;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //base.OnPlayerEnteredRoom(newPlayer);
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //base.OnPlayerLeftRoom(otherPlayer);
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if(index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    private void Update()
    {
        ChatAreaScript Area = roomOption.GetComponent<ChatAreaScript>();
        // ���� �� �ο���
        Area.RoomNum.text = PhotonNetwork.CurrentRoom.PlayerCount + ")";

        roomView.text = RoomManager.instance.room.roomViews.ToString();
        roomHeart.text = RoomManager.instance.room.roomLikes.ToString();
        roomName.text = RoomManager.instance.room.roomName.ToString();


        //print("����" + PhotonNetwork.CountOfPlayersInRooms);

        //print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
        //print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
}
