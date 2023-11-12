using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gara : MonoBehaviour
{
    public static Gara instance;

    public Transform parent;

    public GameObject popular;
    public GameObject newest;
    public GameObject myRoom;
    
    public List<GameObject> roomList;
    public List<int> likeCount;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        likeCount = new List<int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            string s = "";
            s = roomList[i].transform.GetChild(3).GetComponentInChildren<Text>().text;
            likeCount.Add(int.Parse(s));
        }
    }

    public void SetPopular()
    {
        popular.SetActive(true);
        newest.SetActive(false);
        myRoom.SetActive(false);

        for (int i = 0; i < roomList.Count; ++i)
        {
            roomList[i].SetActive(true);
            roomList[i].transform.SetSiblingIndex(i);
        }
    }

    public void SetNewest()
    {
        popular.SetActive(false);
        newest.SetActive(true);
        myRoom.SetActive(false);
    }

    public void SetMyRoom()
    {
        popular.SetActive(false);
        newest.SetActive(false);
        myRoom.SetActive(true);
    }
}
