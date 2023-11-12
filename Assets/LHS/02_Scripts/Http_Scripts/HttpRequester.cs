using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region �����
//�Խù� ����
[Serializable]
public class PostData
{
    public int userId;
    public int id;
    public string title;
    public string body;
}

[Serializable]
public class PostDataArray
{
    public List<PostData> data;
}

public class UserData
{
    public string name;
    public string id;
    public string email;
    public int age;

    /*
    * {
    *      "name":"������",
    *      "id":"rapa_xr",
    *      "email":"lokimve7@naver.com",
    *      "age":20
    * }
    */
}
#endregion


public class HttpRequester : MonoBehaviour
{
    //��û Ÿ�� (GET, POST, PUT, DELETE)
    public RequestType requestType;
    //URL
    public string url;
    //Post Data ������ �־ �����ּ���
    public string body = "{}";

    //������ ���� �� ȣ������ �Լ� (Action)
    //Action : �Լ��� ���� �� �ִ� �ڷ���
    public Action<DownloadHandler> onComplete;
    public Action onFailed;

    public bool isJson;
    public bool isChat;

    public void SetUrl(RequestType type, string strUrl, bool bUseCommonUrl = true)
    {
        requestType = type;

        // ȸ������ / ���̵� / �̸��� / �г��� / �α��� �ּҰ� ����
        if (bUseCommonUrl) url = "http://remembermebackend-env.eba-dcctnmvk.ap-northeast-2.elasticbeanstalk.com";

        url += strUrl;
    }

  
    //����
    public void OnComplete(DownloadHandler result)
    {
        if (onComplete != null) onComplete(result);
    }

    public void OnFailed()
    {
        if (onFailed != null) onFailed();
    }


}


