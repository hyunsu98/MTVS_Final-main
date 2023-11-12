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
            for (int i = 0; i < FileBrowser.Result.Length; ++i)
            {
                Debug.Log(FileBrowser.Result[i]);

                byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

                // 2D �̹��� �����
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(bytes);

                // ���� �̹��� List �� �߰�
                imgMgr.spritesList.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100));

                // UI ��ư(�̹���) Scroll View �� �־��ֱ�
                int idx = imgMgr.spritesList.Count - 1;
                GameObject go = Instantiate(imgPrefab);
                go.transform.parent = content.transform;
                go.GetComponent<Image>().sprite = imgMgr.spritesList[idx];

                // ��ư ������ �̹��� �ٲ��
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
