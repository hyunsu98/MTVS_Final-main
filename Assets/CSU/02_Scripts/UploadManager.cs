using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

public class UploadManager : MonoBehaviour
{
    public GameObject imgPrefab;
    public GameObject content;
    ImageChangeManager imgMgr;

    private void Awake()
    {
        imgMgr = PublicObjectManager.instance.imageChangeManager.GetComponent<ImageChangeManager>();
    }

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
            for (int i = 0; i < FileBrowser.Result.Length; ++i)
            {
                Debug.Log(FileBrowser.Result[i]);

                byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

                // 2D 이미지 만들기
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(bytes);

                // 만든 이미지 List 에 추가
                imgMgr.spritesList.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100));

                // UI 버튼(이미지) Scroll View 에 넣어주기
                int idx = imgMgr.spritesList.Count - 1;
                GameObject go = Instantiate(imgPrefab);
                go.transform.parent = content.transform;
                go.GetComponent<Image>().sprite = imgMgr.spritesList[idx];

                // 버튼 누르면 이미지 바뀌게
                Button btn = go.GetComponent<Button>();
                btn.onClick.AddListener(() => ToDo(idx));
            }
        }
    }

    void ToDo(int idx)
    {
        imgMgr.SetImageChange(idx);
    }
}
