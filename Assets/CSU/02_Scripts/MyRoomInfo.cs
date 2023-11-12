using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Realtime;

[Serializable]
public class RoomInfoSaveJson
{
    public string username;
    public string roomname;
}

[Serializable]
public class RoomInfoLoadJson
{
    public int id;
    public string roomName;
    public string roomStatus;
    public string memberNickname;
    public int roomLikes;
    public int roomViews;
    public int memberId;
}

[Serializable]
public class ArrayRoomInfo
{
    public List<RoomInfoLoadJson> roomList;
}

[Serializable]
public class RoomInfoResult
{
    public RoomInfoJson results;
}

[Serializable]
public class RoomInfoJson
{
    public RoomInfoLoadJson room;
}

public class MyRoomInfo : MonoBehaviour
{
    // �����
    public Image thumbNail;
    public Button thumbNailButton;
    // ���� ������ ���� ��
    int currUserCount;
    Text textCurrentUserCount;
    // ��ȸ ��
    public int view;
    Text textView;
    // ���ƿ� ��
    public int like;
    Text textLike;
    // �� ���� �г���
    public string hostNickname;
    Text textHostNickname;
    // �� �̸�
    public string roomName;
    Text textRoomName;
    // �� id
    public int roomId;

    private void Awake()
    {
        thumbNail = transform.GetChild(0).GetComponent<Image>();
        thumbNailButton = transform.GetChild(0).GetComponent<Button>();

        textCurrentUserCount = transform.GetChild(1).GetComponentInChildren<Text>();
        textView = transform.GetChild(2).GetComponentInChildren<Text>();
        textLike = transform.GetChild(3).GetComponentInChildren<Text>();

        textHostNickname = transform.GetChild(5).GetComponent<Text>();
        textRoomName = transform.GetChild(6).GetComponent<Text>();
    }

    #region ���� ���� ��, ��ȸ��, ���ƿ� �� ��� �� UI ���� �Լ�
    // ���� ���� ��
    public void CalculateUserCount(int count)
    {
        currUserCount += count;
        SynchronizedUserCount(currUserCount);
    }

    void SynchronizedUserCount(int count)
    {
        textCurrentUserCount.text = count.ToString();
    }

    // ��ȸ��
    public void CalculateViewCount(int count)
    {
        view += count;
        SynchronizedViewCount(view);
    }

    void SynchronizedViewCount(int count)
    {
        textView.text = count.ToString();
    }

    // ���ƿ�
    public void CalculateLikeCount(int count)
    {
        like += count;
        SynchronizedLikeCount(like);
    }

    void SynchronizedLikeCount(int count)
    {
        textLike.text = count.ToString();
    }
    #endregion

    // RoomInfo �� ���� ����
    public void SetInfo(RoomInfo info)
    {
        hostNickname = (string)info.CustomProperties["nickname"];
        textHostNickname.text = hostNickname;

        roomName = (string)info.CustomProperties["room_name"];
        textRoomName.text = roomName;

        like = (int)info.CustomProperties["like"];
        textLike.text = like.ToString();

        view = (int)info.CustomProperties["view"];
        textView.text = view.ToString();
    }

    public void SetInfo(string _nickname, string _roomName, int _like, int _view, int _id)
    {
        hostNickname = _nickname;
        textHostNickname.text = hostNickname;

        roomName = _roomName;
        textRoomName.text = roomName;

        like = _like;
        textLike.text = like.ToString();

        view = _view;
        textView.text = view.ToString();

        roomId = _id;
    }
}
