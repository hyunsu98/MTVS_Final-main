using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnClickGetPost()
    {
        ////서버에 게시물 조회 요청(/posts/1 , GET)
        ////HttRequester를 생성
        //HttpRequester requester = new HttpRequester();

        /////posts/1 , GET, 완료되었을 때 호출되는 함수
        //requester.url = "https://jsonplaceholder.typicode.com/posts/1";
        //requester.requestType = RequestType.GET;
        //requester.onComplete = OnCompleteGetPost;

        ////HttpManager에게 요청
        //HttpManager.instance.SendRequest(requester);

        string url = "https://8c49-119-194-163-123.jp.ngrok.io/chat_bot?chat_request=";
        url += "chatText";
        url += "&user_id=" + 1;
        url += "&we_id=" + 1;

        HttpRequester requester = new HttpRequester();
        requester.SetUrl(RequestType.GET, url, false);

        HttpManager.instance.SendRequest(requester);
    }

    //public void OnCompleteGetPost(DownloadHandler handler)
    //{
    //    PostData postData = JsonUtility.FromJson<PostData>(handler.text);
    //    //타이틀  UI에 출력
    //    //내용 UI에 출력
    //    print("조회 완료");
    //}

    //public void OnClickGetPostAll()
    //{
    //    //서버에 게시물 조회 요청(/posts/1 , GET)
    //    //HttRequester를 생성
    //    HttpRequester requester = new HttpRequester();

    //    ///posts/1 , GET, 완료되었을 때 호출되는 함수
    //    requester.url = "https://jsonplaceholder.typicode.com/posts";
    //    requester.requestType = RequestType.GET;
    //    requester.onComplete = OnCompleteGetPostAll;

    //    //HttpManager에게 요청
    //    HttpManager.instance.SendRequest(requester);
    //}

    //public void OnCompleteGetPostAll(DownloadHandler handler)
    //{
    //    //배열 데이터를 키값에 넣는다.
    //    string s = "{\"data\":" + handler.text + "}";

    //    //List<PostData>
    //    PostDataArray array = JsonUtility.FromJson<PostDataArray>(s);
    //    for (int i = 0; i < array.data.Count; i++)
    //    {
    //        print(array.data[i].id + "\n" + array.data[i].title + "\n" + array.data[i].body);
    //    }

    //    print("조회 완료");
    //}

    //public void OnClickSignIn()
    //{
    //    //서버에 게시물 조회 요청(/posts/1 , GET)
    //    //HttRequester를 생성
    //    HttpRequester requester = new HttpRequester();

    //    ///user , POST, 완료되었을 때 호출되는 함수
    //    requester.url = "https://jsonplaceholder.typicode.com/user";
    //    requester.requestType = RequestType.POST;

    //    //post data 셋팅
    //    UserData data = new UserData();
    //    data.name = "김현진";
    //    data.email = "lokimve7@naver.com";
    //    data.id = "rapa_xr";
    //    data.age = 20;

    //    requester.postData = JsonUtility.ToJson(data, true);
    //    print(requester.postData);

    //    requester.onComplete = OnCompleteSignIn;

    //    //HttpManager에게 요청
    //    HttpManager.instance.SendRequest(requester);
    //}

    //public void OnCompleteSignIn(DownloadHandler handler)
    //{
    //    //배열 데이터를 키값에 넣는다.
    //    string s = "{\"data\":" + handler.text + "}";

    //    //List<PostData>
    //    PostDataArray array = JsonUtility.FromJson<PostDataArray>(s);
    //    for (int i = 0; i < array.data.Count; i++)
    //    {
    //        print(array.data[i].id + "\n" + array.data[i].title + "\n" + array.data[i].body);
    //    }

    //    print("조회 완료");
    //}
}
