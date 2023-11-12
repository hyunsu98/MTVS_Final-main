using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;
using System;
using System.IO;

[Serializable]
public class SpriteInfo
{
    public string roomName;
    public string fileName;
}

[Serializable]
public class SpriteInfoList
{
    public List<SpriteInfo> sprites;
}

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    public RoomInfoLoadJson room;
    //public List<GameObject> roomItemList = new List<GameObject>();
    public Dictionary<string, string> thumbsDic = new Dictionary<string, string>();
    public Sprite temp;
    
    public bool isFirst;
    public bool isInRoom;

    public string filePath;
    
    LobbyManager_CSU lobbyMgr;

    public string flag;
    public string fileName;

    private void Awake()
    {
        if (instance == null)
        {
            //instance�� �� �ڽ��� �ִ´�.
            instance = this;

            //���� �ٲ��� �ı����� �ʰ� �Ѵ�.
            DontDestroyOnLoad(gameObject);

        }

        //���࿡ instance�� ���� �ִٸ�(�̹� ������� HttpManager�� ���� �Ѵٸ�)
        else
        {
            print("�ߺ����� �����Ѵ�! �ı��϶�!");
            //�ı�����
            Destroy(gameObject);

        }
    }

    private void Update()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LobbyScene")
        {
            isInRoom = false;
            lobbyMgr = GameObject.Find("LobbyManager").GetComponent<LobbyManager_CSU>();
        }
        else if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "CharacterScene_LHS")
        {
            isInRoom = false;
        }
        else
        {
            if (isInRoom) return;

            isInRoom = true;
            if (room.memberNickname != HttpManager.instance.nickname)
            {
                CheckNickName();
            }
        }
    }

    public void CheckNickName()
    {
        PublicObjectManager.instance.bottomCenter.gameObject.SetActive(false);
    }


    #region ����� ���ε�
    public void ShowFileBrowser()
    {
        // ���Ϻ����� ���� ����
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".jpg");

        // ����ũ (���� ���ã��)
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            // 2D �̹��� �����
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(bytes);

            temp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);

            fileName = FileBrowserHelpers.GetFilename(FileBrowser.Result[0]);
            filePath = Application.streamingAssetsPath + "/Sprite/" + fileName;
            Debug.Log(fileName);

            File.WriteAllBytes(filePath, bytes);
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], filePath);

            lobbyMgr.thumbs.sprite = temp;
        }
    }
    #endregion
}
