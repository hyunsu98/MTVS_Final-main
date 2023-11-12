using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestCtrl : MonoBehaviour
{
    public void Send()
    {
        StartCoroutine(UploadFile());
    }



    IEnumerator UploadFile()
    {
        string[] filePath = new string[2];
        filePath[0] = @"D:\UnityProject\PhotonVoiceTest_0\Assets\StreamingAssets\test.wav";
        filePath[1] = @"D:\UnityProject\PhotonVoiceTest_0\Assets\StreamingAssets\test0.wav";

        UnityWebRequest[] files = new UnityWebRequest[2];
        WWWForm form = new WWWForm();

        files[0] = UnityWebRequest.Get(filePath[0]);
        files[1] = UnityWebRequest.Get(filePath[1]);

        foreach (var item in files)
        {
            yield return item.SendWebRequest();
        }

        form.AddBinaryData("file_0", files[0].downloadHandler.data, Path.GetFileName(filePath[0]));
        form.AddBinaryData("file_1", files[1].downloadHandler.data, Path.GetFileName(filePath[1]));

        string deb = "";
        foreach (var item in form.headers)
        {
            deb += item.Key + " : " + item.Value + "\n";
        }
        Debug.Log(deb);

        using (UnityWebRequest req = UnityWebRequest.Post("http://127.0.0.1:8888/download", form))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log("Uploaded files Successfully!");
            }
        }
    }
}