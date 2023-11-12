using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonManager : MonoBehaviour
{
    string flieName, saveFileName;

    void LoadJson()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/" + saveFileName + ".txt");
        ArrayJson arrayJson = JsonUtility.FromJson<ArrayJson>(jsonData);
        for(int i= 0; i<arrayJson.datas.Count; ++i)
        {
            SaveJsonInfo info = arrayJson.datas[i];
            // 오브젝트 생성

        }
    }

    void SaveJson()
    {
        SaveJsonInfo info;

        ArrayJson arrayJson = new ArrayJson();
        arrayJson.datas = new List<SaveJsonInfo>();

        ObjectManager objMgr = PublicObjectManager.instance.objectManager.GetComponent<ObjectManager>();

        for(int i=0; i<objMgr.createdObj.Count; ++i)
        {
            CreatedInfo ci = objMgr.createdObj[i];

        }
    }
}
