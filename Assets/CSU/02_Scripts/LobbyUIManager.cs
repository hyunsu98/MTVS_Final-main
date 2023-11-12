using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;

    // ù ��° scroll view �� content
    public RectTransform scrollContent;

    // ���� ��ų UI
    [Header("< Fixed UI >")]
    public GameObject holdMenuBar;
    public List<Text> fixedBtnText;

    // ���� UI
    [Header("< Hide UI >")]
    public GameObject panelTop;
    public List<Text> hideBtnText;

    [Header("< RoomItem >")]
    public List<MyRoomInfo> roomInfoList = new List<MyRoomInfo>();
    public List<GameObject> roomInfoUI = new List<GameObject>();
    public Transform roomItemParent;

    [Header("< ETC >")]
    public bool set = false;
    public string flag;

    public string FLAG
    {
        get { return flag; }
        set { flag = value; }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        ChangeSelected(0);
    }

    void Update()
    {
        MenuHold();
    }

    // ��ġ �ʱ�ȭ �Լ� (Top ��ư)
    public void InitializeRectPosition()
    {
        scrollContent.anchoredPosition = Vector3.zero;
    }

    // ���ο� �޴�â�� ���ִ� �Լ�
    public void MenuHold()
    {
        // ž �޴� ����� �� ���̱�
        if(scrollContent.anchoredPosition.y >= 600f)
        {
            panelTop.SetActive(false);
        }
        else
        {
            panelTop.SetActive(true);
        }

        // ���� UI ���̱� �� �����
        if (scrollContent.anchoredPosition.y >= 725f)
        {
            holdMenuBar.SetActive(true);
        }
        else
        {
            holdMenuBar.SetActive(false);
        }
    }

    // �޴� UI (outline & color) ����
    public void ChangeSelected(int idx)
    {
        for(int i = 0; i < fixedBtnText.Count; ++i)
        {
            if (i == idx) continue;
            fixedBtnText[i].color = Color.white;
            hideBtnText[i].color = Color.white;
        }

        fixedBtnText[idx].color = Color.black;
        hideBtnText[idx].color = Color.black;
    }

    // RoomItem ����
    public void ChangeUIList(string s)
    {
        if(s == "Popular")
        {
            roomInfoList.Sort(delegate (MyRoomInfo A, MyRoomInfo B)
            {
                if (A.like < B.like) return 1;
                else if(A.like > B.like) return -1;
                else
                {
                    if(A.view < B.view) return 1;
                    else return -1;
                }
            });

            foreach (GameObject go in roomInfoUI)
            {
                go.SetActive(true);
            }

            for (int i = 0; i < roomInfoList.Count; ++i)
            {
                for(int j = 0; j < roomInfoUI.Count; ++j)
                {
                    if (roomInfoList[i].name == roomInfoUI[j].transform.GetChild(6).GetComponent<Text>().text)
                    {
                        roomInfoUI[j].transform.SetSiblingIndex(i);
                        break;
                    }
                }
            }
        }
        else if(s == "Newest")
        {

            roomInfoList.Sort(delegate (MyRoomInfo A, MyRoomInfo B)
            {
                if (A.roomId < B.roomId) return 1;
                else return -1;
            });

            foreach (GameObject go in roomInfoUI)
            {
                go.SetActive(true);
            }

            for (int i = 0; i < roomInfoList.Count; ++i)
            {
                for (int j = 0; j < roomInfoUI.Count; ++j)
                {
                    if (roomInfoList[i].name == roomInfoUI[j].transform.GetChild(6).GetComponent<Text>().text)
                    {
                        roomInfoUI[j].transform.SetSiblingIndex(i);
                        break;
                    }
                }
            }
        }
        //else if(s == "MyRoom")
        //{
        //    foreach(GameObject go in roomInfoUI)
        //    {
        //        if(HttpManager.instance.nickname != go.transform.GetChild(5).GetComponent<Text>().text)
        //        {
        //            go.SetActive(false);
        //        }
        //    }
        //}
    }
}
