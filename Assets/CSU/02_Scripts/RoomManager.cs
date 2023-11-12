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
            //instance에 나 자신을 넣는다.
            instance = this;

            //씬이 바껴도 파괴되지 않게 한다.
            DontDestroyOnLoad(gameObject);

        }

        //만약에 instance에 값이 있다면(이미 만들어진 HttpManager가 존재 한다면)
        else
        {
            print("중복으로 생성한다! 파괴하라!");
            //파괴하자
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


    #region 썸네일 업로드
    public void ShowFileBrowser()
    {
        // 파일브라우저 필터 설정
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".jpg");

        // 퀵링크 (폴더 즐겨찾기)
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

            // 2D 이미지 만들기
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
