using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButtonManager : MonoBehaviour
{
    public Transform room;
    public List<GameObject> roomList;

    public GameObject popular;
    public GameObject newest;
    public GameObject myRoom;

    private void Start()
    {
        roomList = new List<GameObject>();

        for (int i = 0; i < room.childCount; ++i)
        {
            roomList.Add(room.GetChild(0).gameObject);
        }

        popular.SetActive(true);
        newest.SetActive(false);
        myRoom.SetActive(false);
    }

    public void ShowPopluar()
    {
        popular.SetActive(true);
        newest.SetActive(false);
        myRoom.SetActive(false);
    }

    public void ShowNewest()
    {
        popular.SetActive(false);
        newest.SetActive(true);
        myRoom.SetActive(false);
    }

    public void ShowMyRoom()
    {
        popular.SetActive(false);
        newest.SetActive(false);
        myRoom.SetActive(true);
    }
}
