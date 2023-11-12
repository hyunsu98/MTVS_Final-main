using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region 참고용
//게시물 정보
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
    *      "name":"김현진",
    *      "id":"rapa_xr",
    *      "email":"lokimve7@naver.com",
    *      "age":20
    * }
    */
}
#endregion


public class HttpRequester : MonoBehaviour
{
    //요청 타입 (GET, POST, PUT, DELETE)
    public RequestType requestType;
    //URL
    public string url;
    //Post Data 정보를 넣어서 보내주세요
    public string body = "{}";

    //응답이 왔을 때 호출해줄 함수 (Action)
    //Action : 함수를 넣을 수 있는 자료형
    public Action<DownloadHandler> onComplete;
    public Action onFailed;

    public bool isJson;
    public bool isChat;

    public void SetUrl(RequestType type, string strUrl, bool bUseCommonUrl = true)
    {
        requestType = type;

        // 회원가입 / 아이디 / 이메일 / 닉네임 / 로그인 주소가 같음
        if (bUseCommonUrl) url = "http://remembermebackend-env.eba-dcctnmvk.ap-northeast-2.elasticbeanstalk.com";

        url += strUrl;
    }

  
    //실행
    public void OnComplete(DownloadHandler result)
    {
        if (onComplete != null) onComplete(result);
    }

    public void OnFailed()
    {
        if (onFailed != null) onFailed();
    }


}


