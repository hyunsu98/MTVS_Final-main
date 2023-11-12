using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.IO;

public class UploadManager_LHS : MonoBehaviour
{
    //public GameObject imgPrefab;
    //public GameObject content;
    //ImageChangeManager imgMgr;

    //private void Awake()
    //{
    //    imgMgr = PublicObjectManager.instance.imageChangeManager.GetComponent<ImageChangeManager>();
    //}

    public GameObject restoreTwo;
    public GameObject restoreThree;


    public void ShowFileBrowser()
    {
        // ���Ϻ����� ���� ����
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Text", ".txt"));
        FileBrowser.SetDefaultFilter(".txt");

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
            }

            print("Text ���ε� �Ϸ�");

            restoreTwo.SetActive(false);
            restoreThree.SetActive(true);

           
        }
    }

    //void ToDo(int idx)
    //{
    //    imgMgr.SetImageChange(idx);
    //}
}
